// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("StatProjectUploadingV2")]
    public class StatProjectUploadingV2Entity : StatEntity
    {
        public string EventId { get; set; }

        public string ProjectId { get; set; }

        public string UserId { get; set; }

        public string ProductName { get; set; }

        public DateTime DateTime { get; set; }

        public string IdentityProvider { get; set; }

        public string ProductVersion { get; set; }

        public int ProjectType { get; set; }

        public int TagType { get; set; }

        public string ProjectName { get; set; }
    }
}