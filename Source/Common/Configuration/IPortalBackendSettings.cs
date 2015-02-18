// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Configuration
{
    /// <summary>
    ///     Configuration settings names.
    /// </summary>
    public interface IPortalBackendSettings : IPortalSettings
    {
        /// <summary>
        ///     Middle End address.
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        ///     Bearer token value.
        /// </summary>
        string BearerToken { get; }

        /// <summary>
        ///     Period.
        /// </summary>
        int Period { get; }

        /// <summary>
        ///     FfmpegVersion.
        /// </summary>
        string FfmpegVersion { get; }

        /// <summary>
        ///     TaskTypes.
        /// </summary>
        string TaskTypes { get; }

        /// <summary>
        ///     BackendId.
        /// </summary>
        string BackendId { get; }
    }
}