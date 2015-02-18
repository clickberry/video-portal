// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asp.Infrastructure.Commands;
using MiddleEnd.Api.Models;
using MiddleEnd.Worker.Abstract;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProcessedVideoContext.States;

namespace MiddleEnd.Api.Commands.Common
{
    public class UpdateEntityCommand : ControllerCommandApi<HttpResponseMessage>
    {
        private readonly string _id;
        private readonly UpdateProcessingModel _model;
        private readonly ITaskProvider _taskKeeper;

        public UpdateEntityCommand(ApiController controller, string id, UpdateProcessingModel model, ITaskProvider taskKeeper)
            : base(controller)
        {
            _id = id;
            _model = model;
            _taskKeeper = taskKeeper;
        }

        public ProcessedEntityType ProcessedEntityType { get; set; }

        public override HttpResponseMessage Execute()
        {
            IProcessedEntity task = _taskKeeper.Get(_id);

            if (task == null)
            {
                return Controller.Request.CreateErrorResponse(HttpStatusCode.NotFound, "NotFound");
            }

            if (task.EntityType != ProcessedEntityType || task.State != TaskState.Processing)
            {
                return Controller.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid entity identifier.");
            }

            task.SetProgress(_model.Progress);

            return Controller.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}