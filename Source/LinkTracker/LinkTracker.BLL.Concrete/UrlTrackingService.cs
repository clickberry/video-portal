// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using LinkTracker.BLL.Infrastructure;
using LinkTracker.DAL;
using LinkTracker.DAL.Entities;
using LinkTracker.Domain;
using LinkTracker.Mappings;
using Portal.BLL.Infrastructure;
using Portal.BLL.Subscriptions;
using Portal.Domain.SubscriptionContext;
using Portal.Exceptions.CRUD;

namespace LinkTracker.BLL.Concrete
{
    public class UrlTrackingService : IUrlTrackingService
    {
        private readonly IBalanceService _balanceService;
        private readonly ICompanyService _companyService;
        private readonly IMappingEngine _mappingEngine;
        private readonly IProjectUriProvider _projectUriProvider;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ITrackingUrlRepository _trackingUrlRepository;
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlTrackingService(ISubscriptionService subscriptionService,
            ITrackingUrlRepository trackingUrlRepository,
            IBalanceService balanceService,
            IUrlShortenerService urlShortenerService,
            IProjectUriProvider projectUriProvider,
            IMappingEngine mappingEngine,
            ICompanyService companyService)
        {
            _subscriptionService = subscriptionService;
            _trackingUrlRepository = trackingUrlRepository;
            _balanceService = balanceService;
            _urlShortenerService = urlShortenerService;
            _projectUriProvider = projectUriProvider;
            _mappingEngine = mappingEngine;
            _companyService = companyService;
        }


        public async Task<DomainTrackingUrl> AddAsync(DomainTrackingUrl trackingUrl)
        {
            // Get subscription
            CompanySubscription subscription = await _subscriptionService.GetAsync(trackingUrl.SubscriptionId);

            // Check redirect url
            if (trackingUrl.RedirectUrl == null)
            {
                throw new ArgumentException("RedirectUrl required");
            }

            if (!string.Equals(trackingUrl.RedirectUrl.Host, subscription.SiteHostname, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BadRequestException(
                    string.Format("Client subscription {0} allows only URLs starting with {1}: {2}",
                        subscription.Id, subscription.SiteHostname, trackingUrl.RedirectUrl));
            }

            // Add tracking url
            TrackingUrlEntity entity = _mappingEngine.Map<DomainTrackingUrl, TrackingUrlEntity>(trackingUrl);
            entity = await _trackingUrlRepository.AddAsync(entity);

            DomainTrackingUrl result = _mappingEngine.Map<TrackingUrlEntity, DomainTrackingUrl>(entity);
            result.Key = _urlShortenerService.Encode(Int64.Parse(result.Id));

            return result;
        }

        public async Task<UrlTrackingResult> TrackAsync(DomainTrackingUrl trackingUrl)
        {
            if (string.IsNullOrEmpty(trackingUrl.Id))
            {
                trackingUrl.Id = _urlShortenerService.Decode(trackingUrl.Key).ToString(CultureInfo.InvariantCulture);
            }

            TrackingUrlEntity entity = _mappingEngine.Map<DomainTrackingUrl, TrackingUrlEntity>(trackingUrl);
            entity = await _trackingUrlRepository.GetAsync(entity);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("Could not find tracking url with id {0} and key {1}",
                    trackingUrl.Id, trackingUrl.Key));
            }

            // Build portal url link
            string projectUrl = _projectUriProvider.GetUri(entity.ProjectId);

            // We should keep requested schema in redirect link
            var projectUri = new UriBuilder(projectUrl)
            {
                Scheme = trackingUrl.RequestUri.Scheme,
                Port = trackingUrl.RequestUri.Port
            };

            // Default redirect to Portal
            var result = new UrlTrackingResult
            {
                ProjectId = entity.ProjectId,
                SubscriptionId = entity.SubscriptionId,
                IsAccountable = false,
                Redirect = projectUri.Uri
            };

            DomainCompany company;
            CompanySubscription subscription;
            try
            {
                company = await _companyService.FindBySubscriptionAsync(entity.SubscriptionId);
                subscription = await _subscriptionService.GetAsync(entity.SubscriptionId);
            }
            catch (ForbiddenException)
            {
                // blocked company or subscription - portal redirect
                return result;
            }
            catch (NotFoundException)
            {
                // deleted or unexisting company or subscription - portal redirect
                return result;
            }


            // Checking custom subscription type
            if (subscription.Type == SubscriptionType.Pro || subscription.Type == SubscriptionType.Custom)
            {
                // always redirect to client site
                result.Redirect = new Uri(entity.RedirectUrl);
                result.IsAccountable = true;
            }
            else if (subscription.Type == SubscriptionType.Basic)
            {
                // checking company balance
                if (subscription.IsManuallyEnabled || subscription.HasTrialClicks || (await _balanceService.GetBalanceAsync(company.Id)) > 0)
                {
                    result.Redirect = new Uri(entity.RedirectUrl);
                    result.IsAccountable = true;
                }
            }

            return result;
        }
    }
}