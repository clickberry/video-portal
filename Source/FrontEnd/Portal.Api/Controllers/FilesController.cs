// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Extensions;
using Portal.Api.Infrastructure.Attributes;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.FileContext;
using Portal.Domain.PortalContext;
using Portal.DTO.Files;
using Portal.Resources.Api;

namespace Portal.Api.Controllers
{
    /// <summary>
    ///     Manages file resources.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.User)]
    [ValidationHttp]
    [Route("files/{id?}")]
    public sealed class FilesController : ApiControllerBase
    {
        private readonly IService<DomainUserFile> _userFileRepository;

        public FilesController(IService<DomainUserFile> userFileRepository)
        {
            _userFileRepository = userFileRepository;
        }

        //
        // POST /api/files

        /// <summary>
        ///     Adds file.
        /// </summary>
        /// <returns>Operation status code.</returns>
        [CheckAccessHttp]
        [MultipartHttp]
        [ClearFilesHttp]
        public async Task<HttpResponseMessage> Post(FileModel model)
        {
            var userFile = new DomainUserFile
            {
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                UserId = UserId,
                FileUri = model.File.Uri,
                ContentType = model.File.ContentType,
                FileLength = model.File.Length,
                FileName = model.Name
            };

            userFile = await _userFileRepository.AddAsync(userFile);

            var file = new File
            {
                ContentType = userFile.ContentType,
                Name = userFile.FileName,
                Uri = userFile.FileUri,
                Id = userFile.FileId,
                Length = userFile.FileLength
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, file);
            response.SetLastModifiedDate(userFile.Modified);

            return response;
        }

        //
        // GET /api/files

        /// <summary>
        ///     Receives files list.
        /// </summary>
        /// <returns>Files list.</returns>
        public HttpResponseMessage Get()
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ResponseMessages.MethodNotAllowed);
        }

        //
        // GET /api/files/{id}

        /// <summary>
        ///     Receives file by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>File.</returns>
        public async Task<HttpResponseMessage> Get([FromUri] string id)
        {
            var userFile = new DomainUserFile
            {
                UserId = UserId,
                FileId = id
            };

            userFile = await _userFileRepository.GetAsync(userFile);

            var file = new File
            {
                ContentType = userFile.ContentType,
                Name = userFile.FileName,
                Uri = userFile.FileUri,
                Id = userFile.FileId,
                Length = userFile.FileLength
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, file);
            response.SetLastModifiedDate(userFile.Modified);

            return response;
        }

        //
        // PUT /api/files/{id}

        /// <summary>
        ///     Edits file by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="model">Model.</param>
        /// <returns>Operation status code.</returns>
        [CheckAccessHttp]
        [ClearFilesHttp]
        public async Task<HttpResponseMessage> Put([FromUri] string id, FileModel model)
        {
            var userFile = new DomainUserFile
            {
                UserId = UserId,
                ContentType = model.File.ContentType,
                FileId = id,
                FileUri = model.File.Uri,
                FileLength = model.File.Length,
                FileName = model.Name
            };

            userFile = await _userFileRepository.EditAsync(userFile);

            var file = new File
            {
                ContentType = userFile.ContentType,
                Name = userFile.FileName,
                Uri = userFile.FileUri,
                Length = userFile.FileLength
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, file);
            response.SetLastModifiedDate(userFile.Modified);

            return response;
        }

        public HttpResponseMessage Put()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }

        /// <summary>
        ///     Deletes file by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation status code.</returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Delete([FromUri] string id)
        {
            var userFile = new DomainUserFile
            {
                FileId = id,
                UserId = UserId
            };

            await _userFileRepository.DeleteAsync(userFile);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Deletes file by identifier.
        /// </summary>
        /// <returns>Operation status code.</returns>
        public HttpResponseMessage Delete()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
        }
    }
}