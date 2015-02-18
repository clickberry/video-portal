// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Subscriptions;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.Domain;
using Portal.Domain.SubscriptionContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public SubscriptionService(ICompanyRepository companyRepository,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<CompanySubscription> GetAsync(string subscriptionid)
        {
            CompanyEntity company = await _companyRepository.FindBySubscriptionAsync(subscriptionid);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find subscription by id {0}", subscriptionid));
            }

            CheckCompanyAccess(company);

            SubscriptionEntity subscription = company.Subscriptions.First(s => s.Id == subscriptionid);
            if (subscription.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Subscription {0} is blocked", subscriptionid));
            }
            if (subscription.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Subscription {0} is deleted", subscriptionid));
            }

            return _mapper.Map<SubscriptionEntity, CompanySubscription>(subscription);
        }

        public async Task<CompanySubscription> AddAsync(string userId, CompanySubscriptionCreateOptions options)
        {
            CompanySubscription subscription = _mapper.Map<CompanySubscriptionCreateOptions, CompanySubscription>(options);

            // Searching for company
            CompanyEntity company = await _companyRepository.FindByUserAsync(userId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find company by user '{0}'", userId));
            }

            CheckCompanyAccess(company);

            // Setting defaults
            subscription.Id = Guid.NewGuid().ToString();
            subscription.Created = DateTime.UtcNow;
            subscription.HasTrialClicks = true;

            SubscriptionEntity subscriptionEntity = _mapper.Map<CompanySubscription, SubscriptionEntity>(subscription);

            // Updating company
            company.Subscriptions.Add(subscriptionEntity);
            await _companyRepository.UpdateAsync(company);

            return subscription;
        }

        public async Task<CompanySubscription> UpdateAsync(string userId, string subscriptionId, CompanySubscriptionUpdateOptions options)
        {
            // Get by subscription id
            CompanyEntity company = await _companyRepository.FindBySubscriptionAsync(subscriptionId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find company by subscription id '{0}'", subscriptionId));
            }

            CheckCompanyAccess(company);

            // Check user permissions
            if (!company.Users.Contains(userId))
            {
                throw new ForbiddenException();
            }

            // Get subscription
            SubscriptionEntity subscription = company.Subscriptions.First(s => s.Id == subscriptionId);
            if (subscription.State == (int)ResourceState.Deleted)
            {
                throw new ForbiddenException(string.Format("Subscription '{0}' is deleted", subscriptionId));
            }

            subscription.SiteName = options.SiteName;
            subscription.GoogleAnalyticsId = options.GoogleAnalyticsId;

            await _companyRepository.UpdateAsync(company);

            return _mapper.Map<SubscriptionEntity, CompanySubscription>(subscription);
        }

        public async Task<CompanySubscription> UpdateLastSyncDateAsync(string subscriptionId, DateTime date)
        {
            CompanyEntity company = await _companyRepository.FindBySubscriptionAsync(subscriptionId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find subscription by id {0}", subscriptionId));
            }

            CheckCompanyAccess(company);

            SubscriptionEntity subscription = company.Subscriptions.First(s => s.Id == subscriptionId);
            if (subscription.State == (int)ResourceState.Deleted)
            {
                throw new ForbiddenException(string.Format("Subscription {0} is deleted", subscriptionId));
            }

            await _companyRepository.SetSubscriptionLastSyncDateAsync(subscriptionId, date);

            return await GetAsync(subscriptionId);
        }

        public async Task<CompanySubscription> UpdateLastCycleDateAsync(string subscriptionId, DateTime date)
        {
            CompanyEntity company = await _companyRepository.FindBySubscriptionAsync(subscriptionId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find subscription by id {0}", subscriptionId));
            }

            CheckCompanyAccess(company);

            SubscriptionEntity subscription = company.Subscriptions.First(s => s.Id == subscriptionId);
            if (subscription.State == (int)ResourceState.Deleted)
            {
                throw new ForbiddenException(string.Format("Subscription {0} is deleted", subscriptionId));
            }

            await _companyRepository.SetSubscriptionLastCycleDateAsync(subscriptionId, date);

            return await GetAsync(subscriptionId);
        }

        public async Task<CompanySubscription> UpdateHasTrialClicksAsync(string subscriptionId, bool value)
        {
            CompanyEntity company = await _companyRepository.FindBySubscriptionAsync(subscriptionId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find subscription by id {0}", subscriptionId));
            }

            CheckCompanyAccess(company);

            SubscriptionEntity subscription = company.Subscriptions.First(s => s.Id == subscriptionId);
            if (subscription.State == (int)ResourceState.Deleted)
            {
                throw new ForbiddenException(string.Format("Subscription {0} is deleted", subscriptionId));
            }

            await _companyRepository.SetSubscriptionHasTrialClicksAsync(subscriptionId, value);

            return await GetAsync(subscriptionId);
        }

        public async Task DeleteAsync(string userId, string subscriptionId)
        {
            // Get by subscription id
            CompanyEntity company = await _companyRepository.FindBySubscriptionAsync(subscriptionId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find subscription by id '{0}'", subscriptionId));
            }

            CheckCompanyAccess(company);

            // Check user permissions
            if (!company.Users.Contains(userId))
            {
                throw new ForbiddenException();
            }

            // Get subscription
            SubscriptionEntity subscription = company.Subscriptions.First(s => s.Id == subscriptionId);
            if (subscription.State == (int)ResourceState.Deleted)
            {
                throw new ForbiddenException(string.Format("Subscription '{0}' is deleted", subscriptionId));
            }

            // Deleting subscription
            subscription.State = (int)ResourceState.Deleted;
            await _companyRepository.UpdateAsync(company);
        }

        private static void CheckCompanyAccess(CompanyEntity company)
        {
            if (company.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Company {0} is blocked", company.Id));
            }

            if (company.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Company {0} is deleted", company.Id));
            }
        }
    }
}