// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Portal.DAL.Authentication;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Infrastructure.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private const string UserEmailClaim = "userEmail";
        private const string UserNameClaim = "userName";
        private const string IdentityProviderClaim = "identityProvider";
        private const string FederationAuthenticationType = "Federation";

        private readonly CookieAuthenticationProvider _authenticationProvider;

        public Authenticator()
        {
            _authenticationProvider = new CookieAuthenticationProvider(
                HttpContext.Current,
                CookieNames.CookieName,
                CookieNames.AnonymousCookieName,
                CookieNames.AuthenticationCookieName,
                CookieNames.RolesCookieName);
        }

        public string Authenticate(UserEntity user, string identityProvider, bool isPersistent = false)
        {
            // Set thread identity
            SetPrincipal(user, identityProvider);

            return _authenticationProvider.SetCookie(user.Id, user.Roles.ToArray(), isPersistent);
        }

        public void SetPrincipal(UserEntity user, string identityProvider)
        {
            IEnumerable<Claim> newClaims = CreateClaims(user.Email, user.Name, identityProvider);
            var userIdentity = new SocialIdentity(user.Id) { Memberships = user.Memberships };
            userIdentity.AddClaims(newClaims);

            // check if principal is already has claims and preserve them
            var claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            if (claimsPrincipal != null && claimsPrincipal.Identity.AuthenticationType == FederationAuthenticationType)
            {
                userIdentity.SocialClaims = new List<Claim>(claimsPrincipal.Claims);
            }

            var userPrincipal = new GenericPrincipal(userIdentity, user.Roles.ToArray());

            HttpContext.Current.User = userPrincipal;
        }

        public void UpdateIdentityClaims(string userId, string userEmail, string userName, string identityProvider)
        {
            var userIdentity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            List<Claim> claims = userIdentity.Claims.Where(
                p => p.Type == UserEmailClaim ||
                     p.Type == UserNameClaim ||
                     p.Type == IdentityProviderClaim).ToList();

            foreach (Claim claim in claims)
            {
                userIdentity.RemoveClaim(claim);
            }

            IEnumerable<Claim> newClaims = CreateClaims(userEmail, userName, identityProvider);
            userIdentity.AddClaims(newClaims);
        }

        public void Clear()
        {
            _authenticationProvider.Clear();
        }

        public string GetUserNameFromValue(string value)
        {
            return _authenticationProvider.GetUserNameFromValue(value);
        }

        public string GetUserNameFromCookie()
        {
            return _authenticationProvider.GetUserNameFromCookie();
        }

        public void CheckAnonymousId()
        {
            _authenticationProvider.CheckAnonymousId();
        }

        public string GetAnonymousId()
        {
            return _authenticationProvider.GetAnonymousId();
        }

        public string GetUserId()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public string GetUserEmail()
        {
            return GetClaim(UserEmailClaim);
        }

        public string GetUserName()
        {
            return GetClaim(UserNameClaim);
        }

        public string GetIdentityProvider()
        {
            return GetClaim(IdentityProviderClaim);
        }

        private string GetClaim(string claimType)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            Claim claim = identity.Claims.FirstOrDefault(p => p.Type == claimType);
            return claim == null ? null : claim.Value;
        }

        private IEnumerable<Claim> CreateClaims(string userEmail, string userName, string identityProvider)
        {
            var claims = new List<Claim>();

            if (!String.IsNullOrEmpty(userEmail))
            {
                claims.Add(new Claim(UserEmailClaim, userEmail));
            }
            if (!String.IsNullOrEmpty(userName))
            {
                claims.Add(new Claim(UserNameClaim, userName));
            }
            if (!String.IsNullOrEmpty(identityProvider))
            {
                claims.Add(new Claim(IdentityProviderClaim, identityProvider));
            }

            return claims;
        }
    }
}