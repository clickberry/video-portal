// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Asp.Infrastructure.Validation
{
    public sealed class EmailAttribute : RegularExpressionAttribute
    {
        private const int MaxLen = 320;

        public EmailAttribute()
            : base("(?i)^(?!\\.)(\"([^\"\\r\\\\]|\\\\[\"\\r\\\\])*\"|([-a-z0-9!#$%&\'*+/=?^_`{|}~]|(?<!\\.)\\.)*)(?<!\\.)@[a-z0-9][\\w\\.-]*[a-z0-9]\\.[a-z][a-z\\.]*[a-z]$")
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                // can be null
                return true;
            }

            string[] parts = value.ToString().Split('@');
            if (parts.Length != 2)
            {
                return false; // There should be only one '@'
            }

            /* According to  RFC 5322 the part of email address that 
               precedes '@' symbol is called 'local part'. local part
               must have positive length but not more than 64 symbols
             */
            string localPart = parts[0];

            /* ...and other part is called 'domain part' that can't be longer than 190 symbols */
            string domainPart = parts[1];

            if (localPart.Length < 1 || localPart.Length > 64 || domainPart.Length > (MaxLen - 65))
            {
                return false;
            }
            return base.IsValid(value);
        }
    }
}