// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Asp.Infrastructure.Attributes.WebApi;
using LinkTracker.Api.Models;
using LinkTracker.BLL;
using LinkTracker.Configuration;
using LinkTracker.Domain;
using LinkTracker.Infrastructure.Filters;

namespace LinkTracker.Api.Controllers
{
    [ExceptionHandlingWebApi]
    [ValidationHttp]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TrackController : ApiController
    {
        private const string ApiName = "TrackApi";
        private readonly IApiConfigurationProvider _configurationProvider;
        private readonly IUrlTrackingService _service;

        public TrackController(IUrlTrackingService service, IApiConfigurationProvider configurationProvider)
        {
            _service = service;
            _configurationProvider = configurationProvider;
        }


        //
        // GET /{id}

        [Route("{id}", Name = ApiName)]
        public async Task<HttpResponseMessage> Get(string id)
        {
            UrlTrackingResult result = await _service.TrackAsync(new DomainTrackingUrl
            {
                Key = id,
                RequestUri = Request.RequestUri
            });

            // redirecting
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = result.Redirect;

            return response;
        }

        //
        // GET /

        [Route]
        public HttpResponseMessage Get()
        {
            // redirecting to portal front page
            var portalUri = new UriBuilder(_configurationProvider.ProjectBaseUri)
            {
                Path = "/",
                Scheme = Request.RequestUri.Scheme,
                Port = Request.RequestUri.Port
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = portalUri.Uri;

            return response;
        }

        //
        // POST /

        [Route]
        public async Task<HttpResponseMessage> Post(TrackingUrlModel model)
        {
            var trackingUrl = new DomainTrackingUrl
            {
                ProjectId = model.ProjectId,
                SubscriptionId = model.SubscriptionId,
                RedirectUrl = new Uri(model.Url)
            };

            trackingUrl = await _service.AddAsync(trackingUrl);

            string linkUri = Url.Link(ApiName, new { id = trackingUrl.Key });
            if (string.IsNullOrEmpty(linkUri))
            {
                throw new ApplicationException(string.Format("Could not generate url for link: {0}", trackingUrl.Key));
            }

            var uri = new Uri(linkUri);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, uri.ToString());
            response.Headers.Location = uri;

            return response;
        }
    }
}