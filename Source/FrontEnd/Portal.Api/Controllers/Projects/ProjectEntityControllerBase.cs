// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.Domain.ProjectContext;

namespace Portal.Api.Controllers.Projects
{
    /// <summary>
    ///     Abstract project entity controller.
    /// </summary>
    public abstract class ProjectEntityControllerBase : ApiControllerBase
    {
        private readonly IProjectService _projectRepository;

        protected ProjectEntityControllerBase(IProjectService projectRepository)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        ///     Gets a project by identifier.
        /// </summary>
        /// <param name="id">Project id.</param>
        /// <returns>Project entity.</returns>
        protected async Task<DomainProject> GetProjectAsync(string id)
        {
            return await _projectRepository.GetAsync(new DomainProject { Id = id, UserId = UserId });
        }
    }
}