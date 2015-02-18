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
    public class StripeBillingCardService : IBillingCardService
    {
        private readonly IMapper _mapper;
        private readonly StripeCardService _service;

        public StripeBillingCardService(string apiKey, IMapper mapper)
        {
            _service = new StripeCardService(apiKey);
            _mapper = mapper;
        }


        public Task<DomainCard> AddAsync(DomainCardCreateOptions options)
        {
            return Task.Run(() =>
            {
                try
                {
                    StripeCardCreateOptions createOptions =
                        _mapper.Map<DomainCardCreateOptions, StripeCardCreateOptions>(options);

                    StripeCard card = _service.Create(options.CustomerId, createOptions);
                    return Task.FromResult(_mapper.Map<StripeCard, DomainCard>(card));
                }
                catch (StripeException e)
                {
                    throw new BillingException(string.Format("Failed to register card in billing system for customer {0}: {1}", options.CustomerId, e));
                }
            });
        }
    }
}