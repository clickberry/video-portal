// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkTracker.BillingSync;
using LinkTracker.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using Portal.BLL.Subscriptions;
using Portal.DAL.Entities.Table;
using Portal.Domain;
using Portal.Domain.SubscriptionContext;

namespace LinkTracker.Tests.BillingSync
{
    [TestClass]
    public class BillingSynchronizationManagerTest
    {
        private readonly Mock<IBalanceService> _balanceService = new Mock<IBalanceService>();
        private readonly Mock<ICompanyService> _companyService = new Mock<ICompanyService>();
        private readonly Mock<IBillingSyncConfigurationProvider> _config = new Mock<IBillingSyncConfigurationProvider>();
        private readonly Mock<IUrlTrackingStatService> _statService = new Mock<IUrlTrackingStatService>();
        private readonly Mock<ISubscriptionService> _subscriptionService = new Mock<ISubscriptionService>();
        private BillingSynchronizationManager _syncManager;

        [TestInitialize]
        public void Initialize()
        {
            _syncManager = new BillingSynchronizationManager(_config.Object, _statService.Object, _companyService.Object, _subscriptionService.Object, _balanceService.Object);
        }

        // https://clickberry.atlassian.net/browse/PN-1255
        [TestMethod]
        public void TestInaccauntableClicks()
        {
            // 1. Arrange

            // config
            _config.Setup(x => x.SubscriptionPlans)
                .Returns(
                    () =>
                        JObject.Parse(
                            "{ 0: { syncPeriodMs: 0, cyclePeriodMs: 0, clickRates: { 0: 0 } }, 1: { syncPeriodMs: 60000, cyclePeriodMs: 2592000000, clickRates: { 0: 0, 20: 10 } }, 2: { syncPeriodMs: 2592000000, cyclePeriodMs: 2592000000, clickRates: { 0: 0, 100000: 0.05, 1000000: 0.01 } }, 3: { syncPeriodMs: 2592000000, cyclePeriodMs: 2592000000, clickRates: { 0: 0, 100000: 0.05, 1000000: 0.01 } } }"));
            _config.Setup(x => x.BillingInvoiceItemDescriptionTemplate).Returns(() => "{0}");

            // stats service
            var stas = new List<TrackingStatEntity>
            {
                new TrackingStatEntity { Date = new DateTime(2014, 6, 4, 14, 28, 35, 596) },
                new TrackingStatEntity { Date = new DateTime(2014, 6, 4, 14, 28, 36, 995) },
                new TrackingStatEntity { Date = new DateTime(2014, 6, 4, 14, 28, 41, 889) },
                new TrackingStatEntity { Date = new DateTime(2014, 6, 4, 14, 28, 44, 249) },
                new TrackingStatEntity { Date = new DateTime(2014, 6, 4, 14, 28, 48, 335) }
            };
            _statService.Setup(x => x.GetTotalAsync(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns<string, DateTime?, DateTime?>((i, f, t) =>
            {
                IQueryable<TrackingStatEntity> r = stas.AsQueryable();
                if (f.HasValue)
                {
                    r = r.Where(s => s.Date >= f.Value);
                }

                if (t.HasValue)
                {
                    r = r.Where(s => s.Date < t.Value);
                }

                return Task.FromResult(r.LongCount());
            });

            // company service
            var company = new DomainCompany
            {
                State = ResourceState.Available,
                Subscriptions = new List<CompanySubscription>
                {
                    new CompanySubscription
                    {
                        State = ResourceState.Available,
                        Type = SubscriptionType.Basic,
                        Created = new DateTime(2014, 6, 4, 14, 25, 14, 133),
                        LastSyncDate = null,
                        LastCycleDate = new DateTime(2014, 6, 4, 14, 27, 37, 939),
                        HasTrialClicks = false
                    }
                }
            };
            var companies = new List<DomainCompany> { company };
            _companyService.Setup(x => x.ListAsync()).Returns(() => Task.FromResult(companies.AsEnumerable()));

            // balance service
            var balance = new DomainBalanceHistory();
            _balanceService.Setup(x => x.AddAsync(It.IsAny<DomainBalanceHistory>())).Returns<DomainBalanceHistory>(b =>
            {
                balance = b;
                return Task.FromResult(balance);
            });


            // 2. Act
            var syncDate = new DateTime(2014, 6, 4, 14, 28, 37, 939);
            _syncManager.SyncAsync(syncDate).Wait();


            // 3. Assert
            long actualCount = Int64.Parse(balance.Description);
            Assert.AreEqual(2, actualCount);
        }
    }
}