// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Domain.ProjectContext;

namespace Portal.Domain.Admin
{
    public sealed class DomainProjectForAdmin
    {
        public string ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public ProductType ProductType { get; set; }

        public string Product { get; set; }
    }
}