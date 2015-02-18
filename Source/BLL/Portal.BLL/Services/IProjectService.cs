// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Services
{
    public interface IProjectService
    {
        Task<DomainProject> AddAsync(DomainProject entity);

        Task<List<DomainProject>> AddAsync(IList<DomainProject> entity);

        Task<DomainProject> GetAsync(DomainProject entity);

        Task<List<DomainProject>> GetListAsync(DomainProject entity);

        Task<int> GetSitemapPageCountAsync();

        Task<List<DomainProject>> GetSitemapPageAsync(int pageNumber);

        Task<DomainProject> EditAsync(DomainProject entity);

        Task DeleteAsync(DomainProject entity);

        Task DeleteAsync(IList<DomainProject> entity);

        Task IncrementHitsCounterAsync(string projectId);

        Task UpdateLikesCounterAsync(string projectId, long count);

        Task UpdateDislikesCounterAsync(string projectId, long count);

        Task IncrementAbuseCounterAsync(string projectId);

        Task<List<DomainProject>> GetProjectListByUsersAsync(IEnumerable<DomainUser> users);
    }
}