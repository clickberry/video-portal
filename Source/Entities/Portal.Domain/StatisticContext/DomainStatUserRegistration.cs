// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.StatisticContext
{
    public class DomainStatUserRegistration
    {
        public string Tick { get; set; }

        public string EventId { get; set; }

        public string UserId { get; set; }

        public string ProductName { get; set; }

        public string IdentityProvider { get; set; }

        public DateTime DateTime { get; set; }
    }
}