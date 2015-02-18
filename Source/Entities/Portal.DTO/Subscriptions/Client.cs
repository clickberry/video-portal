// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DTO.Subscriptions
{
    /// <summary>
    ///     Client profile.
    /// </summary>
    public class Client
    {
        public virtual string Email { get; set; }

        public virtual string Country { get; set; }

        public virtual string CompanyName { get; set; }

        public virtual string Ein { get; set; }

        public virtual string Address { get; set; }

        public virtual string ZipCode { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string ContactPerson { get; set; }
    }
}