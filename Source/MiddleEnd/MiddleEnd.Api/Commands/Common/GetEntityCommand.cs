// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asp.Infrastructure.Commands;
using MiddleEnd.Worker.Abstract;
using MiddleEnd.Worker.Infrastructure;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProcessedVideoContext.States;

namespace MiddleEnd.Api.Commands.Common
{
    public class GetEntityCommand : ControllerCommandApi<HttpResponseMessage>
    {
        private readonly string _id;
        private readonly ITaskProvider _taskKeeper;

        public GetEntityCommand(ApiController controller, string id, ITaskProvider taskKeeper)
            : base(controller)
        {
            _id = id;
            _taskKeeper = taskKeeper;
        }

        public ProcessedEntityType ProcessedEntityType { get; set; }

        public Func<IProcessedEntity, object> GetResponseEntity { get; set; }

        public override HttpResponseMessage Execute()
        {
            IProcessedEntity task = _taskKeeper.Get(_id);

            if (task == null || task.EntityType != ProcessedEntityType)
            {
                return Controller.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid entity identifier.");
            }

            task.Started = DateTime.UtcNow;

            try
            {
                task.SetState(new ProcessingState());
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to change task state: {0}", e);
            }

            HttpResponseMessage response = Controller.Request.CreateResponse(HttpStatusCode.OK, GetResponseEntity(task));
            response.Content.Headers.ContentType.MediaType = HttpHelper.GetContentType(task.EntityType);

            return response;
        }
    }
}