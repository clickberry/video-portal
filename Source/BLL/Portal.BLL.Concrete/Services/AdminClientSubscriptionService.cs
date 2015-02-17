// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.Domain.Admin;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public class AdminClientSubscriptionService : IAdminClientSubscriptionService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public AdminClientSubscriptionService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DomainAdminClientSubscription>> GetSubscriptionsAsync(string clientId)
        {
            CompanyEntity company = await _companyRepository.GetAsync(clientId);
            if (company == null)
            {
                throw new NotFoundException();
            }

            return company.Subscriptions.Select(s => _mapper.Map<SubscriptionEntity, DomainAdminClientSubscription>(s));
        }

        public async Task<DomainAdminClientSubscription> EditSubscriptionAsync(string clientId, DomainAdminClientSubscription domainSubscription)
        {
            CompanyEntity company = await _companyRepository.GetAsync(clientId);
            if (company == null)
            {
                throw new NotFoundException();
            }

            SubscriptionEntity subscription = company.Subscriptions.FirstOrDefault(s => s.Id == domainSubscription.Id);
            if (subscription == null)
            {
                throw new NotFoundException();
            }

            subscription.State = (int)domainSubscription.State;
            subscription.IsManuallyEnabled = domainSubscription.IsManuallyEnabled;

            await _companyRepository.UpdateAsync(company);

            return _mapper.Map<SubscriptionEntity, DomainAdminClientSubscription>(subscription);
        }
    }
}