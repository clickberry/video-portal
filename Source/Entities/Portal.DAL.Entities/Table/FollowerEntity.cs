// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DAL.Entities.Table
{
    public class FollowerEntity
    {
        public string Id { get; set; }

        /// <summary>
        ///     Redundant follower name to quick list display.
        /// </summary>
        public string Name { get; set; }
    }
}