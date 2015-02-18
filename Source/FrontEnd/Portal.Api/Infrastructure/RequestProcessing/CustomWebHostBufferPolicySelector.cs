// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Web;
using System.Web.Http.WebHost;

namespace Portal.Api.Infrastructure.RequestProcessing
{
    public class CustomWebHostBufferPolicySelector : WebHostBufferPolicySelector
    {
        // Check incoming requests and modify their buffer policy
        public override bool UseBufferedInputStream(object hostContext)
        {
            var contextBase = hostContext as HttpContextBase;

            if (contextBase != null &&
                contextBase.Request.ContentType != null &&
                contextBase.Request.ContentType.Contains("multipart"))
            {
                // we are enabling streamed mode here
                return false;
            }

            // let the default behavior(buffered mode) to handle the scenario
            return base.UseBufferedInputStream(hostContext);
        }
    }
}