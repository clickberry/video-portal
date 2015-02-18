// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.DAL.Mailer;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Mailer
{
    public class MailerRepository : IMailerRepository
    {
        private readonly IEmailFactory _emailFactory;
        private readonly IEmailInitializer _emailInitializer;

        public MailerRepository(IEmailFactory emailFactory, IEmailInitializer emailInitializer)
        {
            _emailFactory = emailFactory;
            _emailInitializer = emailInitializer;
        }

        public Task SendMail(EmailBase email)
        {
            IMailClient mailClient = _emailFactory.CreateMailClient();
            IEmailBuilder emailBuilder = _emailFactory.CreateEmailBuilder();

            _emailInitializer.Initialize(emailBuilder, email);
            return mailClient.SendAsync(emailBuilder);
        }

        public Task SendMails(IEnumerable<EmailBase> mailList)
        {
            return Task.WhenAll(mailList.Select(email =>
            {
                IMailClient mailClient = _emailFactory.CreateMailClient();
                IEmailBuilder emailBuilder = _emailFactory.CreateEmailBuilder();

                _emailInitializer.Initialize(emailBuilder, email);
                return mailClient.SendAsync(emailBuilder);
            }));
        }
    }
}