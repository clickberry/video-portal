// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Asp.Infrastructure.MessageHandlers;
using Newtonsoft.Json.Serialization;
using Portal.Api.Infrastructure.MediaFormatters;
using Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters;

namespace Portal.Api
{
    /// <summary>
    ///     Web API configuration.
    /// </summary>
    public class WebApiConfig
    {
        /// <summary>
        ///     Configures http message handlers.
        /// </summary>
        /// <param name="configuration"></param>
        public static void Configure(HttpConfiguration configuration)
        {
            // Attribute routes
            configuration.MapHttpAttributeRoutes();

            // Message handlers
            configuration.MessageHandlers.Add(new MethodOverrideHandler());
            configuration.MessageHandlers.Add(new UserAgentOverrideHandler());
            configuration.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());

            // Media type formatters
            configuration.Formatters.Clear();
            configuration.Formatters.Add(new JsonMediaTypeFormatter { SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() } });
            configuration.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
            configuration.Formatters.Add(new JQueryMvcFormUrlEncodedFormatter());
            configuration.Formatters.Add(new MultipartFormFormatter());
            configuration.Formatters.Add(new AnyOtherTypeFormFormatter());
            configuration.Formatters.Add(new ProjectForAdminCsvFormatter());
            configuration.Formatters.Add(new UserForAdminCsvFormatter());
            configuration.Formatters.Add(new ClientForAdminCsvFormatter());

            // CORS support
            configuration.EnableQuerySupport();
            configuration.EnableCors();
        }
    }
}