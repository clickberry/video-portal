// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("StatProjectDeletionV2")]
    public class StatProjectDeletionV2Entity : StatEntity
    {
        public string EventId { get; set; }

        public string ProjectId { get; set; }

        public string UserId { get; set; }

        public string ProductName { get; set; }

        public DateTime DateTime { get; set; }
    }
}