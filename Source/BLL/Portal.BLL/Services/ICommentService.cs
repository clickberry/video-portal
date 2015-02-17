// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Services
{
    public interface ICommentService
    {
        Task<List<DomainComment>> GetCommentsAsync(string projectId, string userId);

        Task<DomainComment> AddCommentAsync(DomainComment domainComment);

        Task<DomainComment> EditCommentAsync(DomainComment domainComment);

        Task DeleteCommentAsync(DomainComment comment);
    }
}