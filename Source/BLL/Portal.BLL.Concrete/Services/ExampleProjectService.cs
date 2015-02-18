// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Common.Helpers;
using Portal.DAL.Comment;
using Portal.DAL.Entities.Storage;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;
using Portal.DAL.User;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;
using Portal.DTO.Watch;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class ExampleProjectService : IExampleProjectService
    {
        private const int RecentCommentsLimit = 3;

        private readonly IAdminUserService _adminUserService;
        private readonly ICommentRepository _commentRepository;
        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IFileUriProvider _uriProvider;
        private readonly IUserAvatarProvider _userAvatarProvider;
        private readonly IUserRepository _userRepository;

        public ExampleProjectService(
            IProjectService projectService,
            IAdminUserService adminUserService,
            IFileSystem fileSystem,
            IFileUriProvider uriProvider,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IUserAvatarProvider userAvatarProvider,
            IMapper mapper)
        {
            _projectService = projectService;
            _adminUserService = adminUserService;
            _fileSystem = fileSystem;
            _uriProvider = uriProvider;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _userAvatarProvider = userAvatarProvider;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExampleProject>> GetSequenceAsync(DataQueryOptions filter)
        {
            // Get example managers
            List<DomainUser> users = await _adminUserService.GetUsersInRoleAsync(DomainRoles.ExamplesManager);
            if (users.Count == 0)
            {
                return new List<ExampleProject>();
            }

            // Get their projects
            IEnumerable<DomainProject> projects = await _projectService.GetProjectListByUsersAsync(users);

            // Only public videos
            projects = projects.Where(p => p.Access == ProjectAccess.Public);

            // Paging
            if (filter.Skip.HasValue)
            {
                projects = projects.Skip(filter.Skip.Value);
            }
            if (filter.Take.HasValue)
            {
                projects = projects.Take(filter.Take.Value);
            }

            // Sorting
            if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<ExampleProject>(x => x.Created),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                projects = filter.OrderByDirection == OrderByDirections.Asc
                    ? projects.OrderBy(p => p.Created)
                    : projects.OrderByDescending(p => p.Created);
            }

            List<DomainProject> result = projects.ToList();

            // Aggregating project data
            List<Task<ExampleProject>> tasks = result.Select(GetProjectAsync).ToList();
            await Task.WhenAll(tasks);

            List<ExampleProject> examples = tasks.Select(t => t.Result).ToList();


            // Post-processing

            // Get comments and total comment counts
            string[] projectIds = result.Select(p => p.Id).ToArray();
            Dictionary<string, int> commentsCounts = (await _commentRepository.GetCommentsCountByProjectsAsync(projectIds)).ToDictionary(c => c.ProjectId, c => c.CommentsCount);
            Dictionary<string, List<CommentEntity>> recentComments = await _commentRepository.GetRecentCommentsByProjectsAsync(projectIds, RecentCommentsLimit);

            // Get related users list
            var userIdsSet = new HashSet<string>();
            foreach (var projectComment in recentComments)
            {
                foreach (CommentEntity c in projectComment.Value)
                {
                    if (userIdsSet.Contains(c.UserId))
                    {
                        continue;
                    }

                    userIdsSet.Add(c.UserId);
                }
            }
            Dictionary<string, UserEntity> commentUsers = (await _userRepository.GetUsersByIdsAsync(userIdsSet.ToArray())).ToDictionary(u => u.Id);

            // Append comments data to exmaples
            foreach (ExampleProject example in examples)
            {
                // comments total count
                example.Comments = commentsCounts.ContainsKey(example.Id) ? commentsCounts[example.Id] : 0;

                // comments list
                if (recentComments.ContainsKey(example.Id))
                {
                    var exampleComments = new List<Comment>();
                    List<CommentEntity> projectComments = recentComments[example.Id];
                    foreach (CommentEntity projectComment in projectComments)
                    {
                        UserEntity commentAuthor = commentUsers.ContainsKey(projectComment.UserId) ? commentUsers[projectComment.UserId] : null;
                        DomainComment domainComment = _mapper.Map<Tuple<CommentEntity, UserEntity>, DomainComment>(new Tuple<CommentEntity, UserEntity>(projectComment, commentAuthor));

                        Comment comment = _mapper.Map<DomainComment, Comment>(domainComment);
                        if (commentAuthor != null)
                        {
                            comment.AvatarUrl = _userAvatarProvider.GetAvatar(commentAuthor.Email);
                        }

                        exampleComments.Add(comment);
                    }

                    exampleComments.Reverse();
                    example.CommentList = exampleComments;
                }
                else
                {
                    example.CommentList = new List<Comment>();
                }
            }

            return examples;
        }

        private async Task<ExampleProject> GetProjectAsync(DomainProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            // Avsx
            Task<StorageFile> avsxTask = Task.FromResult<StorageFile>(null);
            if (!string.IsNullOrEmpty(project.AvsxFileId))
            {
                avsxTask = _fileSystem.GetFilePropertiesSlimAsync(new StorageFile { Id = project.AvsxFileId });
            }

            // Screenshot
            Task<StorageFile> screenshotTask = Task.FromResult<StorageFile>(null);
            if (!string.IsNullOrEmpty(project.ScreenshotFileId))
            {
                screenshotTask = _fileSystem.GetFilePropertiesSlimAsync(new StorageFile { Id = project.ScreenshotFileId });
            }

            // Original video
            Task<StorageFile> videoTask = Task.FromResult<StorageFile>(null);
            if (!string.IsNullOrEmpty(project.OriginalVideoFileId))
            {
                videoTask = _fileSystem.GetFilePropertiesSlimAsync(new StorageFile { Id = project.OriginalVideoFileId });
            }

            await Task.WhenAll(new[]
            {
                avsxTask,
                screenshotTask,
                videoTask
            });


            var result = new ExampleProject
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Created = project.Created,
                Views = project.HitsCount,
                Likes = project.LikesCount,
                Dislikes = project.DislikesCount,
                ProjectType = (int)project.ProjectType,
                ProjectSubtype = (int)project.ProjectSubtype
            };


            if (avsxTask.Result != null)
            {
                result.AvsxUri = _uriProvider.CreateUri(avsxTask.Result.Id);
                result.TotalSize += avsxTask.Result.Length;
            }

            if (screenshotTask.Result != null)
            {
                result.ScreenshotUri = _uriProvider.CreateUri(screenshotTask.Result.Id);
                result.TotalSize += screenshotTask.Result.Length;
            }

            if (videoTask.Result != null)
            {
                result.VideoUri = _uriProvider.CreateUri(videoTask.Result.Id);
                result.VideoHash = videoTask.Result.Id;
                result.TotalSize += videoTask.Result.Length;
            }

            return result;
        }
    }
}