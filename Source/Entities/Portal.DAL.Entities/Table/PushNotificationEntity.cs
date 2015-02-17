// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    /// <summary>
    ///     Push notification entity.
    /// </summary>
    [CollectionName("PushNotification")]
    public sealed class PushNotificationEntity : Entity
    {
        /// <summary>
        ///     Gets or sets a project identifier.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        ///     Gets or sets a creation date.
        /// </summary>
        public DateTime Created { get; set; }

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