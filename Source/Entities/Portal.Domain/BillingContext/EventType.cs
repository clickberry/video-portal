// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.BillingContext
{
    /// <summary>
    ///     Enum representation of stripe event types: https://stripe.com/docs/api#event_types
    /// </summary>
    public enum EventType
    {
        Undefined,
        AccountUpdated,
        AccountApplicationDeauthorized,
        BalanceAvailable,
        ChargeSucceeded,
        ChargeFailed,
        ChargeRefunded,
        ChargeCaptured,
        ChargeUpdated,
        ChargeDisputeCreated,
        ChargeDisputeUpdated,
        ChargeDisputeClosed,
        CustomerCreated,
        CustomerUpdated,
        CustomerDeleted,
        CustomerCardCreated,
        CustomerCardUpdated,
        CustomerCardDeleted,
        CustomerSubscriptionCreated,
        CustomerSubscriptionUpdated,
        CustomerSubscriptionDeleted,
        CustomerSubscriptionTrialWillEnd,
        CustomerDiscountCreated,
        CustomerDiscountUpdated,
        CustomerDiscountDeleted,
        InvoiceCreated,
        InvoiceUpdated,
        InvoicePaymentSucceeded,
        InvoicePaymentFailed,
        InvoiceItemCreated,
        InvoiceItemUpdated,
        InvoiceItemDeleted,
        PlanCreated,
        PlanUpdated,
        PlanDeleted,
        CouponCreated,
        CouponDeleted,
        TransferCreated,
        TransferUpdated,
        TransferPaid,
        TransferFailed,
        Ping
    }
}