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
    public class UsersForAdminCsvPublishService : CsvPublishServiceBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IMapper _mapper;

        public UsersForAdminCsvPublishService(CloudBlobClient blobClient,
            IAdminUserService adminUserService,
            IMapper mapper) : base(blobClient, new UserForAdminCsvFormatter())
        {
            _adminUserService = adminUserService;
            _mapper = mapper;
        }

        protected override async Task<object> OnDataRequest(DataQueryOptions filter, CancellationTokenSource cancellationTokenSource)
        {
            // get data
            DataResult<Task<DomainUserForAdmin>> data = _adminUserService.GetAsyncSequence(filter);

            // loading and shaping data
            IEnumerable<Task<AdminUser>> userTasks = data.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainUserForAdmin, AdminUser>(u.Result), cancellationTokenSource.Token));
            List<AdminUser> users = (await Task.WhenAll(userTasks)).ToList();

            if (users.Count == 0)
            {
                cancellationTokenSource.Cancel();
            }

            return users;
        }
    }
}