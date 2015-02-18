// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.StatisticContext
{
    public class DomainMostSignaledItem
    {
        public string ItemId { get; set; }

        public long Count { get; set; }

        public long Version { get; set; }
    }
}