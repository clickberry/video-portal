// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using AutoMapper;
using Portal.Api.Models;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters;
using Portal.BLL.Services;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.DTO.Admin;
using Portal.Mappers;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Admin
{
    [ValidationHttp]
    [Route("admin/users/{id?}")]
    public class AdminUsersController : ApiControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly ICsvPublishServiceFactory _csvPublishServiceFactory;
        private readonly IMapper _mapper;

        public AdminUsersController(IAdminUserService adminUserService,
            IMapper mapper,
            ICsvPublishServiceFactory csvPublishServiceFactory)
        {
            _adminUserService = adminUserService;
            _mapper = mapper;
            _csvPublishServiceFactory = csvPublishServiceFactory;
        }

        //
        // GET /api/admin/users

        [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
        [ODataValidation]
        [ODataValidationExceptionFilter]
        public async Task<HttpResponseMessage> Get(ODataQueryOptions<AdminUser> options)
        {
            var validationSettings = new ODataValidationSettings
            {
                MaxTop = 100,
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators =
                    AllowedLogicalOperators.Equal | AllowedLogicalOperators.LessThan | AllowedLogicalOperators.LessThanOrEqual | AllowedLogicalOperators.GreaterThan |
                    AllowedLogicalOperators.GreaterThanOrEqual | AllowedLogicalOperators.And,
                AllowedQueryOptions =
                    AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top | AllowedQueryOptions.Format | AllowedQueryOptions.InlineCount
            };

            validationSettings.AllowedOrderByProperties.Add("UserName");
            validationSettings.AllowedOrderByProperties.Add("Created");
            validationSettings.AllowedOrderByProperties.Add("ProductType");

            try
            {
                options.Validate(validationSettings);
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions);
            }


            // Determine media formatter
            IContentNegotiator negotiator = Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult negotiationResult = negotiator.Negotiate(typeof (AdminUser), Request, Configuration.Formatters);
            if (negotiationResult == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ResponseMessages.NotAcceptable);
            }


            // Retrieving users
            DataResult<Task<DomainUserForAdmin>> result;
            IEnumerable<Task<AdminUser>> userTasks;
            DataQueryOptions filter;

            // Parsing filter parameters
            try
            {
                filter = _mapper.Map<ODataQueryOptions, DataQueryOptions>(options);
            }
            catch (AutoMapperMappingException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.InvalidQueryOptions);
            }


            // CSV
            if (negotiationResult.Formatter is UserForAdminCsvFormatter)
            {
                // get csv service
                ICsvPublishService csvService = _csvPublishServiceFactory.GetService<AdminUser>();

                // send the link to the future csv file
                string fileName = string.Format("{0}.csv", Guid.NewGuid());
                Uri fileUri = csvService.GetAccessCsvUri(fileName);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Accepted);
                response.Headers.Location = fileUri;

                // disable paging
                filter.Take = null;

                // start csv publishing in background thread
                Task.Run(() => csvService.PublishAsync(filter, fileName, new CancellationTokenSource())).NoWarning();

                return response;
            }


            // Filtering users
            try
            {
                result = _adminUserService.GetAsyncSequence(filter);
                userTasks = result.Results.Select(t => t.ContinueWith(u => _mapper.Map<DomainUserForAdmin, AdminUser>(u.Result)));
            }
            catch (NotSupportedException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
            }


            // Loading and shaping all users
            List<AdminUser> users = (await Task.WhenAll(userTasks)).ToList();

            if (filter.Count)
            {
                var pageResult = new PageResult<AdminUser>(users, null, result.Count);
                return Request.CreateResponse(HttpStatusCode.OK, pageResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        //
        // GET /api/admin/users/{id}

        [AuthorizeHttp(Roles = DomainRoles.AllAdministrators)]
        public async Task<AdminUser> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User id required"));
            }

            DomainUserForAdmin user = await _adminUserService.GetAsync(new DomainUserForAdmin { UserId = id });
            AdminUser model = _mapper.Map<DomainUserForAdmin, AdminUser>(user);

            return model;
        }


        //
        // PUT /api/admin/users/{id}
        [CheckAccessHttp]
        [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
        public async Task<HttpResponseMessage> Put(string id, AdminSetUserPasswordModel model)
        {
            await _adminUserService.SetUserPasswordAsync(id, model.Password);

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        //
        // DELETE /api/admin/users/{id}
        [CheckAccessHttp]
        [AuthorizeHttp(Roles = DomainRoles.SuperAdministrator)]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await _adminUserService.DeleteAsync(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}