// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain;
using Portal.Domain.Admin;

namespace Portal.BLL.Services
{
    public interface IAdminClientService
    {
        DataResult<Task<DomainClientForAdmin>> GetAsyncSequence(DataQueryOptions filter);

        Task<DomainClientForAdmin> GetAsync(string clientId);

        Task<DomainClientForAdmin> SetStateAsync(string id, ResourceState state);
    }
}