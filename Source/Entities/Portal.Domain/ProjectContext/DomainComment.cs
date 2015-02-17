// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.ProjectContext
{
    public class DomainComment
    {
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string Body { get; set; }

        public DateTime DateTime { get; set; }

        public string OwnerId { get; set; }
    }
}