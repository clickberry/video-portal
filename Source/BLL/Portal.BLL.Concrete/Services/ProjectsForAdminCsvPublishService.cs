// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters;
using Portal.BLL.Services;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.DTO.Admin;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public class ProjectsForAdminCsvPublishService : CsvPublishServiceBase
    {
        private readonly IAdminProjectService _adminProjectService;
        private readonly IMapper _mapper;

        public ProjectsForAdminCsvPublishService(CloudBlobClient blobClient,
            IAdminProjectService adminProjectService,
            IMapper mapper) :
                base(blobClient, new ProjectForAdminCsvFormatter())
        {
            _adminProjectService = adminProjectService;
            _mapper = mapper;
        }

        protected override async Task<object> OnDataRequest(DataQueryOptions filter, CancellationTokenSource cancellationTokenSource)
        {
            // get data
            DataResult<Task<DomainProjectForAdmin>> data = await _adminProjectService.GetAsyncSequenceAsync(filter);

            // loading and shaping data
            IEnumerable<Task<AdminProject>> projectTasks = data.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainProjectForAdmin, AdminProject>(u.Result), cancellationTokenSource.Token));
            List<AdminProject> projects = (await Task.WhenAll(projectTasks)).ToList();

            if (projects.Count == 0)
            {
                cancellationTokenSource.Cancel();
            }

            return projects;
        }
    }
}