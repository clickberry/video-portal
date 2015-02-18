// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.DAL.Entities.Table
{
    public class TrackingStatPerDateEntity
    {
        public DateTime Date { get; set; }

        public long Count { get; set; }
    }
}