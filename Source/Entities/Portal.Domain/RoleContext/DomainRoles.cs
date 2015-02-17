using System;

namespace Portal.Domain.PortalContext
{
    public class DomainRoles
    {
        /// <summary>
        /// Portal administrator.
        /// </summary>
        public const string Administrator = "Administrator";
        
        /// <summary>
        /// Default system administrator.
        /// </summary>
        public const string SuperAdministrator = "SuperAdministrator";

        /// <summary>
        /// Video examples manager.
        /// </summary>
        public const string ExamplesManager = "ExamplesManager";

        public const string AllAdministrators = SuperAdministrator + "," + Administrator;

        public static readonly string[] AllRoles = new[] { SuperAdministrator, Administrator, ExamplesManager };
    }
}