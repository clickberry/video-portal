// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoMapper;
using Portal.Domain.BillingContext;
using Stripe;

namespace Portal.Mappers.ValueResolvers
{
    public class StripeEventToEventTypeResolver : ValueResolver<StripeEvent, EventType>
    {
        private readonly Dictionary<string, EventType> _intervals = new Dictionary<string, EventType>
        {
            { "account.updated", EventType.AccountUpdated },
            { "account.application.deauthorized", EventType.AccountApplicationDeauthorized },
            { "balance.available", EventType.BalanceAvailable },
            { "charge.succeeded", EventType.ChargeSucceeded },
            { "charge.failed", EventType.ChargeFailed },
            { "charge.refunded", EventType.ChargeRefunded },
            { "charge.captured", EventType.ChargeCaptured },
            { "charge.updated", EventType.ChargeUpdated },
            { "charge.dispute.created", EventType.ChargeDisputeCreated },
            { "charge.dispute.updated", EventType.ChargeDisputeUpdated },
            { "charge.dispute.closed", EventType.ChargeDisputeClosed },
            { "customer.created", EventType.CustomerCreated },
            { "customer.updated", EventType.CustomerUpdated },
            { "customer.deleted", EventType.CustomerDeleted },
            { "customer.card.created", EventType.CustomerCardCreated },
            { "customer.card.updated", EventType.CustomerCardUpdated },
            { "customer.card.deleted", EventType.CustomerCardDeleted },
            { "customer.subscription.created", EventType.CustomerSubscriptionCreated },
            { "customer.subscription.updated", EventType.CustomerSubscriptionUpdated },
            { "customer.subscription.deleted", EventType.CustomerSubscriptionDeleted },
            { "customer.subscription.trial_will_end", EventType.CustomerSubscriptionTrialWillEnd },
            { "customer.discount.created", EventType.CustomerDiscountCreated },
            { "customer.discount.updated", EventType.CustomerDiscountUpdated },
            { "customer.discount.deleted", EventType.CustomerDiscountDeleted },
            { "invoice.created", EventType.InvoiceCreated },
            { "invoice.updated", EventType.InvoiceUpdated },
            { "invoice.payment_succeeded", EventType.InvoicePaymentSucceeded },
            { "invoice.payment_failed", EventType.InvoicePaymentFailed },
            { "invoiceitem.created", EventType.InvoiceItemCreated },
            { "invoiceitem.updated", EventType.InvoiceItemUpdated },
            { "invoiceitem.deleted", EventType.InvoiceItemDeleted },
            { "plan.created", EventType.PlanCreated },
            { "plan.updated", EventType.PlanUpdated },
            { "plan.deleted", EventType.PlanDeleted },
            { "coupon.created", EventType.CouponCreated },
            { "coupon.deleted", EventType.CouponDeleted },
            { "transfer.created", EventType.TransferCreated },
            { "transfer.updated", EventType.TransferUpdated },
            { "transfer.paid", EventType.TransferPaid },
            { "transfer.failed", EventType.TransferFailed },
            { "ping", EventType.Ping }
        };

        protected override EventType ResolveCore(StripeEvent source)
        {
            string type = source.Type.ToLowerInvariant();
            if (!_intervals.ContainsKey(type))
            {
                return EventType.Undefined;
            }

            return _intervals[type];
        }
    }
}