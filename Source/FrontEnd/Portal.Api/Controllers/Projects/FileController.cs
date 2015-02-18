// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.WebApi;
using Asp.Infrastructure.Extensions;
using Portal.Api.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.PortalContext;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;
using Portal.DTO.Projects;
using Portal.Exceptions.Multimedia;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Projects
{
    /// <summary>
    ///     Video file controller.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [ClearFilesHttp]
    [Route("projects/{id}/video/file")]
    public class FileController : ProjectEntityControllerBase
    {
        private readonly IProjectVideoService _videoRepository;

        public FileController(IProjectVideoService videoRepository, IProjectService projectRepository)
            : base(projectRepository)
        {
            _videoRepository = videoRepository;
        }

        // POST api/projects/{id}/video/file
        [CheckAccessHttp]
        [StatProjectState(StatActionType.Video)]
        public async Task<HttpResponseMessage> Post(string id, ProjectVideoModel model)
        {
            // Check whether project with such id exists
            await GetProjectAsync(id);

            // Add video to that project
            var video = new DomainVideo
            {
                FileName = model.Video.Name,
                FileUri = model.Video.Uri,
                ContentType = model.Video.ContentType,
                FileLength = model.Video.Length
            };

            try
            {
                video = await _videoRepository.AddAsync(id, video);
            }
            catch (AggregateException aggregateException)
            {
                // Handle VideoFormatExceptions
                var errorsList = new StringBuilder(String.Format("{0}\n", aggregateException.Message));
                foreach (Exception exception in aggregateException.InnerExceptions)
                {
                    var videoFormatException = exception as VideoFormatException;
                    if (videoFormatException == null)
                    {
                        Trace.TraceError("Failed to add a video file: {0}", exception);
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ResponseMessages.InternalServerError);
                    }

                    string errorMessage = GetExceptionDescription(videoFormatException.ParamType);
                    errorsList.AppendLine(errorMessage);
                    ModelState.AddModelError("Video", errorMessage);
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var projectVideo = new ProjectVideo
            {
                Name = video.FileName,
                Uri = video.FileUri,
                ContentType = video.ContentType,
                Length = video.FileLength
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, projectVideo);
            response.SetLastModifiedDate(video.Modified);

            return response;
        }

        // PUT api/projects/{id}/video/file
        public HttpResponseMessage Put(string id, ProjectVideoModel model)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ResponseMessages.MethodNotAllowed);
        }

        /// <summary>
        ///     Gets localized error description.
        /// </summary>
        /// <param name="parameter">Media parameter.</param>
        /// <returns>Description.</returns>
        private static string GetExceptionDescription(ParamType parameter)
        {
            switch (parameter)
            {
                case ParamType.AudioBitrate:
                    return ResponseMessages.InvalidAudioBitrate;

                case ParamType.AudioChannel:
                    return ResponseMessages.InvalidAudioChannels;

                case ParamType.AudioCodec:
                    return ResponseMessages.InvalidAudioCodec;

                case ParamType.AudioStreamCount:
                    return ResponseMessages.InvalidAudioStreamsCount;

                case ParamType.FrameRate:
                    return ResponseMessages.InvalidVideoFrameRate;

                case ParamType.Height:
                    return ResponseMessages.InvalidVideoHeight;

                case ParamType.MediaContainer:
                    return ResponseMessages.InvalidMediaContainer;

                case ParamType.VideoBitRate:
                    return ResponseMessages.InvalidVideoBitrate;

                case ParamType.VideoCodec:
                    return ResponseMessages.InvalidVideoCodec;

                case ParamType.VideoStreamCount:
                    return ResponseMessages.InvalidVideoStreamsCount;

                case ParamType.Width:
                    return ResponseMessages.InvalidVideoWidth;

                default:
                    return ResponseMessages.InvalidMedia;
            }
        }
    }
}