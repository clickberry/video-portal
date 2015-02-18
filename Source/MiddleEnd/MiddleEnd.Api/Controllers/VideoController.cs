// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Web.Http;
using MiddleEnd.Api.Attributes;
using MiddleEnd.Api.Commands.Common;
using MiddleEnd.Api.Models;
using MiddleEnd.Worker.Abstract;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.ProcessedVideoContext;

namespace MiddleEnd.Api.Controllers
{
    [Authorize]
    [Authentication]
    [Route("api/tasks/{id}/video", Name = "VideoApi")]
    public class VideoController : ApiController
    {
        private readonly ITaskChecker _taskChecker;
        private readonly ITaskProvider _taskProvider;

        public VideoController(ITaskProvider taskProvider, ITaskChecker taskChecker)
        {
            _taskProvider = taskProvider;
            _taskChecker = taskChecker;
        }

        // GET api/tasks/5/video

        public HttpResponseMessage Get(string id)
        {
            return new GetEntityCommand(this, id, _taskProvider)
            {
                ProcessedEntityType = ProcessedEntityType.Video,
                GetResponseEntity = entity =>
                {
                    var data = entity as DomainProcessedVideo;
                    if (data == null)
                    {
                        throw new ArgumentOutOfRangeException("entity");
                    }

                    return new VideoEncodeData
                    {
                        ContentType = data.ContentType,
                        AudioParam = data.AudioParam,
                        VideoParam = data.VideoParam,
                        IsAudioCopy = data.IsAudioCopy,
                        IsVideoCopy = data.IsVideoCopy,
                        SourceFileId = data.SourceFileId
                    };
                }
            }.Execute();
        }

        public HttpResponseMessage Put(string id, UpdateProcessingModel model)
        {
            return new UpdateEntityCommand(this, id, model, _taskProvider) { ProcessedEntityType = ProcessedEntityType.Video }.Execute();
        }

        public HttpResponseMessage Delete(string id, DeleteProcessingModel model)
        {
            return new DeleteEntityCommand(this, id, model, _taskProvider, _taskChecker) { ProcessedEntityType = ProcessedEntityType.Video }.Execute();
        }
    }
}