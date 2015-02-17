// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.Domain.ProjectContext;

namespace Portal.DTO.Admin
{
    public sealed class AdminProject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public ProductType ProductType { get; set; }

        public string Product { get; set; }
    }
}