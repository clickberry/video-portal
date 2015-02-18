// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Portal.Api.Models;
using Portal.BLL.Billing;
using Portal.Resources.Api;

namespace Portal.Api.Controllers.Stripe
{
    [Route("stripe/webhooks")]
    public class StripeWebhooksController : ApiControllerBase
    {
        private readonly IBillingEventHandler _billingEventHandler;

        public StripeWebhooksController(IBillingEventHandler billingEventHandler)
        {
            _billingEventHandler = billingEventHandler;
        }

        //
        // POST api/stripe/webhooks

        public async Task<HttpResponseMessage> Post(StripeWebhookEvent stripeEvent)
        {
            if (string.IsNullOrEmpty(stripeEvent.Id))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessages.BadRequest);
            }

            await _billingEventHandler.HandleEventAsync(stripeEvent.Id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}