// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Portal.DAL.NotificationHub
{
    /// <summary>
    ///     Client for accessing Azure Service Bus Notification Hub.
    /// </summary>
    public interface IClientNotificationHub
    {
        /// <summary>
        ///     Sends notification.
        /// </summary>
        /// <param name="notification">Notification instance.</param>
        Task SendNotificationAsync(HubNotification notification);
    }
}