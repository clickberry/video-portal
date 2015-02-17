// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.DTO.User;
using Portal.DTO.Watch;

namespace Portal.BLL.Services
{
    /// <summary>
    ///     Likes/dislikes statistics for Watch objects on top of the Cassandra Statistics.
    /// </summary>
    public interface IProjectLikesService
    {
        // LIKES

        Task<IEnumerable<Watch>> GetUserLikesSequenceAsync(string userId, DataQueryOptions filter);

        Task<bool> IsLikedAsync(string projectId, string userId);

        Task<IEnumerable<UserInfo>> GetProjectLikersSequenceAsync(string projectId, DataQueryOptions filter);

        Task<long> GetProjectLikesCountAsync(string projectId);

        // DISLIKES

        Task<IEnumerable<Watch>> GetUserDislikesSequenceAsync(string userId, DataQueryOptions filter);

        Task<bool> IsDislikedAsync(string projectId, string userId);

        Task<IEnumerable<UserInfo>> GetProjectDislikersSequenceAsync(string projectId, DataQueryOptions filter);

        Task<long> GetProjectDislikesCountAsync(string projectId);
    }
}