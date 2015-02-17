// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Configuration;
using Portal.DAL.Mailer;

namespace Portal.DAL.Infrastructure.Mailer.Wrappers
{
    public class SmtpClientWrapper : IMailClient
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper(MailSettings mailClientSettings)
        {
            _smtpClient = new SmtpClient
            {
                EnableSsl = mailClientSettings.EnableSsl,
                Host = mailClientSettings.Host,
                Port = mailClientSettings.Port,
                Timeout = mailClientSettings.Timeout,
                Credentials = new NetworkCredential(mailClientSettings.UserName, mailClientSettings.Password)
            };
        }

        public Task SendAsync(IEmailBuilder emailBuilder)
        {
            var message = (MailMessageBuilder)emailBuilder;

            try
            {
                _smtpClient.Send(message.GetResult());
            }
            finally
            {
                message.Dispose();
                _smtpClient.Dispose();
            }

            return Task.FromResult(0);
        }
    }
}