// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.PortalContext
{
    public class DomainRoles
    {
        /// <summary>
        ///     Portal administrator.
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        ///     Default system administrator.
        /// </summary>
        public const string SuperAdministrator = "SuperAdministrator";

        /// <summary>
        ///     Video examples manager.
        /// </summary>
        public const string ExamplesManager = "ExamplesManager";

        /// <summary>
        ///     User.
        /// </summary>
        public const string User = "User";

        /// <summary>
        ///     Client.
        /// </summary>
        public const string Client = "Client";

        public const string AllAdministrators = SuperAdministrator + "," + Administrator;
        public const string AllViewers = User + "," + Client;

        public static readonly string[] AllAdminRoles = { SuperAdministrator, Administrator, ExamplesManager };
        public static readonly string[] AllRoles = { SuperAdministrator, Administrator, ExamplesManager, User, Client };
    }
}