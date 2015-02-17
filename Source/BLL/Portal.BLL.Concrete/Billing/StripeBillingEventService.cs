// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.BLL.Billing;
using Portal.Domain.BillingContext;
using Portal.Exceptions.Billing;
using Portal.Mappers;
using Stripe;

namespace Portal.BLL.Concrete.Billing
{
    public class StripeBillingEventService : IBillingEventService
    {
        private readonly IMapper _mapper;
        private readonly StripeEventService _service;

        public StripeBillingEventService(string apiKey, IMapper mapper)
        {
            _service = new StripeEventService(apiKey);
            _mapper = mapper;
        }


        public Task<DomainEvent> GetAsync(DomainEvent e)
        {
            return Task.Run(() =>
            {
                try
                {
                    StripeEvent eventObject = _service.Get(e.Id);
                    return Task.FromResult(_mapper.Map<StripeEvent, DomainEvent>(eventObject));
                }
                catch (StripeException ex)
                {
                    throw new BillingException(string.Format("Failed to get billing event data by id {0}: {1}", e.Id, ex));
                }
            });
        }
    }
}