// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.BillingContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Watch;

namespace Portal.BLL.Services
{
    public interface IEmailNotificationService
    {
        Task SendFirstProjectEmail(string userId, string userAgent);

        Task SendRegistrationEmailAsync(DomainUser user);

        Task SendClientActivationEmailAsync(DomainPendingClient client);

        Task SendPaymentNotificationAsync(DomainEvent billingEvent, DomainCompany company, DomainCharge charge);

        Task SendAbuseNotificationAsync(Watch project, DomainUser reporter);

        Task SendVideoCommentNotificationAsync(DomainUser user, DomainProject project, DomainComment domainComment);
    }
}