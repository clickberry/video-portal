// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Configuration
{
    /// <summary>
    ///     Configuration settings names.
    /// </summary>
    public interface IPortalMiddleendSettings : IPortalSettings
    {
        /// <summary>
        ///     Bearer token value.
        /// </summary>
        string BearerToken { get; }

        /// <summary>
        ///     Defines whether mongodb migrations are enabled.
        /// </summary>
        bool MongoAutomigrationEnabled { get; }

        /// <summary>
        ///     Default administrator data.
        /// </summary>
        string DefaultAdministrator { get; }
    }
}