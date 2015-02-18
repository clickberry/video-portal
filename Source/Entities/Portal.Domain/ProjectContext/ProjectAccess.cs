// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.ProjectContext
{
    public enum ProjectAccess
    {
        /// <summary>
        ///     Visible to all.
        /// </summary>
        Public = 0,

        /// <summary>
        ///     Can be retrieved by direct link.
        /// </summary>
        Hidden = 1,

        /// <summary>
        ///     Restricted access to owner.
        /// </summary>
        Private = 2
    }
}