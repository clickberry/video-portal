// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.DTO.User;

namespace Portal.BLL.Services
{
    /// <summary>
    ///     Abuse statistics for Watch objects on top of the Cassandra Statistics.
    /// </summary>
    public interface IProjectAbuseService
    {
        Task<bool> IsReportedAsync(string projectId, string userId);

        Task<IEnumerable<UserInfo>> GetProjectReportersSequenceAsync(string projectId, DataQueryOptions filter);

        Task<long> GetProjectReportsCountAsync(string projectId);
    }
}