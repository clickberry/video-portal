// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.BLL.Billing;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.Domain.BillingContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Billing
{
    public class BillingEventLogService : IBillingEventLogService
    {
        private readonly IBillingEventRepository _billingEventRepository;
        private readonly IMapper _mapper;

        public BillingEventLogService(
            IBillingEventRepository billingEventRepository,
            IMapper mapper)
        {
            _billingEventRepository = billingEventRepository;
            _mapper = mapper;
        }

        public async Task<bool> ExistsAsync(DomainEvent e)
        {
            BillingEventEntity entity = await _billingEventRepository.GetAsync(e.Id);
            return entity != null;
        }

        public async Task<DomainEvent> AddAsync(DomainEvent e)
        {
            BillingEventEntity entity = _mapper.Map<DomainEvent, BillingEventEntity>(e);

            entity = await _billingEventRepository.AddAsync(entity);
            DomainEvent result = _mapper.Map<BillingEventEntity, DomainEvent>(entity);

            return result;
        }

        public Task DeleteAsync(DomainEvent e)
        {
            return _billingEventRepository.DeleteAsync(e.Id);
        }
    }
}