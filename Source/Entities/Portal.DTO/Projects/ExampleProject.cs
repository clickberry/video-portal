// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Portal.DTO.Projects
{
    /// <summary>
    ///     Example project entity.
    /// </summary>
    public sealed class ExampleProject
    {
        /// <summary>
        ///     Gets or sets a project identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets a video URI.
        /// </summary>
        public string VideoUri { get; set; }

        /// <summary>
        ///     Gets or sets a video file hash.
        /// </summary>
        [Obsolete("For compatibility with iOS tagger")]
        public string VideoHash { get; set; }

        /// <summary>
        ///     Gets or sets an avsx URI.
        /// </summary>
        public string AvsxUri { get; set; }

        /// <summary>
        ///     Gets or sets a screenshot URI.
        /// </summary>
        public string ScreenshotUri { get; set; }

        /// <summary>
        ///     Gets or sets a total example files size.
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        ///     Gets or sets a number of project views.
        /// </summary>
        public long Views { get; set; }

        /// <summary>
        ///     Gets or sets a number of project likes.
        /// </summary>
        public long Likes { get; set; }

        /// <summary>
        ///     Gets or sets a number of project dislikes.
        /// </summary>
        public long Dislikes { get; set; }

        /// <summary>
        ///     Gets or sets a project type.
        /// </summary>
        public int ProjectType { get; set; }

        /// <summary>
        ///     Gets or sets a project subtype.
        /// </summary>
        public int ProjectSubtype { get; set; }

        /// <summary>
        ///     Gets or sets comments count.
        /// </summary>
        public int Comments { get; set; }

        /// <summary>
        ///     Gets or sets project comments (or most recent comments).
        /// </summary>
        public List<Comment> CommentList { get; set; }
    }
}