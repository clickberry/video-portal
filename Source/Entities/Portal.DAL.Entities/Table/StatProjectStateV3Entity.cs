// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("StatProjectStateV3")]
    public class StatProjectStateV3Entity : Entity
    {
        public string ProjectId { get; set; }

        public string ActionType { get; set; }

        public DateTime DateTime { get; set; }

        public string Producer { get; set; }

        public string Version { get; set; }
    }
}