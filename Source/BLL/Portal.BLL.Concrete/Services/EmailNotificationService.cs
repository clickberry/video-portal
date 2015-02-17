// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.BillingContext;
using Portal.Domain.MailerContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Watch;
using Portal.Resources.Emails;
using PortalResources = Portal.Resources.Properties.Resources;

namespace Portal.BLL.Concrete.Services
{
    /// <summary>
    ///     E-mail notifications.
    /// </summary>
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly ITableRepository<SendEmailEntity> _emailRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IProductIdExtractor _productIdExtractor;
        private readonly IProjectUriProvider _projectUriProvider;
        private readonly IPortalFrontendSettings _settings;
        private readonly IUserService _userService;
        private readonly IUserUriProvider _userUriProvider;

        public EmailNotificationService(
            IRepositoryFactory repositoryFactory,
            IEmailSenderService emailSenderService,
            IPortalFrontendSettings settings,
            IUserService userService,
            IProductIdExtractor productIdExtractor,
            IProjectUriProvider projectUriProvider,
            IUserUriProvider userUriProvider)
        {
            _emailSenderService = emailSenderService;
            _settings = settings;
            _productIdExtractor = productIdExtractor;
            _userService = userService;
            _projectUriProvider = projectUriProvider;
            _userUriProvider = userUriProvider;
            _emailRepository = repositoryFactory.Create<SendEmailEntity>();
        }

        public async Task SendFirstProjectEmail(string userId, string userAgent)
        {
            if (!_settings.EmailNotifications)
            {
                return;
            }

            string template;
            EmailTemplate templateId;
            ProductType product = _productIdExtractor.Get(userAgent);

            switch (product)
            {
                case ProductType.ImageShack:
                    template = PortalResources.UserRegistrationImageshack;
                    templateId = EmailTemplate.FirstProjectExtension;
                    break;

                case ProductType.CicIPad:
                case ProductType.CicMac:
                case ProductType.CicPc:
                    template = PortalResources.ProjectFirstCic;
                    templateId = EmailTemplate.FirstProjectCic;
                    break;

                case ProductType.TaggerAndroid:
                case ProductType.TaggerIPhone:
                    template = PortalResources.ProjectFirstTagger;
                    templateId = EmailTemplate.FirstProjectOtherProducts;
                    break;

                case ProductType.Standalone:
                    template = PortalResources.UserRegistration;
                    templateId = EmailTemplate.Registration;
                    break;

                default:
                    return;
            }

            bool emailAlreadySendOnce = await CheckEmailBeenSend(userId, templateId);
            if (emailAlreadySendOnce)
            {
                return;
            }

            DomainUser user = await _userService.GetAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return;
            }

            var email = new SendEmailDomain
            {
                Body = string.Format(template, user.Name),
                Address = _settings.EmailAddressInfo,
                DisplayName = Emails.SenderDisplayName,
                Subject = Emails.SubjectFirstTag,
                Emails = new List<string> { user.Email },
                UserId = userId,
                Template = templateId
            };

