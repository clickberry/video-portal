// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.DTO.Projects
{
    public class Comment
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public virtual string Body { get; set; }

        public DateTime DateTime { get; set; }
    }
}