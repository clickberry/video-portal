// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web.Http;
using System.Web.Http.Hosting;
using Portal.Api.Infrastructure.RequestProcessing;

namespace Portal.Api
{
    public static class TransferModeConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.Services.Replace(typeof (IHostBufferPolicySelector), new CustomWebHostBufferPolicySelector());
        }
    }
}