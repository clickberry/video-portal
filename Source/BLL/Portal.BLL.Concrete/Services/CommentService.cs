// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoRepository;
using Portal.BLL.Services;
using Portal.DAL.Authentication;
using Portal.DAL.Comment;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.User;
using Portal.Domain.ProjectContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public class CommentService : ICommentService
    {
        private readonly IAuthenticator _authenticator;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly int[] _projectAccess = { (int)ProjectAccess.Public, (int)ProjectAccess.Hidden };
        private readonly IRepository<ProjectEntity> _projectRepository;
        private readonly IUserRepository _userRepository;

        public CommentService(IRepositoryFactory repositoryFactory,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IAuthenticator authenticator,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _authenticator = authenticator;
            _mapper = mapper;
            _projectRepository = repositoryFactory.Create<ProjectEntity>();
        }

        public async Task<List<DomainComment>> GetCommentsAsync(string projectId, string userId)
        {
            ProjectEntity projectEntity = await _projectRepository.GetAsync(projectId);
            CheckProject(projectEntity, userId);

            List<CommentEntity> commentEntities = await _commentRepository.GetCommentsAsync(projectId);

            List<UserEntity> userEntities = await _userRepository.GetUsersByIdsAsync(commentEntities.Select(c => c.UserId).ToArray());

            List<DomainComment> domainComments = commentEntities
                .Select(entity => _mapper.Map<Tuple<CommentEntity, UserEntity>, DomainComment>(
                    new Tuple<CommentEntity, UserEntity>(
                        entity,
                        userEntities.FirstOrDefault(u => u.Id == entity.UserId))))
                .ToList();

            return domainComments;
        }

        public async Task<DomainComment> AddCommentAsync(DomainComment domainComment)
        {
            ProjectEntity projectEntity = await _projectRepository.GetAsync(domainComment.ProjectId);
            CheckProject(projectEntity, domainComment.UserId);

            CommentEntity commentEntity = _mapper.Map<DomainComment, CommentEntity>(domainComment);
            await _commentRepository.AddAsync(commentEntity);

            return CreateDomainComment(commentEntity, projectEntity.UserId);
        }

        public async Task<DomainComment> EditCommentAsync(DomainComment domainComment)
        {
            ProjectEntity projectEntity = await _projectRepository.GetAsync(domainComment.ProjectId);
            CheckProject(projectEntity, domainComment.UserId);

            CommentEntity commentEntity = await _commentRepository.GetAsync(domainComment.Id);
            if (commentEntity == null)
            {
                throw new NotFoundException();
            }
            if (commentEntity.UserId != domainComment.UserId)
            {
                throw new ForbiddenException();
            }
            commentEntity.Body = domainComment.Body;

            await _commentRepository.UpdateAsync(commentEntity);

            return CreateDomainComment(commentEntity, projectEntity.UserId);
        }

        public async Task DeleteCommentAsync(DomainComment domainComment)
        {
            ProjectEntity projectEntity = await _projectRepository.GetAsync(domainComment.ProjectId);
            CheckProject(projectEntity, domainComment.UserId);

            CommentEntity commentEntity = await _commentRepository.GetAsync(domainComment.Id);
            if (commentEntity == null)
            {
                throw new NotFoundException();
            }
            if (commentEntity.UserId != domainComment.UserId)
            {
                throw new ForbiddenException();
            }

            await _commentRepository.DeleteAsync(commentEntity.Id);
        }

        private void CheckProject(ProjectEntity projectEntity, string userId)
        {
            if (projectEntity == null)
            {
                throw new NotFoundException();
            }

            // If comments are disabled returns forbidden
            // for both own and other user projects
            if (!projectEntity.EnableComments)
            {
                throw new ForbiddenException();
            }

            // If it's own project skip access checking
            if (projectEntity.UserId == userId)
            {
                return;
            }

            // If it's other user project check access
            if (!_projectAccess.Contains(projectEntity.Access))
            {
                throw new ForbiddenException();
            }
        }

        private DomainComment CreateDomainComment(CommentEntity commentEntity, string ownerId)
        {
            var userEntity = new UserEntity
            {
                Id = commentEntity.UserId,
                Name = _authenticator.GetUserName(),
                Email = _authenticator.GetUserEmail()
            };

            DomainComment comment = _mapper.Map<Tuple<CommentEntity, UserEntity>, DomainComment>(
                new Tuple<CommentEntity, UserEntity>(commentEntity, userEntity));

            comment.OwnerId = ownerId;

            return comment;
        }
    }
}