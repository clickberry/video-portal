// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Common.Helpers;
using Portal.DAL.Comment;
using Portal.DAL.Entities.Table;
using Portal.DAL.Project;
using Portal.DAL.User;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;
using Portal.DTO.Watch;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class WatchProjectService : IWatchProjectService
    {
        private const int RecentCommentsLimit = 3;

        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUriProvider _projectUriProvider;
        private readonly IPortalFrontendSettings _settings;
        private readonly IFileUriProvider _uriProvider;
        private readonly IUserAvatarProvider _userAvatarProvider;
        private readonly IUserRepository _userRepository;
        private readonly int[] _projectAccess = { (int)ProjectAccess.Public, (int)ProjectAccess.Hidden };

        public WatchProjectService(IProjectUriProvider projectUriProvider,
            ICommentRepository commentRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IPortalFrontendSettings settings,
            IFileUriProvider uriProvider,
            IUserAvatarProvider userAvatarProvider)
        {
            _projectUriProvider = projectUriProvider;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _settings = settings;
            _uriProvider = uriProvider;
            _userAvatarProvider = userAvatarProvider;
        }

        public async Task<Watch> GetByIdAsync(string id, string userId)
        {
            // Get entity
            ProjectEntity projectEntity = await _projectRepository.GetAsync(id);
            CheckProject(userId, projectEntity);

            // Get recent comments and total comments count
            Dictionary<string, int> commentsCount = (await _commentRepository.GetCommentsCountByProjectsAsync(new[] { projectEntity.Id })).ToDictionary(c => c.ProjectId, c => c.CommentsCount);
            Dictionary<string, List<CommentEntity>> recentComments = await _commentRepository.GetRecentCommentsByProjectsAsync(new[] { projectEntity.Id }, RecentCommentsLimit);

            // Get users for project and comments
            string[] userIds = GetUserIds(new List<ProjectEntity> { projectEntity }, recentComments);
            Dictionary<string, UserEntity> users = (await _userRepository.GetUsersByIdsAsync(userIds)).ToDictionary(c => c.Id);

            return AggregateProject(projectEntity, userId, users, commentsCount, recentComments);
        }

        public async Task CheckProjectAsync(string id, string userId)
        {
            // Get entity
            ProjectEntity projectEntity = await _projectRepository.GetAsync(id);
            CheckProject(userId, projectEntity);
        }

        public async Task<List<Watch>> GetByIdsAsync(string[] ids, string userId)
        {
            // Receive entities
            IEnumerable<ProjectEntity> projects = await _projectRepository.GetByIdsAsync(ids);

            // Check access: allowed for owner and for public or hidden projects
            projects = projects.Where(p => p.UserId == userId || _projectAccess.Contains(p.Access));

            // Get recent project comments and total comment counts
            string[] projectIds = projects.Select(p => p.Id).ToArray();
            Dictionary<string, int> commentsCounts = (await _commentRepository.GetCommentsCountByProjectsAsync(projectIds)).ToDictionary(c => c.ProjectId, c => c.CommentsCount);
            Dictionary<string, List<CommentEntity>> recentComments = await _commentRepository.GetRecentCommentsByProjectsAsync(projectIds, RecentCommentsLimit);

            // Get project user ids united with comment user ids to load entities in batch
            string[] userIds = GetUserIds(projects, recentComments);
            Dictionary<string, UserEntity> users = (await _userRepository.GetUsersByIdsAsync(userIds)).ToDictionary(u => u.Id);

            // Post-processing
            return projects.Select(p => AggregateProject(p, null, users, commentsCounts, recentComments)).ToList();
        }

        public async Task<DataResult<Watch>> GetSequenceAsync(DataQueryOptions filter, string userId)
        {
            var query = new List<IMongoQuery>(filter.Filters.Count);

            // Filtering
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            string requestedUserId = null;
            var searchList = new List<string>();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.Name),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by name
                    searchList.Add(f.Value.ToString().ToLowerInvariant());
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.UserId),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by user id
                    requestedUserId = f.Value.ToString();
                    query.Add(Query<ProjectEntity>.EQ(p => p.UserId, requestedUserId));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.Generator),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by product id
                    int productId = Int32.Parse(f.Value.ToString());
                    query.Add(Query<ProjectEntity>.EQ(p => p.ProductId, productId));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.ProjectType),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by project type
                    object projectType = Enum.Parse(typeof (ProjectType), f.Value.ToString());
                    query.Add(Query<ProjectEntity>.EQ(p => p.ProjectType, (int)projectType));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.ProjectSubtype),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by project subtype
                    object projectSubtype = Enum.Parse(typeof (ProjectSubtype), f.Value.ToString());
                    query.Add(Query<ProjectEntity>.EQ(p => p.ProjectSubtype, (int)projectSubtype));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.External.VideoUri),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by external video uri
                    query.Add(Query<ProjectEntity>.EQ(p => p.VideoSource, f.Value));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.Created),
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
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.HitsCount),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // by hits count
                    int threshold = Int32.Parse(f.Value.ToString());

                    if (f.Type == DataFilterTypes.GreaterThan)
                    {
                        query.Add(Query<ProjectEntity>.GT(p => p.HitsCount, threshold));
                    }
                    else if (f.Type == DataFilterTypes.GreaterThanOrEqual)
                    {
                        query.Add(Query<ProjectEntity>.GTE(p => p.HitsCount, threshold));
                    }
                    else
                    {
                        query.Add(Query<ProjectEntity>.EQ(p => p.HitsCount, threshold));
                    }
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<Watch>(x => x.State),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by video state
                    var videoState = (WatchState)Enum.Parse(typeof (WatchState), f.Value.ToString());
                    if (videoState == WatchState.Uploading)
                    {
                        var avsx = Query<ProjectEntity>.EQ(p => p.AvsxFileId, null);
                        var originalFileId = Query<ProjectEntity>.EQ(p => p.OriginalVideoFileId, null);
                        var externalSource = Query<ProjectEntity>.EQ(p => p.VideoSource, null);
                        query.Add(Query.Or(avsx, Query.And(originalFileId, externalSource)));
                    }
                    else if (videoState == WatchState.Encoding)
                    {
                        var avsx = Query<ProjectEntity>.NE(p => p.AvsxFileId, null);

                        var originalFileId = Query<ProjectEntity>.NE(p => p.OriginalVideoFileId, null);
                        var encodedVideosName = NameOfHelper.PropertyName<ProjectEntity>(x => x.EncodedVideos);
                        var encodedVideos = Query.NotExists(string.Format("{0}.0", encodedVideosName));

                        query.Add(Query.And(avsx, originalFileId, encodedVideos));
                    }
                    else if (videoState == WatchState.Ready)
                    {
                        var avsx = Query<ProjectEntity>.NE(p => p.AvsxFileId, null);
                        var externalSource = Query<ProjectEntity>.NE(p => p.VideoSource, null);

                        var originalFileId = Query<ProjectEntity>.NE(p => p.OriginalVideoFileId, null);
                        var encodedVideosName = NameOfHelper.PropertyName<ProjectEntity>(x => x.EncodedVideos);
                        var encodedVideos = Query.Exists(string.Format("{0}.0", encodedVideosName));

                        query.Add(Query.And(avsx, Query.Or(externalSource, Query.And(originalFileId, encodedVideos))));
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            string searchText = String.Join(" ", searchList);
            if (!String.IsNullOrEmpty(searchText))
            {
                query.Add(Query.Text(searchText));
            }

            // we should see only public videos for other users
            if (requestedUserId != userId)
            {
                query.Add(Query<ProjectEntity>.EQ(p => p.Access, (int)ProjectAccess.Public));
            }

            MongoCursor<ProjectEntity> cursor = _projectRepository.Collection.Find(query.Count > 0 ? Query.And(query) : Query.Null);

            // Sorting
            IMongoSortBy sortOrder = null;
            if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<Watch>(x => x.Name),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by name
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<ProjectEntity>.Ascending(p => p.Name)
                    : SortBy<ProjectEntity>.Descending(p => p.Name);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<Watch>(x => x.Created),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by created
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<ProjectEntity>.Ascending(p => p.Created)
                    : SortBy<ProjectEntity>.Descending(p => p.Created);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<Watch>(x => x.HitsCount),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by hits count
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<ProjectEntity>.Ascending(p => p.HitsCount)
                    : SortBy<ProjectEntity>.Descending(p => p.HitsCount);
            }

            if (sortOrder != null)
            {
                cursor.SetSortOrder(sortOrder);
            }

            // Paging
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

            List<ProjectEntity> projects = cursor.ToList();

            // Get comments and total comment counts
            string[] projectIds = projects.Select(p => p.Id).ToArray();
            Dictionary<string, int> commentsCounts = (await _commentRepository.GetCommentsCountByProjectsAsync(projectIds)).ToDictionary(c => c.ProjectId, c => c.CommentsCount);
            Dictionary<string, List<CommentEntity>> recentComments = await _commentRepository.GetRecentCommentsByProjectsAsync(projectIds, RecentCommentsLimit);

            // Get project user ids united with comment user ids to load entities in batch
            string[] userIds = GetUserIds(projects, recentComments);
            Dictionary<string, UserEntity> users = (await _userRepository.GetUsersByIdsAsync(userIds)).ToDictionary(u => u.Id);

            // Post-processing
            return new DataResult<Watch>(projects.Select(p => AggregateProject(p, userId, users, commentsCounts, recentComments)), count);
        }

        private Watch AggregateProject(ProjectEntity projectEntity, string userId, Dictionary<string, UserEntity> users, Dictionary<string, int> commentsCounts,
            Dictionary<string, List<CommentEntity>> comments)
        {
            DomainProject project = _mapper.Map<ProjectEntity, DomainProject>(projectEntity);

            // 1. Aggregate Data

            UserEntity projectUser = users.ContainsKey(project.UserId) ? users[project.UserId] : null;
            int commentsCount = commentsCounts.ContainsKey(project.Id) ? commentsCounts[project.Id] : 0;
            List<CommentEntity> projectComments = comments.ContainsKey(project.Id) ? comments[project.Id] : new List<CommentEntity>();


            // Processed Videos
            var watchVideos = new List<WatchVideo>();
            watchVideos.AddRange(
                from @group in project.EncodedVideos.GroupBy(q => q.Width)
                select @group.ToList()
                into processedVideos
                where processedVideos.Count == 2
                from v in processedVideos
                select new WatchVideo
                {
                    ContentType = v.ContentType,
                    Width = v.Width,
                    Height = v.Height,
                    Uri = _uriProvider.CreateUri(v.FileId)
                });

            // Processed Screenshots
            List<WatchScreenshot> watchScreenshots = project.EncodedScreenshots.Select(
                s => new WatchScreenshot
                {
                    ContentType = s.ContentType,
                    Uri = _uriProvider.CreateUri(s.FileId)
                }).ToList();

            // External Video
            ExternalVideo externalVideo = !string.IsNullOrEmpty(project.VideoSource)
                ? new ExternalVideo
                {
                    ProductName = project.VideoSourceProductName,
                    VideoUri = project.VideoSource,
                    AcsNamespace = _settings.AcsNamespace
                }
                : null;


            // 2. Calculate Video State
            WatchState state;
            if (string.IsNullOrEmpty(project.AvsxFileId) || (string.IsNullOrEmpty(project.OriginalVideoFileId) && externalVideo == null))
            {
                state = WatchState.Uploading;
            }
            else if (externalVideo == null && watchVideos.Count == 0)
            {
                state = WatchState.Encoding;
            }
            else
            {
                state = WatchState.Ready;
            }

            // 3. Map comments
            var watchComments = new List<Comment>();
            foreach (CommentEntity projectComment in projectComments)
            {
                UserEntity commentAuthor = users.ContainsKey(projectComment.UserId) ? users[projectComment.UserId] : null;
                DomainComment domainComment = _mapper.Map<Tuple<CommentEntity, UserEntity>, DomainComment>(new Tuple<CommentEntity, UserEntity>(projectComment, commentAuthor));

                Comment comment = _mapper.Map<DomainComment, Comment>(domainComment);
                if (commentAuthor != null)
                {
                    comment.AvatarUrl = _userAvatarProvider.GetAvatar(commentAuthor.Email);
                }

                watchComments.Add(comment);
            }
            watchComments.Reverse();

            // Result
            return new Watch
            {
                Name = project.Name,
                UserId = project.UserId,
                UserName = projectUser != null ? projectUser.Name : null,
                UserAvatarUrl = projectUser != null ? _userAvatarProvider.GetAvatar(new DomainUser { Email = projectUser.Email }) : null,
                Description = project.Description,
                Created = project.Created,
                Avsx = !string.IsNullOrEmpty(project.AvsxFileId) ? _uriProvider.CreateUri(project.AvsxFileId) : null,
                Screenshots = watchScreenshots,
                Videos = watchVideos,
                PublicUrl = _projectUriProvider.GetUri(project.Id),
                Id = project.Id,
                Access = project.Access,
                IsEditable = project.UserId == userId,
                HitsCount = project.HitsCount,
                External = externalVideo,
                ScreenshotUrl = !string.IsNullOrEmpty(project.ScreenshotFileId) ? _uriProvider.CreateUri(project.ScreenshotFileId) : null,
                State = state,
                Generator = (int)project.ProductType,
                ProjectType = project.ProjectType,
                ProjectSubtype = project.ProjectSubtype,
                EnableComments = project.EnableComments,
                CommentsCount = commentsCount,
                LikesCount = project.LikesCount,
                DislikesCount = project.DislikesCount,
                Comments = watchComments
            };
        }

        private static string[] GetUserIds(IEnumerable<ProjectEntity> projects, Dictionary<string, List<CommentEntity>> projectComments)
        {
            var userHashset = new HashSet<string>();
            foreach (ProjectEntity project in projects)
            {
                if (userHashset.Contains(project.UserId))
                {
                    continue;
                }

                userHashset.Add(project.UserId);
            }
            foreach (var projectComment in projectComments)
            {
                foreach (CommentEntity c in projectComment.Value)
                {
                    if (userHashset.Contains(c.UserId))
                    {
                        continue;
                    }

                    userHashset.Add(c.UserId);
                }
            }

            return userHashset.ToArray();
        }

        private void CheckProject(string userId, ProjectEntity projectEntity)
        {
            if (projectEntity == null)
            {
                throw new NotFoundException();
            }

            if (!_projectAccess.Contains(projectEntity.Access))
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedException();
                }

                if (projectEntity.UserId != userId)
                {
                    throw new ForbiddenException();
                }
            }
        }
    }
}