// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.DAL.Entities.Table;

namespace Portal.DAL.Authentication
{
    public interface IAuthenticator
    {
        /// <summary>
        ///     Sets identity and response cookie.
        /// </summary>
        /// <param name="user">User entity.</param>
        /// <param name="identityProvider">Identity Provider.</param>
        /// <param name="isPersistent">Determines whether cookie is persistent.</param>
        /// <returns>Authorization token.</returns>
        string Authenticate(UserEntity user, string identityProvider, bool isPersistent = false);

        /// <summary>
        ///     Sets a principal.
        /// </summary>
        /// <param name="user">User entity.</param>
        /// <param name="identityProvider">Identity Provider.</param>
        void SetPrincipal(UserEntity user, string identityProvider);

        /// <summary>
        ///     Update identity claims.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="userEmail">Email address.</param>
        /// <param name="userName">User name.</param>
        /// <param name="identityProvider"></param>
        void UpdateIdentityClaims(string userId, string userEmail, string userName, string identityProvider);

        /// <summary>
        ///     Clears cookie.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Trying to parse request data.
        /// </summary>
        string GetUserNameFromValue(string value);

        /// <summary>
        ///     Trying to parse request data.
        /// </summary>
        string GetUserNameFromCookie();

        /// <summary>
        ///     Checks for an anonymouse identifier.
        /// </summary>
        void CheckAnonymousId();

        /// <summary>
        ///     Get anonymous Id.
        /// </summary>
        string GetAnonymousId();

        /// <summary>
        ///     Get user Id.
        /// </summary>
        string GetUserId();

        /// <summary>
        ///     Return true if authenticated.
        /// </summary>
        bool IsAuthenticated();

        /// <summary>
        ///     Return user email address.
        /// </summary>
        /// <returns></returns>
        string GetUserEmail();

        /// <summary>
        ///     Return user name.
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        ///     Return identity provider.
        /// </summary>
        /// <returns></returns>
        string GetIdentityProvider();
    }
}