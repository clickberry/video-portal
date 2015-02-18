// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Configuration.Azure.Concrete;
using MongoDB.Driver;
using Portal.DAL.Azure.User;
using Portal.DAL.Entities.Table;
using Portal.DAL.Infrastructure.Authentication;
using Portal.Domain;
using Portal.Exceptions.CRUD;

namespace Asp.Infrastructure.Modules.Authentication
{
    public sealed class AuthenticationModule : IHttpModule
    {
        private UserRepository _userRepository;

        public String ModuleName
        {
            get { return "AuthenticationModule"; }
        }

        public void Init(HttpApplication context)
        {
            var settings = new PortalSettings(new ConfigurationProvider());
            var url = new MongoUrl(settings.MongoConnectionString);
            _userRepository = new UserRepository(url);

            var asyncHelper = new EventHandlerTaskAsyncHelper(AuthenticateRequestAsync);
            context.AddOnAuthenticateRequestAsync(asyncHelper.BeginEventHandler, asyncHelper.EndEventHandler);
        }

        public void Dispose()
        {
        }

        private async Task AuthenticateRequestAsync(object sender, EventArgs eventArgs)
        {
            // Parse cookies
            var autheticationProvider = new Authenticator();

            autheticationProvider.CheckAnonymousId();

            string userId = autheticationProvider.GetUserNameFromCookie();
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // Get user data

            UserEntity user;
            try
            {
                user = await _userRepository.GetAsync(userId);
            }
            catch (NotFoundException e)
            {
                Trace.TraceError("User {0} was not found: {1}", userId, e);
                autheticationProvider.Clear();
                return;
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive user data: {0}", e);
                return;
            }

            // Check user state
            if (user == null || user.State != (int)ResourceState.Available)
            {
                // user does not exist or unavailable
                autheticationProvider.Clear();
                return;
            }

            autheticationProvider.SetPrincipal(user, null);
        }
    }
}