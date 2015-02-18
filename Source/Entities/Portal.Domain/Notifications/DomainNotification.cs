// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.Notifications
{
    /// <summary>
    ///     Push notification.
    /// </summary>
    public sealed class DomainNotification
    {
        /// <summary>
        ///     Gets or sets a notification identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets a project identifier.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        ///     Gets or sets an user idetifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets a notification title.
        /// </summary>
        public string Title { get; set; }
    }
}