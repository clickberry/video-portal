// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.DTO.Email
{
    public class SendEmail
    {
        /// <summary>
        ///     List of contacts who will receive a mail with a tag
        /// </summary>
        public virtual List<string> Emails { get; set; }

        /// <summary>
        ///     Subject. Pretty self explaining
        /// </summary>
        public virtual string Subject { get; set; }

        /// <summary>
        ///     The message body.
        /// </summary>
        public virtual string Body { get; set; }
    }
}