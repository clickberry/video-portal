// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("Comment")]
    public sealed class CommentEntity : Entity
    {
        public string ProjectId { get; set; }

        public string UserId { get; set; }

        public string Body { get; set; }

        public DateTime DateTime { get; set; }
    }
}