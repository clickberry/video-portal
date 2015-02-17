// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Services
{
    public interface IProjectVideoService
    {
        Task<DomainVideo> AddAsync(string projectId, DomainVideo entity);

        Task<DomainVideo> GetAsync(string projectId);

        Task DeleteAsync(string projectId);
    }
}