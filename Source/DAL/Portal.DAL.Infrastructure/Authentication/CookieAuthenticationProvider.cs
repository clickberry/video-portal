// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Portal.DAL.Infrastructure.Extensions;

namespace Portal.DAL.Infrastructure.Authentication
{
    /// <summary>
    ///     Manages cookie authentication.
    /// </summary>
    public sealed class CookieAuthenticationProvider
    {
        private readonly string _anonymousCookieName;
        private readonly string _authenticatedCookieName;
        private readonly string _rolesCookieName;
        private readonly HttpContext _context;
        private readonly string _cookieName;
        private const string Authorization = "Authorization";
        private const string AuthorizationBearer = "Bearer ";

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <param name="cookieName">Cookie name.</param>
        /// <param name="anonymousCookieName">Anonymous cookie name.</param>
        /// <param name="authenticatedCookieName">Authenticated cookie name.</param>
        /// <param name="rolesCookieName"></param>
        public CookieAuthenticationProvider(HttpContext context, string cookieName, string anonymousCookieName, string authenticatedCookieName, string rolesCookieName)
        {
            _context = context;
            _cookieName = cookieName;
            _anonymousCookieName = anonymousCookieName;
            _authenticatedCookieName = authenticatedCookieName;
            _rolesCookieName = rolesCookieName;
        }

        /// <summary>
        ///     Sets a response cookie.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="roles"></param>
        /// <param name="isPersistent">Determines whether cookie is persistent.</param>
        public string SetCookie(string userId, string[] roles, bool isPersistent = false)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            DateTime dateTime = DateTime.UtcNow;

            // Create ticket
            var authTicket = new FormsAuthenticationTicket(
                1,
                userId,
                dateTime,
                dateTime.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty);

            // Set cookie
            string ticket = FormsAuthentication.Encrypt(authTicket);

            // User marker cookie
            var userCookie = new HttpCookie(_cookieName, ticket)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath,
                Domain = FormsAuthentication.CookieDomain
            };

            // Authentication cookie
            var authenticationCookie = new HttpCookie(_authenticatedCookieName, GetHash(userId))
            {
                HttpOnly = false,
                Path = FormsAuthentication.FormsCookiePath,
                Domain = FormsAuthentication.CookieDomain
            };

            // Roles cookie
            var rolesCookie = new HttpCookie(_rolesCookieName, string.Join(",", roles))
            {
                HttpOnly = false,
                Path = FormsAuthentication.FormsCookiePath,
                Domain = FormsAuthentication.CookieDomain
            };

            if (isPersistent)
            {
                userCookie.Expires = authTicket.Expiration;
                authenticationCookie.Expires = authTicket.Expiration;
                rolesCookie.Expires = authTicket.Expiration;
            }

            _context.Response.AppendCookie(userCookie);
            _context.Response.AppendCookie(authenticationCookie);
            _context.Response.AppendCookie(rolesCookie);

            return _context.Response.Cookies.GetItem(_cookieName).Value;
        }

        /// <summary>
        ///     Clears cookie.
        /// </summary>
        public void Clear()
        {
            _context.Response.Cookies.Add(new HttpCookie(_cookieName) { Expires = DateTime.Now.AddDays(-1d) });
            _context.Response.Cookies.Add(new HttpCookie(_authenticatedCookieName) { Expires = DateTime.Now.AddDays(-1d) });
            _context.Response.Cookies.Add(new HttpCookie(_rolesCookieName) { Expires = DateTime.Now.AddDays(-1d) });
        }

        /// <summary>
        ///     Trying to parse request data.
        /// </summary>
        public string GetUserNameFromValue(string value)
        {
            // If the cookie can't be found, don't issue the ticket
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // Get the authentication ticket and rebuild the principal & identity
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(value);

            if (authTicket == null || authTicket.Expired)
            {
                return null;
            }

            return authTicket.Name;
        }

        /// <summary>
        ///     Trying to parse request data.
        /// </summary>
        public string GetUserNameFromCookie()
        {
            string value = null;

            // Try to get bearer token value
            var authHeader = _context.Request.Headers.Get(Authorization);
            if (string.IsNullOrEmpty(authHeader))
            {
                // Get the authentication cookie
                HttpCookie authCookie = _context.Request.Cookies.GetItem(_cookieName);

                // If the cookie can't be found, don't issue the ticket
                if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                {
                    value = authCookie.Value;
                }
            }
            else if (authHeader.StartsWith(AuthorizationBearer, StringComparison.OrdinalIgnoreCase))
            {
                value = authHeader.Substring(AuthorizationBearer.Length);
            }

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // Get the authentication ticket and rebuild the principal & identity
            FormsAuthenticationTicket authTicket;
            try
            {
                authTicket = FormsAuthentication.Decrypt(value);
            }
            catch (ArgumentException)
            {
                return null;
            }

            if (authTicket == null || authTicket.Expired)
            {
                return null;
            }

            return authTicket.Name;
        }

        /// <summary>
        ///     Checks for an anonymouse identifier.
        /// </summary>
        public void CheckAnonymousId()
        {
            // Ignore static resources
            if (_context.Request.Url.AbsolutePath.StartsWith("/cdn", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            HttpCookie authCookie = _context.Request.Cookies.GetItem(_anonymousCookieName);

            // If the cookie can't be found, don't issue the ticket
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                return;
            }

            // Set cookie
            var cookie = new HttpCookie(_anonymousCookieName, GetHash(Guid.NewGuid().ToString("N")))
            {
                Expires = new DateTime(2999, 12, 31, 23, 59, 59),
                HttpOnly = true
            };

            _context.Response.AppendCookie(cookie);
        }

        public string GetAnonymousId()
        {
            // Get the authentication cookie
            HttpCookie authCookie = _context.Request.Cookies.GetItem(_anonymousCookieName) ?? _context.Response.Cookies.GetItem(_anonymousCookieName);

            // If the cookie can't be found, don't issue the ticket
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                return null;
            }

            return authCookie.Value;
        }

        /// <summary>
        ///     Gets a Sha1 hash of data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns>Hash.</returns>
        private string GetHash(string data)
        {
            return BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(data)))
                .Replace("-", string.Empty);
        }
    }
}