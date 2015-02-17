// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.ProfileContext;
using Portal.DTO.Watch;

namespace Portal.Web.Models
{
    /// <summary>
    ///     Video view model.
    /// </summary>
    public class VideoModel
    {
        /// <summary>
        ///     Watch video.
        /// </summary>
        public Watch Video { get; set; }

        /// <summary>
        ///     Video URL.
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        ///     Video secure URL.
        /// </summary>
        public string VideoSecureUrl { get; set; }

        /// <summary>
        ///     Embed URL.
        /// </summary>
        public string EmbedUrl { get; set; }

        /// <summary>
        ///     Embed secure URL.
        /// </summary>
        public string EmbedSecureUrl { get; set; }

        /// <summary>
        ///     Project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Project image.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        ///     Project image secure URL.
        /// </summary>
        public string ImageSecure { get; set; }

        /// <summary>
        ///     Current user's avatar url.
        /// </summary>
        public string UserAvatarUrl { get; set; }

        /// <summary>
        /// Gets a robot.
        /// </summary>
        public ProviderType Robot { get; set; }
    }
}