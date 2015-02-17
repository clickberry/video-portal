// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Configuration;
using Configuration.Azure.Concrete;
using Configuration.ReportGenerator;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using MongoDB.Driver;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Services;
using Portal.BLL.Concrete.Statistics.Filter;
using Portal.BLL.Concrete.Statistics.Generator;
using Portal.BLL.Concrete.Statistics.Helper;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Statistics.Filter;
using Portal.BLL.Statistics.Generator;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.DAL.Azure.Context;
using Portal.DAL.Azure.Statistics;
using Portal.DAL.Azure.User;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.DAL.Statistics;
using Portal.DAL.User;
using Portal.Mappers;
using Portal.Mappers.Statistics;
using SimpleInjector;
using Wrappers.Implementation;
using Wrappers.Interface;

namespace MiddleEnd.Api
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            var container = new Container();

            Initialize(container);

            try
            {
                container.Verify();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Invalid IoC configuration: {0}", exception);
                throw;
            }

            try
            {
                var initializers = new List<IInitializable>
                {
                    container.GetInstance<DiagnosticsInitializer>(),
                    container.GetInstance<TraceListenersInitializer>(),
                    container.GetInstance<TempPathInitializer>(),
                    container.GetInstance<MiddleEndConfigurationInitializer>(),
                    container.GetInstance<ReportGeneratorInitializer>(),
                    container.GetInstance<MongoMigrationInitializer>(),
                    container.GetInstance<RolesInitializer>()
                };

                foreach (IInitializable initializer in initializers)
                {
                    Trace.TraceInformation("Executing '{0}' initializer.", initializer.GetType().Name);
                    initializer.Initialize();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Middle end has failed: {0}", e);
                throw;
            }

            return base.OnStart();
        }

        public static void Initialize(Container container)
        {
            // Infrastructure
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalMiddleendSettings, PortalMiddleendSettings>();
            container.Register<IPortalSettings, PortalSettings>();
            container.RegisterSingle<IMapper, PortalMapper>();
            container.Register<ICryptoService, CryptoService>();

            // Email
            container.Register(() => container.GetInstance<IPortalMiddleendSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();

            // Azure SDK
            container.Register(() => CloudStorageAccount.Parse(container.GetInstance<IPortalMiddleendSettings>().DataConnectionString));
            container.Register(() =>
            {
                CloudTableClient tableClient = container.GetInstance<CloudStorageAccount>().CreateCloudTableClient();
                tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(4), 4);
                return tableClient;
            });
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudBlobClient());


            // DAL
            container.Register(() => new MongoUrl(container.GetInstance<IPortalMiddleendSettings>().MongoConnectionString));
            container.Register<IRepositoryFactory, RepositoryFactory>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IStandardReportRepository, StandardReportRepository>();
            container.Register<IStatisticsRepository<StatWatchingV2Entity>, StatisticsRepository<StatWatchingV2Entity>>();
            container.Register<IStatisticsRepository<StatUserRegistrationV2Entity>, StatisticsRepository<StatUserRegistrationV2Entity>>();
            container.Register<IStatisticsRepository<StatProjectUploadingV2Entity>, StatisticsRepository<StatProjectUploadingV2Entity>>();
            container.Register<IStatisticsRepository<StatProjectStateV3Entity>, StatisticsRepository<StatProjectStateV3Entity>>();
            container.Register<IStatisticsRepository<StatProjectDeletionV2Entity>, StatisticsRepository<StatProjectDeletionV2Entity>>();

            // BLL
            container.Register<IPasswordService, PasswordService>();


            // Initializers
            container.Register<MiddleEndConfigurationInitializer>();
            container.Register<DiagnosticsInitializer>();
            container.Register<TempPathInitializer>();
            container.Register<ReportGeneratorInitializer>();
            container.Register<TraceListenersInitializer>();
            container.Register<RolesInitializer>();
            container.Register(() => new MongoMigrationInitializer(
                container.GetInstance<MongoUrl>(),
                container.GetInstance<IPortalMiddleendSettings>().MongoAutomigrationEnabled));

            //Report
            container.Register<IReportGenerator, ReportGenerator>();
            container.Register<IStandardReportService, StandardReportService>();
            container.Register<IReportBuilderService, ReportBuilderService>();
            container.Register<ICompilerFactory, CompilerFactory>();
            container.Register<ITableValueConverter, TableValueConverter>();
            container.Register<IGuidWrapper, GuidWrapper>();
            container.Register<IDateTimeWrapper, DateTimeWrapper>();
            container.Register<ITimerWrapper, TimerWrapper>();
            container.Register<IFiltersManager, FiltersManager>();
            container.Register<IFiltersFactory>(() => new FiltersFactory(GetHostName(container)));
            container.Register<IFiltersChainBuilder, FiltersChainBuilder>();
            container.Register<IReportAccumulator, ReportAccumulator>();
            container.Register<IStatisticsService, StatisticsService>();
            container.Register<IStatMapper, StatMapper>();
            container.Register<IIntervalHelper, IntervalHelper>();
            container.Register<IReportMapper, ReportMapper>();
        }

        private static string GetHostName(Container container)
        {
            string url = container.GetInstance<IPortalMiddleendSettings>().PortalUri;
            return new Uri(url).Host;
        }
    }
}