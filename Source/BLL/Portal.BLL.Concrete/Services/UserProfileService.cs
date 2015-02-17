using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Portal.BLL.Abstract;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class UserProfileService : IService<DomainProfile>
    {
        private const string DefaultTimezone = "UTC";

        private readonly long _defaultUserStorageSpace;
        private readonly IMappingEngine _mappingEngine;
        private readonly IService<DomainEmailMembership> _membershipService;
        private readonly IRepository<UserProfileEntity> _profileRepository;
        private readonly IService<DomainProject> _projectService;
        private readonly IRepository<StorageSpaceEntity> _storageSpaceRepository;
        private readonly IRepository<UserRoleEntity> _userRolesRepository;
        private readonly IRepository<AuthenticationEntity> _authenticationRepository;

        public UserProfileService(IRepositoryFactory repositoryFactory,
                                  IMappingEngine mappingEngine,
                                  IService<DomainEmailMembership> membershipService,
                                  IService<DomainProject> projectService,
                                  IConfigurationProvider configurationProvider)
        {
            _mappingEngine = mappingEngine;
            _membershipService = membershipService;
            _projectService = projectService;
            _profileRepository = repositoryFactory.Create<UserProfileEntity>(Tables.UserProfile);
            _storageSpaceRepository = repositoryFactory.Create<StorageSpaceEntity>(Tables.StorageSpace);
            _userRolesRepository = repositoryFactory.Create<UserRoleEntity>(Tables.UserRole);
            _authenticationRepository = repositoryFactory.Create<AuthenticationEntity>(Tables.Authentication);
            _defaultUserStorageSpace = long.Parse(configurationProvider.Get(FrontEndSettings.DefaultMaxUserCapacity));
        }

        public async Task<DomainProfile> AddAsync(DomainProfile entity)
        {
            entity.UserId = Guid.NewGuid().ToString();
            entity.MaximumStorageSpace = _defaultUserStorageSpace;
            entity.Created = DateTime.UtcNow;
            entity.Modified = DateTime.UtcNow;
            entity.Blocked = DateTime.UtcNow;
            entity.Timezone = entity.Timezone ?? DefaultTimezone;

            UserProfileEntity userProfile = _mappingEngine.Map<DomainProfile, UserProfileEntity>(entity);
            userProfile = await _profileRepository.AddAsync(userProfile);

            return _mappingEngine.Map<UserProfileEntity, DomainProfile>(userProfile);
        }

        public Task<List<DomainProfile>> AddAsync(IList<DomainProfile> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<DomainProfile> GetAsync(DomainProfile entity)
        {
            UserProfileEntity userProfile = await _profileRepository.SingleAsync(p => p.AppName == entity.ApplicationName && p.UserId == entity.UserId);
            List<StorageSpaceEntity> storageSpaces = await _storageSpaceRepository.ToListAsync(p => p.UserId == userProfile.UserId);

            DomainProfile profile = _mappingEngine.Map<UserProfileEntity, DomainProfile>(userProfile);
            profile.UsedStorageSpace = storageSpaces.Where(p => !p.IsArtifact).Sum(p => p.FileLength);

            return profile;
        }

        public async Task<List<DomainProfile>> GetListAsync(DomainProfile entity)
        {
            List<UserProfileEntity> userProfiles = await _profileRepository.ToListAsync();
            List<DomainProfile> profiles = userProfiles.Select(p => _mappingEngine.Map<UserProfileEntity, DomainProfile>(p)).ToList();

            foreach (DomainProfile profile in profiles)
            {
                DomainProfile currentProfile = profile;
                List<StorageSpaceEntity> storageSpaces = await _storageSpaceRepository.ToListAsync(p => p.UserId == currentProfile.UserId);
                profile.UsedStorageSpace = storageSpaces.Where(p => !p.IsArtifact).Sum(q => q.FileLength);
            }

            return profiles;
        }

        public async Task<DomainProfile> EditAsync(DomainProfile entity)
        {
            UserProfileEntity userProfile = await _profileRepository.SingleAsync(p => p.UserId == entity.UserId);

            if (userProfile == null)
            {
                throw new ApplicationException("Unexpected exception occured!");
            }

            userProfile.UserName = entity.UserName;
            userProfile.City = entity.City;
            userProfile.Country = entity.Country;
            userProfile.Timezone = entity.Timezone;
            userProfile.Modified = DateTime.UtcNow;

            userProfile = await _profileRepository.UpdateAsync(userProfile);
            List<StorageSpaceEntity> storageSpace = await _storageSpaceRepository.ToListAsync(p => p.UserId == entity.UserId);

            DomainProfile profile = _mappingEngine.Map<UserProfileEntity, DomainProfile>(userProfile);
            profile.UsedStorageSpace = storageSpace.Where(p => !p.IsArtifact).Sum(p => p.FileLength);

            return profile;
        }

        public async Task DeleteAsync(DomainProfile entity)
        {
            // Delete profile
            var userProfile = _mappingEngine.Map<DomainProfile, UserProfileEntity>(entity);
            await _profileRepository.DeleteAsync(userProfile);

            // Delete memberships
            List<DomainEmailMembership> memberships = await _membershipService.GetListAsync(
                new DomainEmailMembership
                    {
                        ApplicationName = entity.ApplicationName,
                        UserId = entity.UserId
                    });

            await _membershipService.DeleteAsync(memberships);

            // Delete projects
            List<DomainProject> projects = await _projectService.GetListAsync(
                new DomainProject
                    {
                        UserId = entity.UserId
                    });

            await _projectService.DeleteAsync(projects);

            // Delete roles
            List<UserRoleEntity> roles = await _userRolesRepository.ToListAsync(p => p.UserId == entity.UserId);
            await _userRolesRepository.DeleteAsync(roles);

            // Delete authentication
            var authentication = await _authenticationRepository.ToListAsync(p => p.UserId == entity.UserId);
            await _authenticationRepository.DeleteAsync(authentication);
        }

        public Task DeleteAsync(IList<DomainProfile> entity)
        {
            throw new NotImplementedException();
        }
    }
}