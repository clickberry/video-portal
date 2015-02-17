// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Mailer
{
    public interface IEmailBuilder
    {
        void AddToRecipient(EmailAddress emailAddress);
        void AddCcRecipient(EmailAddress emailAddress);
        void AddBccRecipient(EmailAddress emailAddress);
        void AddToWithHeader(List<string> emailAddresses);
        void SetSender(EmailAddress emailAddress);
        void SetContent(string subject, string content, string contentType);
    }
}