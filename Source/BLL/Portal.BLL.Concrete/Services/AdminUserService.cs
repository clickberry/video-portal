// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Common.Helpers;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.Project;
using Portal.DAL.User;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class AdminUserService : IAdminUserService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IProductWriterForAdmin _productWriterForAdmin;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;
        private readonly ITableRepository<FileEntity> _fileRepository;
        private readonly IUserRepository _userRepository;

        public AdminUserService(
            IRepositoryFactory repositoryFactory,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IPasswordService passwordService,
            IProjectService projectService,
            IProductWriterForAdmin productWriterForAdmin,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _projectService = projectService;
            _productWriterForAdmin = productWriterForAdmin;
            _mapper = mapper;
            _fileRepository = repositoryFactory.Create<FileEntity>();
            _projectRepository = projectRepository;
        }


        public DataResult<Task<DomainUserForAdmin>> GetAsyncSequence(DataQueryOptions filter)
        {
            var query = new List<IMongoQuery>(filter.Filters.Count);

            // filtering
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.UserName),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by user name
                    query.Add(Query.Text(f.Value.ToString().ToLowerInvariant()));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.Email),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by email
                    string email = f.Value.ToString().ToLowerInvariant();
                    var expression = new BsonRegularExpression(string.Format("^{0}.*", Regex.Escape(email)));
                    query.Add(Query<UserEntity>.Matches(p => p.Email, expression));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.ProductType),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by product type
                    var productId = Int32.Parse(f.Value.ToString());
                    query.Add(Query<UserEntity>.EQ(p => p.ProductId, productId));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.Created),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // by created date
                    var date = (DateTime)f.Value;
                    switch (f.Type)
                    {
                        case DataFilterTypes.Equal:
                            query.Add(Query<UserEntity>.EQ(p => p.Created, date));
                            break;
                        case DataFilterTypes.LessThan:
                            query.Add(Query<UserEntity>.LT(p => p.Created, date));
                            break;
                        case DataFilterTypes.LessThanOrEqual:
                            query.Add(Query<UserEntity>.LTE(p => p.Created, date));
                            break;
                        case DataFilterTypes.GreaterThan:
                            query.Add(Query<UserEntity>.GT(p => p.Created, date));
                            break;
                        case DataFilterTypes.GreaterThanOrEqual:
                            query.Add(Query<UserEntity>.GTE(p => p.Created, date));
                            break;
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            // Filter only users
            query.Add(Query<UserEntity>.EQ(p => p.Roles, DomainRoles.User));

            MongoCursor<UserEntity> cursor = _userRepository.Collection.Find(query.Count > 0 ? Query.And(query) : Query.Null);
            IMongoSortBy sortOrder = null;

            // sorting
            if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.UserName),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by name
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<UserEntity>.Ascending(p => p.Name)
                    : SortBy<UserEntity>.Descending(p => p.Name);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.Created),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by created
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<UserEntity>.Ascending(p => p.Created)
                    : SortBy<UserEntity>.Descending(p => p.Created);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.ProductType),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by product type
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<UserEntity>.Ascending(p => p.ProductId)
                    : SortBy<UserEntity>.Descending(p => p.ProductId);
            }

            if (sortOrder != null)
            {
                cursor.SetSortOrder(sortOrder);
            }

            // paging

            if (filter.Skip.HasValue)
            {
                cursor.SetSkip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                cursor.SetLimit(filter.Take.Value);
            }

            // Count of results
            long? count = null;
            if (filter.Count)
            {
                count = cursor.Count();
            }

            // post-processing

            return new DataResult<Task<DomainUserForAdmin>>(cursor.Select(GetUserDataAsync), count);
        }

        public async Task DeleteAsync(string userId)
        {
            UserEntity user = await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new NotFoundException();
            }

            if (user.Roles.Contains(DomainRoles.SuperAdministrator))
            {
                throw new ForbiddenException();
            }

            List<DomainProject> projects = await _projectService.GetListAsync(
                new DomainProject
                {
                    UserId = userId
                });

            await _projectService.DeleteAsync(projects);
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<DomainUserForAdmin> GetAsync(DomainUserForAdmin user)
        {
            UserEntity profile = await _userRepository.GetAsync(user.UserId);
            if (profile == null)
            {
                throw new NotFoundException();
            }
            return await GetUserDataAsync(profile);
        }

        public Task SetUserPasswordAsync(string userId, string password)
        {
            return _passwordService.ChangePasswordAsync(userId, password);
        }


        public async Task<List<DomainUser>> GetUsersInRoleAsync(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            List<UserEntity> users = await _userRepository.GetUsersInRoleAsync(role);

            return users.Select(p => _mapper.Map<UserEntity, DomainUser>(p)).ToList();
        }


        public async Task<List<DomainUser>> FindByNameAsync(string userName)
        {
            List<UserEntity> users = await _userRepository.FindByNameAsync(userName);
            return users.Select(_mapper.Map<UserEntity, DomainUser>).ToList();
        }

        private async Task<DomainUserForAdmin> GetUserDataAsync(UserEntity user)
        {
            Task<List<FileEntity>> storageSpacesTask = _fileRepository.ToListAsync(p => p.UserId == user.Id && !p.IsArtifact);
            Task<List<ProjectEntity>> projectsTask = _projectRepository.GetUserProjectsAsync(user.Id);

            await Task.WhenAll(new Task[]
            {
                storageSpacesTask,
                projectsTask
            });

            return new DomainUserForAdmin
            {
                AppName = user.AppName,
                Created = user.Created,
                MaximumStorageSpace = user.MaximumStorageSpace,
                UserId = user.Id,
                UserName = user.Name ?? user.Id,
                UsedStorageSpace = storageSpacesTask.Result.Sum(p => p.Length),
                VideosCount = projectsTask.Result.Count,
                Memberships = user.Memberships.Select(
                    p => new DomainUserMembershipForAdmin
                    {
                        Identity = p.UserIdentifier,
                        Provider = p.IdentityProvider
                    }).ToList(),
                ProductType = (ProductType)user.ProductId,
                ProductName = _productWriterForAdmin.WriteProduct(user.ProductId),
                Email = user.Email
            };
        }
    }
}