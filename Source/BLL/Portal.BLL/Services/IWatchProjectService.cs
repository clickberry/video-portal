// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.DTO.Watch;

namespace Portal.BLL.Services
{
    public interface IWatchProjectService
    {
        Task<Watch> GetByIdAsync(string id, string userId);

        Task CheckProjectAsync(string id, string userId);

        Task<List<Watch>> GetByIdsAsync(string[] ids, string userId);

        Task<DataResult<Watch>> GetSequenceAsync(DataQueryOptions filter, string userId);
    }
}