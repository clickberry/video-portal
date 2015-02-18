// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.DAL.Mailer;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Mailer
{
    public class EmailInitializer : IEmailInitializer
    {
        public void Initialize(IEmailBuilder emailBuilder, EmailBase email)
        {
            emailBuilder.SetSender(email.From);
            emailBuilder.SetContent(email.Subject, email.Content, email.ContentType);

            if (email.Headers.Count > 0)
                emailBuilder.AddToWithHeader(email.Headers);

            foreach (EmailAddress emailAddress in email.To)
            {
                emailBuilder.AddToRecipient(emailAddress);
            }
            foreach (EmailAddress emailAddress in email.Cc)
            {
                emailBuilder.AddCcRecipient(emailAddress);
            }
            foreach (EmailAddress emailAddress in email.Bcc)
            {
                emailBuilder.AddBccRecipient(emailAddress);
            }
        }
    }
}