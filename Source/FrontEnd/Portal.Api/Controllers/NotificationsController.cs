// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Extensions;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.Notifications;
using Portal.Domain.PortalContext;
using Portal.Domain.ProjectContext;
using Portal.DTO.Notifications;
using Portal.Exceptions.CRUD;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    [AuthorizeHttp(Roles = DomainRoles.ExamplesManager)]
    [ValidationHttp]
    [Route("notifications/{id?}")]
    public class NotificationsController : ApiControllerBase
    {
        private readonly IService<DomainNotification> _notificationService;
        private readonly IProjectService _projectService;

        public NotificationsController(IService<DomainNotification> notificationService, IProjectService projectService)
        {
            _notificationService = notificationService;
            _projectService = projectService;
        }

        // GET api/notifications
        public async Task<IEnumerable<Notification>> Get()
        {
            List<DomainNotification> notifications = await _notificationService.GetListAsync(null);

            return notifications.Select(
                p => new Notification
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title
                });
        }

        // GET api/notifications/5
        public async Task<IEnumerable<Notification>> Get(string id)
        {
            // Checks whether project exist
            try
            {
                await _projectService.GetAsync(new DomainProject { Id = id });
            }
            catch (ForbiddenException)
            {
            }

            List<DomainNotification> notifications = await _notificationService.GetListAsync(
                new DomainNotification { ProjectId = id });

            return notifications.Select(
                p => new Notification
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title
                });
        }

        // POST api/notifications
        public async Task<HttpResponseMessage> Post(NotificationModel notification)
        {
            DomainNotification domainNotification;

            try
            {
                domainNotification = await _notificationService.AddAsync(
                    new DomainNotification
                    {
                        ProjectId = notification.ProjectId,
                        Title = notification.Title,
                        UserId = UserId
                    });
            }
            catch (NotFoundException)
            {
                ModelState.AddModelError("ProjectId", ResponseMessages.NotificationInvalidProjectId);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, e.Message);
            }

            var result = new Notification
            {
                ProjectId = domainNotification.ProjectId,
                Title = domainNotification.Title
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, result);
            response.SetLastModifiedDate(domainNotification.Created);

            return response;
        }
    }
}