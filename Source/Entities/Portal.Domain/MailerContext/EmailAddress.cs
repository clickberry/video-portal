// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.MailerContext
{
    /// <summary>
    ///     E-mail address entity.
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        ///     Default constructor.
        /// </summary>
        public EmailAddress()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="emailAddress">E-mail address.</param>
        /// <param name="displayName">User display name.</param>
        public EmailAddress(string emailAddress, string displayName)
        {
            Address = emailAddress;
            DisplayName = displayName;
        }

        /// <summary>
        ///     Gets or sets an user display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets an user e-mail address.
        /// </summary>
        public string Address { get; set; }
    }
}