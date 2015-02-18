// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DAL.Entities.AggregateObject
{
    public sealed class CommentsAggregation
    {
        public string ProjectId { get; set; }

        public int CommentsCount { get; set; }
    }
}