// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Entities.Table;
using Portal.DAL.User;
using Portal.Domain.PortalContext;
using Portal.Exceptions.CRUD;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Configures default roles and administrator.
    /// </summary>
    public sealed class RolesInitializer : IInitializable
    {
        private const char FieldDelimiter = ';';
        private readonly IPasswordService _passwordService;
        private readonly IPortalMiddleendSettings _settings;
        private readonly IUserRepository _userRepository;

        public RolesInitializer(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IPortalMiddleendSettings settings)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _settings = settings;
        }

        public void Initialize()
        {
            InitializeSuperAdmin().Wait();
        }

        private async Task InitializeSuperAdmin()
        {
            // Get admin configuration
            string dataString = _settings.DefaultAdministrator;
            if (string.IsNullOrEmpty(dataString))
            {
                return;
            }

            // Get default administrator data
            List<string> userData = dataString.Split(FieldDelimiter).ToList();

            if (userData.Count != 2)
            {
                Trace.TraceError("Failed to parse default administrator data");
                return;
            }

            // Check user existence
            UserEntity user = null;

            try
            {
                user = await _userRepository.FindByEmailAsync(userData[0]);
            }
            catch (NotFoundException)
            {
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to get administrator: {0}", e);
                return;
            }

            if (user == null)
            {
                // Try to create a new super administrator
                try
                {
                    // Add user
                    user = await _userRepository.AddAsync(new UserEntity
                    {
                        AppName = new Uri(_settings.PortalUri).Host,
                        Name = DomainRoles.SuperAdministrator,
                        NameSort = DomainRoles.SuperAdministrator.ToLowerInvariant(),
                        Email = userData[0],
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        Roles = new List<string>
                        {
                            DomainRoles.SuperAdministrator
                        }
                    });

                    // Set adminstrator password
                    await _passwordService.ChangePasswordAsync(user.Id, userData[1]);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to add administrator profile: {0}", e);
                }
            }
        }
    }
}