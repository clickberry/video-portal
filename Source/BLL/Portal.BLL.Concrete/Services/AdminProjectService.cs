// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Common.Helpers;
using Portal.DAL.Context;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.User;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Concrete.Services
{
    public sealed class AdminProjectService : IAdminProjectService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProductWriterForAdmin _productWriterForAdmin;
        private readonly ITableRepository<ProjectEntity> _projectRepository;
        private readonly IUserRepository _userRepository;

        public AdminProjectService(IRepositoryFactory repositoryFactory, IUserRepository userRepository, IProductWriterForAdmin productWriterForAdmin, IFileSystem fileSystem)
        {
            _projectRepository = repositoryFactory.Create<ProjectEntity>();
            _userRepository = userRepository;
            _productWriterForAdmin = productWriterForAdmin;
            _fileSystem = fileSystem;
        }

        public async Task<DataResult<Task<DomainProjectForAdmin>>> GetAsyncSequenceAsync(DataQueryOptions filter)
        {
            var query = new List<IMongoQuery>(filter.Filters.Count);

            // filtering
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.Name),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by name
                    query.Add(Query.Text(f.Value.ToString().ToLowerInvariant()));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.ProductType),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by product type
                    var productId = Int32.Parse(f.Value.ToString());
                    query.Add(Query<ProjectEntity>.EQ(p => p.ProductId, productId));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.UserId),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by user id
                    query.Add(Query<ProjectEntity>.EQ(p => p.UserId, f.Value.ToString()));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.UserName),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by user name
                    List<UserEntity> profiles = await _userRepository.FindByNameAsync(f.Value.ToString());
                    if (profiles.Count == 0)
                    {
                        // no users found
                        return new DataResult<Task<DomainProjectForAdmin>>(new Task<DomainProjectForAdmin>[] { });
                    }

                    List<string> allIds = profiles.Select(prof => prof.Id).ToList();
                    query.Add(Query<ProjectEntity>.Where(p => allIds.Contains(p.UserId)));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.Created),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // by created date
                    var date = (DateTime)f.Value;
                    switch (f.Type)
                    {
                        case DataFilterTypes.Equal:
                            query.Add(Query<ProjectEntity>.EQ(p => p.Created, date));
                            break;
                        case DataFilterTypes.LessThan:
                            query.Add(Query<ProjectEntity>.LT(p => p.Created, date));
                            break;
                        case DataFilterTypes.LessThanOrEqual:
                            query.Add(Query<ProjectEntity>.LTE(p => p.Created, date));
                            break;
                        case DataFilterTypes.GreaterThan:
                            query.Add(Query<ProjectEntity>.GT(p => p.Created, date));
                            break;
                        case DataFilterTypes.GreaterThanOrEqual:
                            query.Add(Query<ProjectEntity>.GTE(p => p.Created, date));
                            break;
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            if (!filters.Any() && !string.IsNullOrEmpty(filter.OrderBy))
            {
                // MongoDb 2.6 HACK!!!
                // adding fake query to hint proper index

                if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.Name),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // order by name
                    query.Add(Query<ProjectEntity>.Exists(p => p.Name));
                }
                else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.Created),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // order by created
                    query.Add(Query<ProjectEntity>.Exists(p => p.Created));
                }
                else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.ProductType),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // order by product type
                    query.Add(Query<ProjectEntity>.Exists(p => p.ProductId));
                }
            }

            MongoCursor<ProjectEntity> cursor = _projectRepository.Collection.Find(query.Count > 0 ? Query.And(query) : Query.Null);
            IMongoSortBy sortOrder = null;

            // sorting
            if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.Name),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by name
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<ProjectEntity>.Ascending(p => p.Name)
                    : SortBy<ProjectEntity>.Descending(p => p.Name);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.Created),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by created
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<ProjectEntity>.Ascending(p => p.Created)
                    : SortBy<ProjectEntity>.Descending(p => p.Created);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainProjectForAdmin>(x => x.ProductType),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by product type
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<ProjectEntity>.Ascending(p => p.ProductId)
                    : SortBy<ProjectEntity>.Descending(p => p.ProductId);
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

            return new DataResult<Task<DomainProjectForAdmin>>(cursor.Select(GetProjectDataAsync), count);
        }


        public async Task DeleteAsync(DomainProjectForAdmin project)
        {
            ProjectEntity entity = await _projectRepository.SingleAsync(p => p.Id == project.ProjectId);

            var tasks = new List<Task>();

            // removing avsx
            if (!string.IsNullOrEmpty(entity.AvsxFileId))
            {
                tasks.Add(_fileSystem.DeleteFileAsync(new StorageFile { Id = entity.AvsxFileId, UserId = entity.UserId }));
            }

            // removing video
            if (!string.IsNullOrEmpty(entity.OriginalVideoFileId))
            {
                tasks.Add(_fileSystem.DeleteFileAsync(new StorageFile { Id = entity.OriginalVideoFileId, UserId = entity.UserId }));
            }

            // removing screenshot
            if (!string.IsNullOrEmpty(entity.ScreenshotFileId))
            {
                tasks.Add(_fileSystem.DeleteFileAsync(new StorageFile { Id = entity.ScreenshotFileId, UserId = entity.UserId }));
            }

            // removing encoded screenshots
            tasks.AddRange(entity.EncodedScreenshots.Select(s => _fileSystem.DeleteFileAsync(new StorageFile { Id = s.FileId, UserId = entity.UserId })));

            // removing encoded videos
            tasks.AddRange(entity.EncodedVideos.Select(s => _fileSystem.DeleteFileAsync(new StorageFile { Id = s.FileId, UserId = entity.UserId })));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                // could not delete all related files
                Trace.TraceError("Could not delete all related files for project {0}: {1}", entity.Id, e);
            }

            // removing project with all embedded data: avsx, video, screenshot
            await _projectRepository.DeleteAsync(entity);
        }


        private async Task<DomainProjectForAdmin> GetProjectDataAsync(ProjectEntity project)
        {
            UserEntity user = await _userRepository.GetAsync(project.UserId);

            return new DomainProjectForAdmin
            {
                ProjectId = project.Id,
                Name = project.Name,
                Description = project.Description,
                Created = project.Created,
                Modified = project.Modified,
                UserId = project.UserId,
                UserName = user != null ? user.Name : null,
                ProductType = (ProductType)project.ProductId,
                Product = _productWriterForAdmin.WriteProduct(project.ProductId)
            };
        }
    }
}