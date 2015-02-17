// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Configuration;
using Portal.DAL.Infrastructure.Mailer.Wrappers;
using Portal.DAL.Mailer;

namespace Portal.DAL.Infrastructure.Mailer
{
    public class SmtpEmailFactory : IEmailFactory
    {
        private readonly MailSettings _mailClientSettings;

        public SmtpEmailFactory(MailSettings mailSettings)
        {
            _mailClientSettings = mailSettings;
        }

        public IEmailBuilder CreateEmailBuilder()
        {
            return new MailMessageBuilder();
        }

        public IMailClient CreateMailClient()
        {
            return new SmtpClientWrapper(_mailClientSettings);
        }
    }
}