// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Twitter
{
    /// <summary>
    ///     Twitter user profile data.
    /// </summary>
    public class TwitterProfile
    {
        /// <summary>
        ///     Gets or sets an user identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets an user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets an user screenname.
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        ///     Gets or sets an user profile image URL.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}