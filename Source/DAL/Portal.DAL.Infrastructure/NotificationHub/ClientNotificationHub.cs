// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json;
using Portal.DAL.NotificationHub;
using Portal.Exceptions.CRUD;

namespace Portal.DAL.Infrastructure.NotificationHub
{
    /// <summary>
    ///     Azure Service Bus Notification Hub client.
    /// </summary>
    public sealed class ClientNotificationHub : IClientNotificationHub
    {
        private readonly NotificationHubClient _hub;

        public ClientNotificationHub(string connectionString, string hubName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(hubName))
            {
                throw new ArgumentNullException("hubName");
            }

            _hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);
        }

        public async Task SendNotificationAsync(HubNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException("notification");
            }

            string applePayload = CreateApplePayload(notification);
            string googlePayload = CreateGooglePayload(notification);

            try
            {
                await Task.WhenAll(new[]
                {
                    _hub.SendAppleNativeNotificationAsync(applePayload),
                    _hub.SendGcmNativeNotificationAsync(googlePayload)
                });
            }
            catch (MessagingException e)
            {
                throw new BadGatewayException(e);
            }
        }

        private string CreateApplePayload(HubNotification notification)
        {
            return JsonConvert.SerializeObject(new
            {
                aps = new
                {
                    alert = notification.Header
                }
            });
        }

        private string CreateGooglePayload(HubNotification notification)
        {
            return JsonConvert.SerializeObject(new
            {
                data = new
                {
                    message = notification.Header
                }
            });
        }
    }
}