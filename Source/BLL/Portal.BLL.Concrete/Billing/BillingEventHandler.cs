// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Portal.BLL.Billing;
using Portal.BLL.Services;
using Portal.BLL.Subscriptions;
using Portal.Domain.BillingContext;
using Portal.Domain.SubscriptionContext;
using Portal.Resources.Billing;

namespace Portal.BLL.Concrete.Billing
{
    public sealed class BillingEventHandler : IBillingEventHandler
    {
        private readonly IBalanceService _balanceService;
        private readonly IBillingChargeService _billingChargeService;
        private readonly IBillingEventLogService _billingEventLogService;
        private readonly IBillingEventService _billingEventService;
        private readonly ICompanyService _companyService;
        private readonly Dictionary<EventType, Func<DomainEvent, Task>> _eventHandlers;
        private readonly IEmailNotificationService _notificationService;

        public BillingEventHandler(
            IBillingEventService billingEventService,
            IBalanceService balanceService,
            IBillingChargeService billingChargeService,
            IBillingEventLogService billingEventLogService,
            ICompanyService companyService,
            IEmailNotificationService notificationService)
        {
            _billingEventService = billingEventService;
            _balanceService = balanceService;
            _billingChargeService = billingChargeService;
            _billingEventLogService = billingEventLogService;
            _companyService = companyService;
            _notificationService = notificationService;

            _eventHandlers = new Dictionary<EventType, Func<DomainEvent, Task>>
            {
                { EventType.ChargeFailed, ChargeFailedAsync },
                { EventType.ChargeRefunded, ChargeRefundedAsync },
                { EventType.ChargeSucceeded, ChargeSucceededAsync },
            };
        }

        public async Task HandleEventAsync(string eventId)
        {
            // Retrieving event from billing
            DomainEvent billingEvent = await _billingEventService.GetAsync(new DomainEvent { Id = eventId });

            // Retrieve event handler
            Func<DomainEvent, Task> eventHandler;
            if (!_eventHandlers.TryGetValue(billingEvent.Type, out eventHandler))
            {
                return;
            }

            // Insert log entity (persistence lock to prevent double charging)
            await _billingEventLogService.AddAsync(billingEvent);

            // Handle event within critical section
            try
            {
                // Process event
                await eventHandler(billingEvent);
            }
            catch
            {
                // Deleting event from log if processing has failed
                _billingEventLogService.DeleteAsync(new DomainEvent { Id = eventId }).Wait();

                throw;
            }
        }

        private async Task ChargeSucceededAsync(DomainEvent billingEvent)
        {
            // Retrieve company by charge data
            DomainCharge charge = await _billingChargeService.GetAsync(new DomainCharge { Id = billingEvent.ObjectId });
            DomainCompany company = await _companyService.FindByCustomerAsync(charge.CustomerId);

            // Updating balance
            var balanceRecord = new DomainBalanceHistory
            {
                Amount = charge.AmountInCents,
                Description = BillingMessages.ChargeSucceeded,
                CompanyId = company.Id
            };

            await _balanceService.AddAsync(balanceRecord);

            // Notify client about payment operation result
            try
            {
                await _notificationService.SendPaymentNotificationAsync(billingEvent, company, charge);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send payment notification e-mail to company {0}: {1}", company.Id, e);
            }
        }

        private async Task ChargeRefundedAsync(DomainEvent billingEvent)
        {
            // Get charge info
            DomainCharge charge = await _billingChargeService.GetAsync(new DomainCharge { Id = billingEvent.ObjectId });

            var refunds = billingEvent.Object["refunds"] as JArray;
            if (refunds != null)
            {
                // refund can be partial, accounting only last refund
                JToken lastRefund = refunds.Last;
                int refundInCents = Int32.Parse(lastRefund["amount"].ToString());

                charge.AmountInCents = refundInCents;
            }

            // Retrieve company by customer
            DomainCompany company = await _companyService.FindByCustomerAsync(charge.CustomerId);

            // updating balance
            var balanceRecord = new DomainBalanceHistory
            {
                Amount = -charge.AmountInCents,
                Description = BillingMessages.ChargeRefunded,
                CompanyId = company.Id
            };

            await _balanceService.AddAsync(balanceRecord);

            // Notify client about payment operation result
            try
            {
                await _notificationService.SendPaymentNotificationAsync(billingEvent, company, charge);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send payment notification e-mail to company {0}: {1}", company.Id, e);
            }
        }

        private async Task ChargeFailedAsync(DomainEvent billingEvent)
        {
            // Retrieve company by charge data
            DomainCharge charge = await _billingChargeService.GetAsync(new DomainCharge { Id = billingEvent.ObjectId });
            DomainCompany company = await _companyService.FindByCustomerAsync(charge.CustomerId);

            // Notify client about payment operation result
            try
            {
                await _notificationService.SendPaymentNotificationAsync(billingEvent, company, charge);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send payment notification e-mail to company {0}: {1}", company.Id, e);
            }
        }
    }
}