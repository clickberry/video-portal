// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;
using Portal.BLL.Services;
using Portal.DTO.Admin;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public class CsvPublishServiceFactory : ICsvPublishServiceFactory
    {
        private readonly Dictionary<Type, ICsvPublishService> _services;

        public CsvPublishServiceFactory(CloudBlobClient blobClient,
            IMapper mapper,
            IAdminUserService adminUserService,
            IAdminProjectService adminProjectService,
            IAdminClientService adminClientService)
        {
            _services = new Dictionary<Type, ICsvPublishService>
            {
                { typeof (AdminUser), new UsersForAdminCsvPublishService(blobClient, adminUserService, mapper) },
                { typeof (AdminProject), new ProjectsForAdminCsvPublishService(blobClient, adminProjectService, mapper) },
                { typeof (AdminClient), new ClientsForAdminCsvPublishService(blobClient, adminClientService, mapper) }
            };
        }

        public ICsvPublishService GetService<T>()
        {
            return !_services.ContainsKey(typeof (T)) ? null : _services[typeof (T)];
        }
    }
}