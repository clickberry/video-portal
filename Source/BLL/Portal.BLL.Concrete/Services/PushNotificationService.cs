// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.NotificationHub;
using Portal.Domain.Notifications;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class PushNotificationService : IService<DomainNotification>
    {
        private readonly IClientNotificationHub _clientNotificationHub;
        private readonly IMapper _mapper;
        private readonly ITableRepository<ProjectEntity> _projectRepository;
        private readonly ITableRepository<PushNotificationEntity> _pushNotificationRepository;

        public PushNotificationService(IRepositoryFactory repositoryFactory,
            IClientNotificationHub clientNotificationHub, IMapper mapper)
        {
            _clientNotificationHub = clientNotificationHub;
            _mapper = mapper;
            _pushNotificationRepository = repositoryFactory.Create<PushNotificationEntity>();
            _projectRepository = repositoryFactory.Create<ProjectEntity>();
        }

        public async Task<DomainNotification> AddAsync(DomainNotification entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (string.IsNullOrEmpty(entity.ProjectId))
            {
                throw new NullReferenceException("ProjectId");
            }

            if (string.IsNullOrEmpty(entity.UserId))
            {
                throw new NullReferenceException("UserId");
            }

            if (string.IsNullOrEmpty(entity.Title))
            {
                throw new NullReferenceException("Title");
            }

            ProjectEntity project = await _projectRepository.SingleOrDefaultAsync(p => p.Id == entity.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(string.Format("Project {0} was not found.", entity.ProjectId));
            }

            entity.Created = DateTime.UtcNow;
            entity.Id = DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);

            // Send notification
            await _clientNotificationHub.SendNotificationAsync(new HubNotification(entity.Title));

            // Save sent notification
            PushNotificationEntity notificationEntity =
                _mapper.Map<DomainNotification, PushNotificationEntity>(entity);
            await _pushNotificationRepository.AddAsync(notificationEntity);

            return entity;
        }

        public Task<List<DomainNotification>> AddAsync(IList<DomainNotification> entity)
        {
            throw new NotImplementedException();
        }

        public Task<DomainNotification> GetAsync(DomainNotification entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DomainNotification>> GetListAsync(DomainNotification entity)
        {
            IEnumerable<PushNotificationEntity> notificationEntities;

            if (entity != null && !string.IsNullOrEmpty(entity.ProjectId))
            {
                notificationEntities =
                    await _pushNotificationRepository.ToListAsync(p => p.ProjectId == entity.ProjectId);
            }
            else
            {
                notificationEntities = await _pushNotificationRepository.ToListAsync();
            }

            return
                notificationEntities.Select(p => _mapper.Map<PushNotificationEntity, DomainNotification>(p))
                    .ToList();
        }

        public Task<DomainNotification> EditAsync(DomainNotification entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DomainNotification entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IList<DomainNotification> entity)
        {
            throw new NotImplementedException();
        }
    }
}