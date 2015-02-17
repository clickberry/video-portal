// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.Domain.MailerContext
{
    public abstract class EmailBase
    {
        public virtual List<EmailAddress> To { get; protected set; }
        public virtual List<EmailAddress> Cc { get; protected set; }
        public virtual List<EmailAddress> Bcc { get; protected set; }
        public virtual string Subject { get; protected set; }
        public virtual string Content { get; protected set; }
        public virtual string ContentType { get; protected set; }
        public virtual EmailAddress From { get; protected set; }
        public virtual List<string> Headers { get; protected set; }
    }
}