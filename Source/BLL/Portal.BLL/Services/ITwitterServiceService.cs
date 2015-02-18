// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.DTO.Twitter;

namespace Portal.BLL.Services
{
    /// <summary>
    ///     Twitter social integration service.
    /// </summary>
    public interface ITwitterServiceService
    {
        /// <summary>
        ///     Sets an user status.
        /// </summary>
        /// <param name="status">Status entity.</param>
        Task SetStatusAsync(TwitterStatus status);

        /// <summary>
        ///     Gets an user profile information.
        /// </summary>
        /// <param name="request">Request message.</param>
        /// <returns>User profile.</returns>
        TwitterProfile GetProfile(TwitterRequest request);
    }
}