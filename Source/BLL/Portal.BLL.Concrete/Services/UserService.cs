// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Comment;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.Project;
using Portal.DAL.Subscriptions;
using Portal.DAL.User;
using Portal.Domain;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class UserService : IUserService
    {
        private const string DefaultTimezone = "UTC";
        private readonly ICommentRepository _commentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly CryptoService _cryptoService;
        private readonly long _defaultUserStorageSpace;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository,
            IProjectRepository projectRepository,
            IFileRepository fileRepository,
            ICompanyRepository companyRepository,
            ICommentRepository commentRepository,
            IMapper mapper,
            IProjectService projectService,
            IPortalFrontendSettings settings)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _fileRepository = fileRepository;
            _companyRepository = companyRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _projectService = projectService;
            _defaultUserStorageSpace = settings.DefaultMaxUserCapacity;
            _cryptoService = new CryptoService();
        }

        public async Task<List<DomainUser>> GetByIdsAsync(string[] ids)
        {
            var users = await _userRepository.GetByIdsAsync(ids);
            return users.Select(u => _mapper.Map<UserEntity, DomainUser>(u)).ToList();
        }

        // todo: should be renamed to GetByEmailAsync or does not throw NotFoundException
        public async Task<DomainUser> FindByEmailAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email.ToLowerInvariant());
            if (user == null)
            {
                throw new NotFoundException(string.Format("User with email '{0}' was not found.", email));
            }

            var storageSpaces = _fileRepository.Context.Where(p => p.UserId == user.Id).ToList();

            var profile = _mapper.Map<UserEntity, DomainUser>(user);
            profile.UsedStorageSpace = storageSpaces.Where(p => !p.IsArtifact).Sum(p => p.Length);

            return profile;
        }

        public async Task<DomainUser> AddAsync(DomainUser entity)
        {
            entity.MaximumStorageSpace = _defaultUserStorageSpace;
            entity.Created = DateTime.UtcNow;
            entity.Modified = DateTime.UtcNow;
            entity.Timezone = entity.Timezone ?? DefaultTimezone;

            // normalizing email
            if (!string.IsNullOrEmpty(entity.Email))
            {
                entity.Email = entity.Email.ToLowerInvariant();
            }

            // truncating name if the name is email
            if (!string.IsNullOrEmpty(entity.Name))
            {
                var index = entity.Name.IndexOf("@", StringComparison.Ordinal);
                if (index > 0)
                {
                    entity.Name = entity.Name.Substring(0, index);
                }
            }

            var user = _mapper.Map<DomainUser, UserEntity>(entity);
            user = await _userRepository.AddAsync(user);

            return _mapper.Map<UserEntity, DomainUser>(user);
        }

        public async Task<DomainUser> GetAsync(string id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new NotFoundException(string.Format("User '{0}' was not found.", id));
            }

            var storageSpaces = _fileRepository.Context.Where(p => p.UserId == user.Id).ToList();

            var profile = _mapper.Map<UserEntity, DomainUser>(user);
            profile.UsedStorageSpace = storageSpaces.Where(p => !p.IsArtifact).Sum(p => p.Length);

            return profile;
        }

        public async Task<DomainUser> UpdateAsync(string userId, UserUpdateOptions update)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("Could not find user '{0}'.", userId));
            }

            // Patching
            user.Modified = DateTime.UtcNow;
            user.Name = update.UserName;
            user.NameSort = update.UserName == null ? null : update.UserName.ToLowerInvariant();
            user.City = update.City;
            user.Country = update.Country;
            user.Timezone = update.Timezone;
            user.NotifyOnVideoComments = update.NotifyOnVideoComments;

            user = await _userRepository.UpdateAsync(user);

            // Calculate storage space
            var storageSpace = _fileRepository.Context.Where(p => p.UserId == userId).ToList();

            var profile = _mapper.Map<UserEntity, DomainUser>(user);
            profile.UsedStorageSpace = storageSpace.Where(p => !p.IsArtifact).Sum(p => p.Length);

            return profile;
        }

        public async Task DeleteMembersipAsync(string userId, ProviderType provider)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("Could not find user '{0}'.", userId));
            }

            await _userRepository.DeleteMembershipAsync(userId, provider.ToString());
        }

        public async Task DeleteAsync(string userId)
        {
            // Delete user
            await _userRepository.DeleteAsync(userId);

            // Delete projects
            var projects = await _projectService.GetListAsync(
                new DomainProject
                {
                    UserId = userId
                });

            await _projectService.DeleteAsync(projects);
        }

        public async Task MergeFromAsync(string userId, string toUserId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("Could not find user '{0}' to merge from.", userId));
            }

            var toUser = await _userRepository.GetAsync(toUserId);
            if (toUser == null)
            {
                throw new NotFoundException(string.Format("Could not find user '{0}' to merge to.", userId));
            }

            // Update related entities

            // Projects
            await _projectRepository.UpdateUserIdFromAsync(userId, toUserId);

            // Files
            await _fileRepository.UpdateUserIdFromAsync(userId, toUserId);

            // Companies
            await _companyRepository.UpdateUserIdFromAsync(userId, toUserId);

            // Comments
            await _commentRepository.UpdateUserIdFromAsync(userId, toUserId);


            // Set user properties
            if (string.IsNullOrEmpty(toUser.Email) && !string.IsNullOrEmpty(user.Email))
            {
                // set email if not specified
                toUser.Email = user.Email.ToLowerInvariant();
            }
            if (string.IsNullOrEmpty(toUser.Country))
            {
                toUser.Country = user.Country;
            }
            if (string.IsNullOrEmpty(toUser.City))
            {
                toUser.City = user.City;
            }
            if (string.IsNullOrEmpty(toUser.Timezone))
            {
                toUser.Timezone = user.Timezone;
            }

            // Merge memberships
            foreach (var userMembership in user.Memberships)
            {
                var membership = userMembership;
                if (toUser.Memberships.Any(m => m.IdentityProvider == membership.IdentityProvider && m.UserIdentifier == membership.UserIdentifier))
                {
                    continue;
                }

                // merging unexisting memberships
                toUser.Memberships.Add(userMembership);
            }

            // Merge roles
            foreach (var userRole in user.Roles)
            {
                var role = userRole;
                if (toUser.Roles.Any(r => r == role))
                {
                    continue;
                }

                // merging unexisting roles
                toUser.Roles.Add(role);
            }

            // Delete merged user account
            await DeleteAsync(user.Id);

            // Update current account
            await _userRepository.UpdateAsync(toUser);
        }

        public Task AddFollowerAsync(string userId, Follower follower)
        {
            return _userRepository.AddFollowerAsync(userId, _mapper.Map<Follower, FollowerEntity>(follower));
        }

        public async Task DeleteFollowerAsync(string userId, Follower follower)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("Could not find user '{0}'.", userId));
            }

            await _userRepository.DeleteFollowerAsync(userId, _mapper.Map<Follower, FollowerEntity>(follower));
        }

        public async Task<List<Follower>> FindFollowingUsersAsync(string userId)
        {
            var result = await _userRepository.FindFollowingUsersAsync(userId);
            return _mapper.Map<List<FollowerEntity>, List<Follower>>(result);
        }

        public async Task AddRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            await CheckRoleName(role);

            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("Could not find user #{0}.", userId));
            }

            // Check role uniqueness
            if (user.Roles.Contains(role))
            {
                throw new ConflictException(string.Format("Role {0} already exists for user {1}", role, userId));
            }

            // Add role
            user.Roles.Add(role);

            // Update user
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteRoleAsync(string userId, string role)
        {
            await CheckRoleName(role);

            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("Could not find user #{0}.", userId));
            }

            // Check role existence
            if (!user.Roles.Contains(role))
            {
                throw new NotFoundException(string.Format("User {0} does not have role {1}", userId, role));
            }

            // Delete role
            user.Roles.Remove(role);

            // Update user
            await _userRepository.UpdateAsync(user);
        }

        public async Task<DomainUser> FindByIdentityAsync(ProviderType provider, string userIdentifier)
        {
            var user = await _userRepository.FindByIdentityAsync(provider.ToString(), userIdentifier);
            if (user == null)
            {
                throw new NotFoundException(string.Format("User with identity '{0}:{1}' was not found.", provider, userIdentifier));
            }

            CheckUserState(user);

            return _mapper.Map<UserEntity, DomainUser>(user);
        }

        public async Task<DomainUser> CheckCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.FindByEmailAsync(email.ToLowerInvariant());
            if (user == null)
            {
                throw new NotFoundException(string.Format("User with email '{0}' was not found.", email));
            }

            CheckUserState(user);

            var encodedPassword = _cryptoService.EncodePassword(password, user.PasswordSalt);
            if (user.Password != encodedPassword)
            {
                throw new BadRequestException("Invalid password.");
            }

            return _mapper.Map<UserEntity, DomainUser>(user);
        }

        public async Task ChangeEmailAsync(string userId, string email)
        {
            email = email.ToLowerInvariant();
            await _userRepository.ChangeEmailAsync(userId, email);
        }

        public Task AddMembersipAsync(string userId, TokenData tokenData)
        {
            return _userRepository.AddMembershipAsync(userId, _mapper.Map<TokenData, UserMembershipEntity>(tokenData));
        }

        /// <summary>
        ///     Checks whether user state is valid.
        /// </summary>
        /// <param name="user">User.</param>
        private void CheckUserState(UserEntity user)
        {
            var state = (ResourceState)user.State;
            switch (state)
            {
                case ResourceState.Blocked:
                    throw new ForbiddenException();

                case ResourceState.Deleted:
                    throw new NotFoundException();
            }
        }

        private async Task CheckRoleName(string role)
        {
            // Check whether role name is valid
            if (!DomainRoles.AllRoles.Contains(role))
            {
                throw new BadRequestException();
            }

            // Check whether SuperAdministrator already exists
            if (role == DomainRoles.SuperAdministrator &&
                await _userRepository.GetUsersInRoleAsync(role) != null)
            {
                throw new ForbiddenException(string.Format("{0} role already exists!", role));
            }
        }
    }
}