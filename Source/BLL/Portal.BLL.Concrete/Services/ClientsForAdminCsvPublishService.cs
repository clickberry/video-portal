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
    public class ClientsForAdminCsvPublishService : CsvPublishServiceBase
    {
        private readonly IAdminClientService _adminClientService;
        private readonly IMapper _mapper;

        public ClientsForAdminCsvPublishService(CloudBlobClient blobClient,
            IAdminClientService adminClientService,
            IMapper mapper) : base(blobClient, new ClientForAdminCsvFormatter())
        {
            _adminClientService = adminClientService;
            _mapper = mapper;
        }

        protected override async Task<object> OnDataRequest(DataQueryOptions filter, CancellationTokenSource cancellationTokenSource)
        {
            // get data
            DataResult<Task<DomainClientForAdmin>> data = _adminClientService.GetAsyncSequence(filter);

            // loading and shaping data
            IEnumerable<Task<AdminClient>> clientTasks = data.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainClientForAdmin, AdminClient>(u.Result), cancellationTokenSource.Token));
            List<AdminClient> clients = (await Task.WhenAll(clientTasks)).ToList();

            if (clients.Count == 0)
            {
                cancellationTokenSource.Cancel();
            }

            return clients;
        }
    }
}