// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Subscriptions;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.Domain.SubscriptionContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Subscriptions
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceHistoryRepository _balanceHistoryRepository;
        private readonly IMapper _mapper;

        public BalanceService(IBalanceHistoryRepository balanceHistoryRepository,
            IMapper mapper)
        {
            _balanceHistoryRepository = balanceHistoryRepository;
            _mapper = mapper;
        }

        public async Task<DomainBalanceHistory> AddAsync(DomainBalanceHistory record)
        {
            BalanceHistoryEntity entity = _mapper.Map<DomainBalanceHistory, BalanceHistoryEntity>(record);
            entity.Date = DateTime.UtcNow;

            entity = await _balanceHistoryRepository.AddAsync(entity);
            DomainBalanceHistory result = _mapper.Map<BalanceHistoryEntity, DomainBalanceHistory>(entity);

            return result;
        }

        public async Task<IEnumerable<DomainBalanceHistory>> QueryHistoryAsync(string companyId)
        {
            IEnumerable<BalanceHistoryEntity> balanceHistory =
                await _balanceHistoryRepository.FindByCompanyAsync(companyId);
            return
                _mapper.Map<IEnumerable<BalanceHistoryEntity>, IEnumerable<DomainBalanceHistory>>(balanceHistory);
        }

        public async Task<IEnumerable<DomainBalanceHistory>> QueryPaymentsHistoryAsync(string companyId)
        {
            IEnumerable<BalanceHistoryEntity> balanceHistory =
                await _balanceHistoryRepository.FindByCompanyAsync(companyId);
            return
                _mapper.Map<IEnumerable<BalanceHistoryEntity>, IEnumerable<DomainBalanceHistory>>(
                    balanceHistory.Where(b => b.Amount > 0));
        }

        public async Task<IEnumerable<DomainBalanceHistory>> QueryChargesHistoryAsync(string companyId)
        {
            IEnumerable<BalanceHistoryEntity> balanceHistory =
                await _balanceHistoryRepository.FindByCompanyAsync(companyId);
            return
                _mapper.Map<IEnumerable<BalanceHistoryEntity>, IEnumerable<DomainBalanceHistory>>(
                    balanceHistory.Where(b => b.Amount < 0));
        }

        public async Task<decimal> GetBalanceAsync(string companyId)
        {
            IEnumerable<BalanceHistoryEntity> balanceHistory =
                await _balanceHistoryRepository.FindByCompanyAsync(companyId);
            return balanceHistory.Sum(b => b.Amount);
        }

        public async Task DeleteAsync(string id)
        {
            BalanceHistoryEntity company = await _balanceHistoryRepository.GetAsync(id);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find balance history redord by id {0}", id));
            }

            await _balanceHistoryRepository.DeleteAsync(id);
        }
    }
}