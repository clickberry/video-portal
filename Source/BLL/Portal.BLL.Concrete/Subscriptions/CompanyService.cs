// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Billing;
using Portal.BLL.Subscriptions;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.Domain;
using Portal.Domain.BillingContext;
using Portal.Domain.SubscriptionContext;
using Portal.Exceptions.Billing;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Subscriptions
{
    public class CompanyService : ICompanyService
    {
        private readonly IBillingCardService _billingCardService;
        private readonly IBillingChargeService _billingChargeService;
        private readonly IBillingCustomerService _billingCustomerService;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyRepository companyRepository,
            IMapper mapper,
            IBillingCardService billingCardService,
            IBillingCustomerService billingCustomerService,
            IBillingChargeService billingChargeService)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _billingCardService = billingCardService;
            _billingCustomerService = billingCustomerService;
            _billingChargeService = billingChargeService;
        }

        public async Task<IEnumerable<DomainCompany>> ListAsync()
        {
            IEnumerable<CompanyEntity> companies = await _companyRepository.GetAllAsync();
            return companies.Select(c => _mapper.Map<CompanyEntity, DomainCompany>(c));
        }

        public async Task<DomainCompany> FindBySubscriptionAsync(string subscriptionId)
        {
            CompanyEntity entity = await _companyRepository.FindBySubscriptionAsync(subscriptionId);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("Could not find company for subscription id {0}", subscriptionId));
            }

            if (entity.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Company {0} is blocked", entity.Id));
            }
            if (entity.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Company {0} is deleted", entity.Id));
            }

            return _mapper.Map<CompanyEntity, DomainCompany>(entity);
        }

        public async Task<DomainCompany> FindByUserAsync(string userId)
        {
            CompanyEntity entity = await _companyRepository.FindByUserAsync(userId);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("Could not find company for user id {0}", userId));
            }

            if (entity.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Company {0} is blocked", entity.Id));
            }
            if (entity.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Company {0} is deleted", entity.Id));
            }

            return _mapper.Map<CompanyEntity, DomainCompany>(entity);
        }

        public async Task<DomainCompany> FindByCustomerAsync(string customerId)
        {
            CompanyEntity entity = await _companyRepository.FindByCustomerAsync(customerId);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("Could not find company for customer id {0}", customerId));
            }

            // non-throwing exception for blocked/deleted companies because this method used privately by Stripe event handler

            return _mapper.Map<CompanyEntity, DomainCompany>(entity);
        }

        public async Task<DomainCompany> AddAsync(DomainCompany company)
        {
            company.Created = DateTime.UtcNow;
            company.Email = company.Email.ToLowerInvariant();

            CompanyEntity entity = _mapper.Map<DomainCompany, CompanyEntity>(company);

            // Creating customer in billing system
            var customerCreateOptions = new DomainCustomerCreateOptions
            {
                Email = entity.Email
            };

            DomainCustomer customer;
            try
            {
                customer = await _billingCustomerService.AddAsync(customerCreateOptions);
            }
            catch (BillingException e)
            {
                throw new BadRequestException(string.Format("Failed to register customer {0}: {1}", entity.Email, e));
            }

            entity.BillingCustomerId = customer.Id;
            entity = await _companyRepository.AddAsync(entity);

            return _mapper.Map<CompanyEntity, DomainCompany>(entity);
        }

        public async Task<DomainCompany> UpdateByUserAsync(string userId, CompanyUpdateOptions update)
        {
            CompanyEntity company = await _companyRepository.FindByUserAsync(userId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find company by user id '{0}'.", userId));
            }

            if (company.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Company {0} is blocked", company.Id));
            }
            if (company.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Company {0} is deleted", company.Id));
            }

            // Patching
            company.Name = update.Name;
            company.NameSort = update.Name == null ? null : update.Name.ToLowerInvariant();
            company.Country = update.Country;
            company.Ein = update.Ein;
            company.Address = update.Address;
            company.ZipCode = update.ZipCode;
            company.Phone = update.Phone;
            company.Email = update.Email;

            company = await _companyRepository.UpdateAsync(company);

            return _mapper.Map<CompanyEntity, DomainCompany>(company);
        }

        public async Task DeleteByUserAsync(string userId)
        {
            CompanyEntity company = await _companyRepository.FindByUserAsync(userId);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find company {0}", userId));
            }

            // Mark as deleted
            company.State = (int)ResourceState.Deleted;
            foreach (SubscriptionEntity subscription in company.Subscriptions)
            {
                subscription.State = (int)ResourceState.Deleted;
            }

            // Save
            await _companyRepository.UpdateAsync(company);
        }

        public async Task ChargeAsync(CompanyChargeOptions charge)
        {
            CompanyEntity company = await _companyRepository.GetAsync(charge.Id);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find company by id {0}", charge.Id));
            }

            if (company.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Company {0} is blocked", company.Id));
            }
            if (company.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Company {0} is deleted", company.Id));
            }

            // Creating customer card
            var cardCreateOptions = new DomainCardCreateOptions
            {
                CustomerId = company.BillingCustomerId,
                TokenId = charge.TokenId
            };

            DomainCard billingCard;
            try
            {
                billingCard = await _billingCardService.AddAsync(cardCreateOptions);
            }
            catch (BillingException e)
            {
                throw new BadRequestException(string.Format("Failed to register card for company {0}: {1}", company.Id, e));
            }


            // Charging card
            var chargeCreateOptions = new DomainChargeCreateOptions
            {
                AmountInCents = charge.AmountInCents,
                CustomerId = company.BillingCustomerId,
                Currency = charge.Currency,
                Description = charge.Description,
                Card = billingCard.Id
            };

            // Balance will be updated automatically after callback (webhook) from billing system
            DomainCharge billingCharge;
            try
            {
                billingCharge = await _billingChargeService.AddAsync(chargeCreateOptions);
            }
            catch (BillingException e)
            {
                throw new BadRequestException(string.Format("Failed to charge billing customer {0} for company {1}: {2}", company.BillingCustomerId, company.Id, e));
            }

            if (!billingCharge.IsPaid)
            {
                // payment failed
                throw new PaymentRequiredException(string.Format("Payment failed for company {0}", company.Id));
            }
        }

        public async Task<DomainCompany> GetAsync(string id)
        {
            CompanyEntity company = await _companyRepository.GetAsync(id);
            if (company == null)
            {
                throw new NotFoundException(string.Format("Could not find company {0}", id));
            }

            if (company.State == (int)ResourceState.Blocked)
            {
                throw new ForbiddenException(string.Format("Company {0} is blocked", id));
            }
            if (company.State == (int)ResourceState.Deleted)
            {
                throw new NotFoundException(string.Format("Company {0} is deleted", id));
            }

            return _mapper.Map<CompanyEntity, DomainCompany>(company);
        }
    }
}