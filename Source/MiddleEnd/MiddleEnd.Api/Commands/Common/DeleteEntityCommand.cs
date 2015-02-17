// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
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
    public class DeleteEntityCommand : ControllerCommandApi<HttpResponseMessage>
    {
        private readonly string _id;
        private readonly DeleteProcessingModel _model;
        private readonly ITaskChecker _taskChecker;
        private readonly ITaskProvider _taskProvider;

        public DeleteEntityCommand(ApiController controller, string id, DeleteProcessingModel model, ITaskProvider taskProvider, ITaskChecker taskChecker) : base(controller)
        {
            _id = id;
            _model = model;
            _taskProvider = taskProvider;
            _taskChecker = taskChecker;
        }

        public ProcessedEntityType ProcessedEntityType { get; set; }

        public override HttpResponseMessage Execute()
        {
            IProcessedEntity task = _taskProvider.Get(_id);

            if (task == null)
            {
                return Controller.Request.CreateErrorResponse(HttpStatusCode.NotFound, "NotFound");
            }

            if (task.EntityType != ProcessedEntityType || task.State != TaskState.Processing)
            {
                return Controller.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid entity identifier.");
            }

            task.Completed = DateTime.UtcNow;
            task.DestinationFileId = _model.FileId;

            try
            {
                task.SetState(TaskStateFactory.GetState(_model.Result));
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to change task state: {0}", e);
            }

            _taskChecker.CheckTask(task);


            if (!string.IsNullOrEmpty(_model.Message))
            {
                Trace.TraceError("File {0} processing failed: {1}", task.SourceFileId, _model.Message);
            }

            return Controller.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}