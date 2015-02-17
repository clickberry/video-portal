// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Portal.Domain.MailerContext
{
    public class SendEmailDomain
    {
        public string Id { get; set; }

        public EmailTemplate Template { get; set; }

        public DateTime Created { get; set; }

        public string UserId { get; set; }

        public List<string> Emails { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Address { get; set; }

        public string DisplayName { get; set; }
    }
}