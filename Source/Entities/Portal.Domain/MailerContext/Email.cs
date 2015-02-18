// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.Domain.MailerContext
{
    public sealed class Email : EmailBase
    {
        public Email(string subject, string content, string contentType, EmailAddress from)
        {
            Subject = subject;
            Content = content;
            ContentType = contentType;
            From = from;

            To = new List<EmailAddress>();
            Cc = new List<EmailAddress>();
            Bcc = new List<EmailAddress>();
            Headers = new List<string>();
        }
    }
}