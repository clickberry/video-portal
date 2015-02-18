// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;
using Portal.Mappers;

namespace Portal.Api.Controllers.Projects
{
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [RoutePrefix("projects/{projectId}/comments")]
    public class CommentsController : ApiControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IUserAvatarProvider _userAvatarProvider;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IEmailNotificationService _notificationService;

        public CommentsController(ICommentService commentService,
            IMapper mapper,
            IUserAvatarProvider userAvatarProvider,
            IProjectService projectService,
            IUserService userService,
            IEmailNotificationService notificationService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _userAvatarProvider = userAvatarProvider;
            _projectService = projectService;
            _userService = userService;
            _notificationService = notificationService;
        }

        [Route]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get(string projectId)
        {
            List<DomainComment> domainComments = await _commentService.GetCommentsAsync(projectId, UserId);

            IEnumerable<Comment> responseComments = domainComments
                .OrderByDescending(c => c.DateTime)
                .Select(comment => _mapper.Map<Tuple<DomainComment, string>, Comment>(
                    new Tuple<DomainComment, string>(
                        comment,
                        _userAvatarProvider.GetAvatar(new DomainUser { Email = comment.UserEmail }))));

            return Request.CreateResponse(HttpStatusCode.OK, responseComments);
        }

        [Route]
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Post(string projectId, CommentModel comment)
        {
            var domainComment = new DomainComment
            {
                ProjectId = projectId,
                UserId = UserId,
                Body = comment.Body,
                DateTime = DateTime.UtcNow
            };

            domainComment = await _commentService.AddCommentAsync(domainComment);

            var avatarUrl = _userAvatarProvider.GetAvatar(new DomainUser { Email = domainComment.UserEmail });
            var responseComment = _mapper.Map<Tuple<DomainComment, string>, Comment>(
                new Tuple<DomainComment, string>(domainComment, avatarUrl));

            var project = await _projectService.GetAsync(new DomainProject
            {
                Id = domainComment.ProjectId,
                UserId = domainComment.OwnerId
            });

            // Notify owner about video comments
            if (project.UserId != domainComment.UserId)
            {
                var videoOwner = await _userService.GetAsync(project.UserId);

                // Checks whether video comments notification enabled
                if (videoOwner.NotifyOnVideoComments)
                {
                    // Send notification e-mail
                    try
                    {
                        await _notificationService.SendVideoCommentNotificationAsync(videoOwner, project, domainComment);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to send video comment notification email to address {0} for user {1}: {2}", videoOwner.Email, videoOwner.Id, e);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.Created, responseComment);
        }

        [Route("{commentId}")]
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Put(string projectId, string commentId, CommentModel comment)
        {
            var domainComment = new DomainComment
            {
                Id = commentId,
                ProjectId = projectId,
                UserId = UserId,
                Body = comment.Body
            };

            domainComment = await _commentService.EditCommentAsync(domainComment);

            var avatarUrl = _userAvatarProvider.GetAvatar(new DomainUser { Email = domainComment.UserEmail });
            var responseComment = _mapper.Map<Tuple<DomainComment, string>, Comment>(
                new Tuple<DomainComment, string>(domainComment, avatarUrl));

            return Request.CreateResponse(HttpStatusCode.OK, responseComment);
        }

        [Route("{commentId}")]
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete(string projectId, string commentId)
        {
            var domainComment = new DomainComment
            {
                Id = commentId,
                ProjectId = projectId,
                UserId = UserId
            };

            await _commentService.DeleteCommentAsync(domainComment);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}