// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.BLL.Billing;
using Portal.Domain.BillingContext;
using Portal.Exceptions.Billing;
using Portal.Mappers;
using Stripe;

namespace Portal.BLL.Concrete.Billing
{
    public class StripeBillingCustomerService : IBillingCustomerService
    {
        private readonly IMapper _mapper;
        private readonly StripeCustomerService _service;

        public StripeBillingCustomerService(string apiKey, IMapper mapper)
        {
            _service = new StripeCustomerService(apiKey);
            _mapper = mapper;
        }


        public Task<DomainCustomer> GetAsync(DomainCustomer customer)
        {
            return Task.Run(() =>
            {
                try
                {
                    StripeCustomer c = _service.Get(customer.Id);
                    return Task.FromResult(_mapper.Map<StripeCustomer, DomainCustomer>(c));
                }
                catch (StripeException e)
                {
                    throw new BillingException(string.Format("Failed to get customer {0}: {1}", customer.Id, e));
                }
            });
        }

        public Task<DomainCustomer> AddAsync(DomainCustomerCreateOptions options)
        {
            return Task.Run(() =>
            {
                try
                {
                    StripeCustomerCreateOptions createOptions =
                        _mapper.Map<DomainCustomerCreateOptions, StripeCustomerCreateOptions>(options);

                    StripeCustomer customer = _service.Create(createOptions);
                    return Task.FromResult(_mapper.Map<StripeCustomer, DomainCustomer>(customer));
                }
                catch (StripeException e)
                {
                    throw new BillingException(string.Format("Failed to create customer {0}: {1}", options.Email, e));
                }
            });
        }
    }
}