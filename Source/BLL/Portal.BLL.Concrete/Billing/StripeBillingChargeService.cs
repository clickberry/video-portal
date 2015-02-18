// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.BLL.Billing;
using Portal.Domain.BillingContext;
using Portal.Exceptions.Billing;
using Portal.Mappers;
using Stripe;

namespace Portal.BLL.Concrete.Billing
{
    public class StripeBillingChargeService : IBillingChargeService
    {
        private readonly IMapper _mapper;
        private readonly StripeChargeService _service;

        public StripeBillingChargeService(string apiKey, IMapper mapper)
        {
            _service = new StripeChargeService(apiKey);
            _mapper = mapper;
        }

        public Task<DomainCharge> GetAsync(DomainCharge charge)
        {
            return Task.Run(() =>
            {
                try
                {
                    StripeCharge c = _service.Get(charge.Id);
                    return Task.FromResult(_mapper.Map<StripeCharge, DomainCharge>(c));
                }
                catch (StripeException e)
                {
                    throw new BillingException(string.Format("Failed to get charge by id {0}: {1}", charge.Id, e));
                }
            });
        }

        public Task<DomainCharge> AddAsync(DomainChargeCreateOptions options)
        {
            return Task.Run(() =>
            {
                try
                {
                    StripeChargeCreateOptions createOptions =
                        _mapper.Map<DomainChargeCreateOptions, StripeChargeCreateOptions>(options);

                    StripeCharge charge = _service.Create(createOptions);
                    return Task.FromResult(_mapper.Map<StripeCharge, DomainCharge>(charge));
                }
                catch (StripeException e)
                {
                    throw new BillingException(string.Format("Failed to create charge for customer {0}: {1}", options.CustomerId, e));
                }
            });
        }

        public Task<IEnumerable<DomainCharge>> ListAsync(DomainCustomer customer, int count)
        {
            return Task.Run(() =>
            {
                try
                {
                    IEnumerable<StripeCharge> charges = _service.List(count, customer.Id);
                    return Task.FromResult(_mapper.Map<IEnumerable<StripeCharge>, IEnumerable<DomainCharge>>(charges));
                }
                catch (StripeException e)
                {
                    throw new BillingException(string.Format("Failed to list charges for customer {0}: {1}", customer.Id, e));
                }
            });
        }
    }
}