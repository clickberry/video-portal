// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asp.Infrastructure.Commands;
using MiddleEnd.Worker.Abstract;
using MiddleEnd.Worker.Infrastructure;
using Portal.Domain.ProcessedVideoContext;
using Portal.DTO.Sheduler;

namespace MiddleEnd.Api.Commands.Tasks
{
    /// <summary>
    ///     Handles task distribution.
    /// </summary>
    public class TaskPostCommand : ControllerCommandApi<HttpResponseMessage>
    {
        private readonly ITaskProvider _taskProvider;

        public TaskPostCommand(ApiController controller, ITaskProvider taskProvider)
            : base(controller)
        {
            _taskProvider = taskProvider;
        }

        public override HttpResponseMessage Execute()
        {
            // Gets an accepted task types
            ParsedTaskRequest request = HttpHelper.GetTaskRequest(Controller.Request);

            if (request.Accepts.Count == 0)
            {
                return Controller.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request.");
            }

            // Gets a task
            IProcessedEntity task = _taskProvider.GetNext(request.Accepts);

            if (task == null)
            {
                return Controller.Request.CreateResponse(HttpStatusCode.NoContent);
            }

            // Creates a response
            HttpResponseMessage responseMessage = Controller.Request.CreateResponse(
                HttpStatusCode.Created,
                new VideoProcessingTask
                {
                    TaskId = task.TaskId
                });

            responseMessage.Content.Headers.ContentType.MediaType = HttpHelper.GetContentType(task.EntityType);

            var routeParameters = new Dictionary<string, object>
            {
                { "id", task.TaskId }
            };

            string controllerName = HttpHelper.GetControllerName(task.EntityType);
            string routeAddress = Controller.Url.Route(controllerName + "Api", routeParameters);
            if (routeAddress != null)
            {
                responseMessage.Headers.Location = new Uri(routeAddress, UriKind.Relative);
            }

            return responseMessage;
        }
    }
}