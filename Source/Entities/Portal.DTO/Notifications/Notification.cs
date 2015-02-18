// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DTO.Notifications
{
    /// <summary>
    ///     Push notification.
    /// </summary>
    public class Notification
    {
        /// <summary>
        ///     Gets or sets a project identifier.
        /// </summary>
        public virtual string ProjectId { get; set; }

        /// <summary>
        ///     Gets or sets a message title.
        /// </summary>
        public virtual string Title { get; set; }
    }
}