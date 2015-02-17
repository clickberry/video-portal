// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("VideoQueue")]
    public sealed class VideoQueueEntity : Entity
    {
        public string ProjectId { get; set; }

        public string FileId { get; set; }

        public string UserId { get; set; }


        [Obsolete("For backward compatibility only")]
        public string VideoFileHash { get; set; }
    }
}