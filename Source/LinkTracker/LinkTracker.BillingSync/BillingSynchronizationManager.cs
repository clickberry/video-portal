// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LinkTracker.Configuration;
using Newtonsoft.Json.Linq;
using Portal.BLL.Subscriptions;
using Portal.Domain;
using Portal.Domain.SubscriptionContext;

namespace LinkTracker.BillingSync
{
    public class BillingSynchronizationManager
    {
        private readonly IBalanceService _balanceService;
        private readonly ICompanyService _companyService;
        private readonly IBillingSyncConfigurationProvider _config;
        private readonly IUrlTrackingStatService _statService;
        private readonly ISubscriptionService _subscriptionService;

        public BillingSynchronizationManager(IBillingSyncConfigurationProvider config,
            IUrlTrackingStatService statService,
            ICompanyService companyService,
            ISubscriptionService subscriptionService,
            IBalanceService balanceService)
        {
            _config = config;
            _statService = statService;
            _companyService = companyService;
            _subscriptionService = subscriptionService;
            _balanceService = balanceService;
        }

        public async Task SyncAsync(DateTime? syncDate = null)
        {
            try
            {
                JObject subscriptionPlanInfo = _config.SubscriptionPlans;

                IEnumerable<DomainCompany> companies = await _companyService.ListAsync();
                List<Task> syncTasks = companies.Where(c => c.State == ResourceState.Available).Select(c => SyncCompanyAsync(c, subscriptionPlanInfo, syncDate)).ToList();

                await Task.WhenAll(syncTasks);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to sync with billing system: {0}", e);
            }
        }

        private async Task SyncCompanyAsync(DomainCompany company, JObject subscriptionPlans, DateTime? syncDate = null)
        {
            syncDate = syncDate ?? DateTime.UtcNow.AddMinutes(-1); // just to be sure that all transactions were committed
            List<CompanySubscription> subscriptions = company.Subscriptions.Where(s => s.State == ResourceState.Available).ToList();
            List<Task> subscriptionTasks = subscriptions.Select(s => SyncSubscriptionAsync(company.Id, s, syncDate.Value, subscriptionPlans)).ToList();

            await Task.WhenAll(subscriptionTasks);
        }

        private async Task SyncSubscriptionAsync(string companyId, CompanySubscription subscription, DateTime syncDate, JObject subscriptionPlans)
        {
            if (subscription.Type == SubscriptionType.Free)
            {
                // nothing to sync
                return;
            }

            JToken planInfo = subscriptionPlans[((int)subscription.Type).ToString(CultureInfo.InvariantCulture)];

            // Check invoice period
            DateTime lastSyncDate = subscription.LastSyncDate ?? subscription.Created;
            long syncPeriodMs = Int64.Parse((string)planInfo["syncPeriodMs"]);

            if (syncPeriodMs <= 0 || syncDate.Subtract(lastSyncDate).TotalMilliseconds < syncPeriodMs)
            {
                // its too early for invoice
                return;
            }

            // Calculate clicks for sync period
            long periodClicks = await _statService.GetTotalAsync(subscription.Id, lastSyncDate, syncDate);

            // Calculate clicks for cycle
            DateTime lastCycleDate = subscription.LastCycleDate ?? subscription.Created;
            long cycleClicks = await _statService.GetTotalAsync(subscription.Id, lastCycleDate, lastSyncDate);

            JToken clickRatesInfo = planInfo["clickRates"];
            var charges = new List<Tuple<decimal, long, long, long?>>();
            long clicks = periodClicks;
            decimal lastRate = 0;
            long lastCount = 0;
            long clicksMargin = cycleClicks;
            foreach (JToken clickRate in clickRatesInfo)
            {
                var r = clickRate as JProperty;
                if (r == null)
                {
                    continue;
                }

                long count = Int64.Parse(r.Name);
                decimal rate = Decimal.Parse((string)r.Value);
                if (count <= 0)
                {
                    // skip free/initial rate range
                    lastRate = rate;
                    continue;
                }

                if (clicksMargin >= count)
                {
                    // skip last bill
                    lastRate = rate;
                    lastCount = count;
                    continue;
                }

                // calculate charge for range of clicks
                long lastRange = Math.Max(clicksMargin, lastCount);
                long rangeClicks = lastRange + clicks < count ? clicks : count - lastRange;

                // reseting margin
                clicksMargin = 0;

                // creating charge
                if (rangeClicks > 0)
                {
                    charges.Add(new Tuple<decimal, long, long, long?>(lastRate, rangeClicks, lastCount, count));
                }


                // going to next range
                lastRate = rate;
                lastCount = count;
                clicks -= rangeClicks;
                if (clicks == 0)
                {
                    break;
                }
            }

            if (clicks > 0)
            {
                // using last specified rate for exceeded clicks
                charges.Add(new Tuple<decimal, long, long, long?>(lastRate, clicks, lastCount, null));
            }


            // Synchronizing
            var chargeRecords = new List<DomainBalanceHistory>();
            try
            {
                if (charges.Count > 0)
                {
                    if (charges.Sum(c => c.Item1) > 0)
                    {
                        // trial clicks ended
                        await _subscriptionService.UpdateHasTrialClicksAsync(subscription.Id, false);
                    }
                    else
                    {
                        // trial clicks
                        await _subscriptionService.UpdateHasTrialClicksAsync(subscription.Id, true);
                    }

                    foreach (var charge in charges)
                    {
                        decimal rate = charge.Item1;
                        long count = charge.Item2;
                        long lowCount = charge.Item3;
                        long? highCount = charge.Item4;
                        string packageName = highCount.HasValue ? string.Format("{0}-{1}", lowCount, highCount) : string.Format("{0}+", lowCount);
                        var balanceRecord = new DomainBalanceHistory
                        {
                            CompanyId = companyId,
                            Amount = -(rate*count),
                            Description =
                                string.Format(_config.BillingInvoiceItemDescriptionTemplate, count, subscription.SiteName, subscription.Id, subscription.Type, rate/100, packageName, lastSyncDate,
                                    syncDate)
                        };

                        await _balanceService.AddAsync(balanceRecord);
                        chargeRecords.Add(balanceRecord);
                    }
                }

                // cycle completed
                long cyclePeriodMs = Int64.Parse((string)planInfo["cyclePeriodMs"]);
                if (cyclePeriodMs > 0 && syncDate.Subtract(lastCycleDate).TotalMilliseconds >= cyclePeriodMs)
                {
                    // updating last cycle date if cycle completed
                    await _subscriptionService.UpdateLastCycleDateAsync(subscription.Id, syncDate);

                    // enabling trial clicks
                    await _subscriptionService.UpdateHasTrialClicksAsync(subscription.Id, true);
                }

                // updating last sync date
                await _subscriptionService.UpdateLastSyncDateAsync(subscription.Id, syncDate);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to sync balance for subscription {0} of company {1}: {2}", companyId, subscription.Id, e);

                // trying rollback
                try
                {
                    // rollback balance
                    if (chargeRecords.Count > 0)
                    {
                        foreach (DomainBalanceHistory record in chargeRecords)
                        {
                            _balanceService.DeleteAsync(record.Id).Wait();
                        }
                    }

                    // rollback subscription state
                    _subscriptionService.UpdateLastCycleDateAsync(subscription.Id, lastCycleDate).Wait();
                    _subscriptionService.UpdateHasTrialClicksAsync(subscription.Id, subscription.HasTrialClicks).Wait();
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Failed to roll back subscription {0} state for company {1}. Subscription data is corrupted: {2}", subscription.Id, companyId, ex);
                }
            }
        }
    }
}