// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.Projects
{
    /// <summary>
    ///     External video entity.
    /// </summary>
    public sealed class ExternalVideo
    {
        /// <summary>
        ///     External video provider name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     External video source URL.
        /// </summary>
        public string VideoUri { get; set; }

        /// <summary>
        ///     Gets or sets an ACS namespace.
        /// </summary>
        public string AcsNamespace { get; set; }
    }
}