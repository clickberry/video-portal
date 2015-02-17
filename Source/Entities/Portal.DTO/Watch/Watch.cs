// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.DTO.Projects;

namespace Portal.DTO.Watch
{
    /// <summary>
    ///     Public project data.
    /// </summary>
    public class Watch : Project
    {
        /// <summary>
        ///     Gets or sets an avsx URI.
        /// </summary>
        public string Avsx { get; set; }

        /// <summary>
        ///     Gets or sets a value idicating whether project is editable by user.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        ///     Gets a videos.
        /// </summary>
        public List<WatchVideo> Videos { get; set; }

        /// <summary>
        ///     Gets or sets a screenshots.
        /// </summary>
        public List<WatchScreenshot> Screenshots { get; set; }

        /// <summary>
        ///     Gets or sets a project hits count
        /// </summary>
        public long HitsCount { get; set; }

        /// <summary>
        ///     Gets or sets an external video.
        /// </summary>
        public ExternalVideo External { get; set; }

        /// <summary>
        ///     Gets or sets a custom screenshot URI.
        /// </summary>
        public string ScreenshotUrl { get; set; }

        /// <summary>
        ///     Gets or sets a current video state.
        /// </summary>
        public WatchState State { get; set; }

        /// <summary>
        ///     Gets or sets a project generator identifier.
        /// </summary>
        public int Generator { get; set; }

        /// <summary>
        ///     Creator's user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Creator's user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Creator's user avatar.
        /// </summary>
        public string UserAvatarUrl { get; set; }

        /// <summary>
        ///     Gets or sets comments count.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        ///     Gets or sets project comments (or most recent comments).
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        ///     Gets or sets likes count.
        /// </summary>
        public long LikesCount { get; set; }

        /// <summary>
        ///     Gets or sets dislikes count.
        /// </summary>
        public long DislikesCount { get; set; }

        /// <summary>
        ///     Gets or sets value indicating whether project has been liked by requesting user or not.
        /// </summary>
        public bool IsLiked { get; set; }

        /// <summary>
        ///     Gets or sets value indicating whether project has been disliked by requesting user or not.
        /// </summary>
        public bool IsDisliked { get; set; }
    }
}