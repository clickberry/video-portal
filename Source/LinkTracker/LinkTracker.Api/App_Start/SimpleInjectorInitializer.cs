// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Filters;
using Configuration;
using Configuration.Azure.Concrete;
using LinkTracker.Api;
using LinkTracker.BLL;
using LinkTracker.BLL.Concrete;
using LinkTracker.BLL.Concrete.Infrastructure;
using LinkTracker.BLL.Concrete.Interceptions;
using LinkTracker.BLL.Infrastructure;
using LinkTracker.Configuration;
using LinkTracker.Configuration.Azure;
using LinkTracker.DAL;
using LinkTracker.DAL.Mongo;
using LinkTracker.DAL.Mongo.IdGenerators;
using LinkTracker.Infrastructure.Dependencies;
using LinkTracker.Infrastructure.Filters;
using LinkTracker.Mappings;
using MongoDB.Driver;
using Portal.BLL.Billing;
using Portal.BLL.Concrete.Billing;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Services;
using Portal.BLL.Concrete.Subscriptions;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Subscriptions;
using Portal.Common.Interceptions;
using Portal.DAL.Azure.Context;
using Portal.DAL.Azure.Subscriptions;
using Portal.DAL.Context;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.DAL.Subscriptions;
using Portal.Mappers;
using SimpleInjector;
using WebActivator;

[assembly: PostApplicationStartMethod(typeof (SimpleInjectorInitializer), "Initialize")]

namespace LinkTracker.Api
{
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();

            InitializeContainer(container);

            try
            {
                container.Verify();
            }
            catch (Exception exception)
            {
                Trace.TraceError("{0}", exception);
                throw;
            }

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.Services.Replace(typeof (IFilterProvider), new SimpleInjectorWebApiFilterProvider(container));

            try
            {
                new TraceListenersInitializer(container.GetInstance<IPortalSettings>(), container.GetInstance<IEmailSenderService>()).Initialize();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to initialize trace listeners: {0}", e);
                throw;
            }
        }

        private static void InitializeContainer(Container container)
        {
            // Infrastructure
            container.Register<IApiConfigurationProvider, AzureApiConfigurationProvider>();
            container.Register<IMappingEngine, MappingEngine>(Lifestyle.Singleton);
            container.Register<IMapper, PortalMapper>(Lifestyle.Singleton);
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalSettings, PortalSettings>();


            // DAL Infrastructure
            container.Register(() => new MongoUrl(container.GetInstance<IApiConfigurationProvider>().MongoConnectionString));
            container.RegisterSingle<ITrackingUrlIdGenerator, TrackingUrlIdGenerator>();

            // DAL
            container.Register<IRepositoryFactory, RepositoryFactory>();
            container.Register<ITrackingUrlRepository, TrackingUrlRepository>();
            container.Register<ITrackingStatRepository, TrackingStatRepository>();
            container.Register<IBalanceHistoryRepository, BalanceHistoryRepository>();
            container.Register<ICompanyRepository, CompanyRepository>();


            // BLL Infrastructure
            container.Register<IUrlShortenerService, UrlShortenerService>(Lifestyle.Singleton);
            container.Register<IProjectUriProvider>(() => new ProjectUriProvider(
                container.GetInstance<IApiConfigurationProvider>().ProjectBaseUri), Lifestyle.Singleton);

            // Email
            container.Register(() => container.GetInstance<IPortalSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();

            // BLL
            container.Register<IBalanceService, BalanceService>();
            container.Register<IUrlTrackingService, UrlTrackingService>();
            container.Register<IUrlTrackingStatService, UrlTrackingStatService>();
            container.Register<ICompanyService, CompanyService>();
            container.Register<ISubscriptionService, SubscriptionService>();


            // BLL Billing
            container.Register<IBillingCardService>(
                () =>
                    new StripeBillingCardService(container.GetInstance<IApiConfigurationProvider>().StripeApiKey,
                        container.GetInstance<IMapper>()));
            container.Register<IBillingChargeService>(
                () =>
                    new StripeBillingChargeService(container.GetInstance<IApiConfigurationProvider>().StripeApiKey,
                        container.GetInstance<IMapper>()));
            container.Register<IBillingCustomerService>(
                () =>
                    new StripeBillingCustomerService(container.GetInstance<IApiConfigurationProvider>().StripeApiKey,
                        container.GetInstance<IMapper>()));
            container.Register<IBillingEventService>(
                () =>
                    new StripeBillingEventService(container.GetInstance<IApiConfigurationProvider>().StripeApiKey,
                        container.GetInstance<IMapper>()));


            // Interceptions
            container.InterceptWith<UrlTrackingServiceStatInterceptor>(type => type == typeof (IUrlTrackingService));
            container.RegisterSingle<UrlTrackingServiceStatInterceptor>();
        }
    }
}