            await _emailSenderService.SendAndLogEmailAsync(email);
        }

        public Task SendClientActivationEmailAsync(DomainPendingClient client)
        {
            if (string.IsNullOrEmpty(client.Email))
            {
                return Task.FromResult(0);
            }

            var email = new SendEmailDomain
            {
                Address = _settings.EmailAddressAlerts,
                DisplayName = Emails.SenderDisplayName,
                Emails = new List<string> { client.Email },
                Subject = Emails.SubjectRegistration,
                Body = string.Format(
                    PortalResources.ClientActivation,
                    client.ContactPerson,
                    client.Email,
                    _settings.PortalUri,
                    client.Id)
            };

            // Send email on user registration
            return _emailSenderService.SendEmailAsync(email);
        }

        public async Task SendPaymentNotificationAsync(DomainEvent billingEvent, DomainCompany company, DomainCharge charge)
        {
            if (billingEvent == null)
            {
                throw new ArgumentNullException("billingEvent");
            }

            if (company == null)
            {
                throw new ArgumentNullException("company");
            }

            if (charge == null)
            {
                throw new ArgumentNullException("charge");
            }

            if (!_settings.EmailNotifications)
            {
                return;
            }

            var email = new SendEmailDomain
            {
                Address = _settings.EmailAddressAlerts,
                DisplayName = Emails.SenderDisplayName,
                Emails = new List<string> { company.Email }
            };

            switch (billingEvent.Type)
            {
                case EventType.ChargeFailed:
                    email.Subject = Emails.SubjectPaymentFailed;
                    email.Body = string.Format(
                        PortalResources.PaymentFailed,
                        company.Name,
                        billingEvent.Id,
                        string.Format("{0} {1}", charge.AmountInCents*0.01, charge.Currency),
                        charge.Created);
                    break;

                case EventType.ChargeSucceeded:
                    email.Subject = Emails.SubjectPaymentCompleted;
                    email.Body = string.Format(
                        PortalResources.PaymentCompleted,
                        company.Name,
                        billingEvent.Id,
                        string.Format("{0} {1}", charge.AmountInCents*0.01, charge.Currency),
                        charge.Created);
                    break;

                case EventType.ChargeRefunded:
                    email.Subject = Emails.SubjectPaymentRefunded;
                    email.Body = string.Format(
                        PortalResources.PaymentRefunded,
                        company.Name,
                        billingEvent.Id,
                        string.Format("{0} {1}", charge.AmountInCents*0.01, charge.Currency),
                        charge.Created);
                    break;

                default:
                    return;
            }


            // Send email on user registration
            await _emailSenderService.SendEmailAsync(email);
        }

        public async Task SendAbuseNotificationAsync(Watch project, DomainUser reporter)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            if (reporter == null)
            {
                throw new ArgumentNullException("reporter");
            }

            if (!_settings.EmailNotifications)
            {
                return;
            }

            string projectUri = _projectUriProvider.GetUri(project.Id);
            var email = new SendEmailDomain
            {
                Address = _settings.EmailAddressAlerts,
                DisplayName = Emails.SenderDisplayName,
                Emails = new List<string> { _settings.EmailAddressAbuse },
                Subject = string.Format(Emails.SubjectAbuseReport, projectUri),
                Body = string.Format(
                    PortalResources.AbuseReported,
                    reporter.Name,
                    _userUriProvider.GetUri(reporter.Id),
                    project.Name,
                    projectUri)
            };

            // Send email
            await _emailSenderService.SendEmailAsync(email);
        }

        public Task SendVideoCommentNotificationAsync(DomainUser user, DomainProject project, DomainComment domainComment)
        {
            var email = new SendEmailDomain
            {
                Address = _settings.EmailAddressInfo,
                DisplayName = Emails.SenderDisplayName,
                Emails = new List<string> { user.Email },
                Subject = Emails.SubjectVideoComment,
                UserId = user.Id,
                Body = string.Format(PortalResources.VideoCommentNotification,
                    user.Name,
                    _userUriProvider.GetUri(domainComment.UserId),
                    domainComment.UserName,
                    _projectUriProvider.GetUri(project.Id),
                    project.Name,
                    domainComment.Body,
                    _settings.PortalUri)
            };

            // Send email on user registration
            return _emailSenderService.SendEmailAsync(email);
        }

        public Task SendRegistrationEmailAsync(DomainUser user)
        {
            if (string.IsNullOrEmpty(user.Email) || !_settings.EmailNotifications)
            {
                return Task.FromResult(0);
            }

            switch (user.UserAgent)
            {
                case ProductType.CicPc:
                case ProductType.CicMac:
                case ProductType.CicIPad:
                case ProductType.TaggerAndroid:
                case ProductType.TaggerIPhone:
                case ProductType.YoutubePlayer:
                case ProductType.DailyMotion:
                case ProductType.Other:
                    break;

                default:
                    return Task.FromResult(0);
            }

            var email = new SendEmailDomain
            {
                Address = _settings.EmailAddressInfo,
                DisplayName = Emails.SenderDisplayName,
                Emails = new List<string> { user.Email },
                Subject = Emails.SubjectRegistration,
                UserId = user.Id,
                Body = string.Format(PortalResources.UserRegistration, user.Name)
            };

            // Send email on user registration
            return _emailSenderService.SendEmailAsync(email);
        }

        private async Task<bool> CheckEmailBeenSend(string userId, EmailTemplate templateId)
        {
            string templateIdentifier = ((int)templateId).ToString(CultureInfo.InvariantCulture);
            List<SendEmailEntity> emails = await _emailRepository.ToListAsync(
                p => p.UserId == userId && p.Id.StartsWith(templateIdentifier));

            return emails.Count > 0;
        }
    }
}