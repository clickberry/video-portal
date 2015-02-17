// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Configuration;
using Configuration.Azure.Concrete;
using LinkTracker.Configuration;
using LinkTracker.Configuration.Azure;
using LinkTracker.Mappings;
using Microsoft.WindowsAzure.ServiceRuntime;
using MongoDB.Driver;
using Portal.BLL.Billing;
using Portal.BLL.Concrete.Billing;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Services;
using Portal.BLL.Concrete.Subscriptions;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Subscriptions;
using Portal.DAL.Azure.Context;
using Portal.DAL.Azure.Subscriptions;
using Portal.DAL.Context;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.DAL.Subscriptions;
using SimpleInjector;

namespace LinkTracker.BillingSync
{
    public class WorkerRole : RoleEntryPoint
    {
        private Container _container;
        private bool _isStopRequired;

        public override void Run()
        {
            var config = _container.GetInstance<IBillingSyncConfigurationProvider>();
            var synchronizationManager = _container.GetInstance<BillingSynchronizationManager>();

            while (!_isStopRequired)
            {
                synchronizationManager.SyncAsync().Wait();
                Task.Delay(config.SyncInterval).Wait();
            }
        }

        public override bool OnStart()
        {
            var container = new Container();

            try
            {
                // Building IoC container
                Initialize(container);
                container.Verify();

                // Initializers
                var initializers = new List<IInitializable>
                {
                    container.GetInstance<TraceListenersInitializer>(),
                    container.GetInstance<DiagnosticsInitializer>()
                };
                foreach (IInitializable initializer in initializers)
                {
                    initializer.Initialize();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to start BillingSync worker role: {0}", e);
                throw;
            }

            _container = container;


            return base.OnStart();
        }

        public override void OnStop()
        {
            _isStopRequired = true;

            base.OnStop();
        }

        private static void Initialize(Container container)
        {
            // Configuration provider
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IBillingSyncConfigurationProvider, AzureBillingSyncConfigurationProvider>(Lifestyle.Singleton);
            container.Register<IPortalSettings, PortalSettings>();

            // Infrastructure
            container.Register<IMappingEngine, MappingEngine>(Lifestyle.Singleton);
            container.Register<Portal.Mappers.IMapper, Portal.Mappers.PortalMapper>(Lifestyle.Singleton);

            // DAL
            container.Register<IRepositoryFactory>(
                () => new RepositoryFactory(
                    new MongoUrl(container.GetInstance<IBillingSyncConfigurationProvider>().MongoConnectionString)));
            container.Register<ITrackingStatRepository>(
                () => new TrackingStatRepository(
                    new MongoUrl(container.GetInstance<IBillingSyncConfigurationProvider>().MongoConnectionString)));
            container.Register<IBalanceHistoryRepository>(
                () => new BalanceHistoryRepository(
                    new MongoUrl(container.GetInstance<IBillingSyncConfigurationProvider>().MongoConnectionString)));
            container.Register<ICompanyRepository>(
                () => new CompanyRepository(
                    new MongoUrl(container.GetInstance<IBillingSyncConfigurationProvider>().MongoConnectionString)));

            // BLL
            container.Register<IBalanceService, BalanceService>();
            container.Register<IUrlTrackingStatService, UrlTrackingStatService>();
            container.Register<ICompanyService, CompanyService>();
            container.Register<ISubscriptionService, SubscriptionService>();

            // Email
            container.Register(() => container.GetInstance<IPortalSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();

            // BLL Billing
            container.Register<IBillingCardService>(
                () =>
                    new StripeBillingCardService(
                        container.GetInstance<IBillingSyncConfigurationProvider>().StripeApiKey,
                        container.GetInstance<Portal.Mappers.IMapper>()));
            container.Register<IBillingChargeService>(
                () =>
                    new StripeBillingChargeService(
                        container.GetInstance<IBillingSyncConfigurationProvider>().StripeApiKey,
                        container.GetInstance<Portal.Mappers.IMapper>()));
            container.Register<IBillingCustomerService>(
                () =>
                    new StripeBillingCustomerService(
                        container.GetInstance<IBillingSyncConfigurationProvider>().StripeApiKey,
                        container.GetInstance<Portal.Mappers.IMapper>()));

            // INITIALIZERS
            container.Register<DiagnosticsInitializer>();
            container.Register<TraceListenersInitializer>();

            // WORKER ROLE
            container.Register<BillingSynchronizationManager, BillingSynchronizationManager>();
        }
    }
}