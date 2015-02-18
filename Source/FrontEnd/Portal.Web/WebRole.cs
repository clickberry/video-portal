// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Configuration;
using Configuration.Azure.Concrete;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Services;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL;
using Portal.DAL.Azure;
using Portal.DAL.Azure.Context;
using Portal.DAL.Context;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.Mappers;
using SimpleInjector;

namespace Portal.Web
{
    public class WebRole : RoleEntryPoint
    {
        private List<IInitializable> _initializers;

        public override bool OnStart()
        {
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
                _initializers = new List<IInitializable>
                {
                    container.GetInstance<DiagnosticsInitializer>(),
                    container.GetInstance<TraceListenersInitializer>(),
                    container.GetInstance<FrontEndConfigurationInitializer>(),
                    container.GetInstance<AspInitializer>(),
                    container.GetInstance<IisLoggingInitializer>(),
                    container.GetInstance<BlobCorsInitializer>(),
                    container.GetInstance<StatisticsInitializer>()
                };

                foreach (IInitializable initializer in _initializers)
                {
                    Trace.TraceInformation("Executing '{0}' initializer.", initializer.GetType().Name);
                    initializer.Initialize();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Web role has failed: {0}", e);
                throw;
            }

            return base.OnStart();
        }

        public override void OnStop()
        {
            foreach (IInitializable initializable in _initializers)
            {
                if (initializable is IDisposable)
                {
                    (initializable as IDisposable).Dispose();
                }
            }

            base.OnStop();
        }

        private static void Initialize(Container container)
        {
            // Infrastructure
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalFrontendSettings, PortalFrontendSettings>();
            container.Register<IPortalSettings, PortalSettings>();
            container.RegisterSingle<IMapper, PortalMapper>();

            // Azure SDK
            container.Register(() => CloudStorageAccount.Parse(container.GetInstance<IPortalFrontendSettings>().DataConnectionString));
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudTableClient());
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudBlobClient());

            // DAL
            container.Register(() => new MongoUrl(container.GetInstance<IPortalFrontendSettings>().MongoConnectionString));
            container.Register<IRepositoryFactory, RepositoryFactory>();

            // DAL Statistics
            container.Register<ICassandraClient, CassandraClient>(Lifestyle.Singleton);

            // Email
            container.Register(() => container.GetInstance<IPortalFrontendSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();

            // Initializers
            container.Register<FrontEndConfigurationInitializer>();
            container.Register<DiagnosticsInitializer>();
            container.Register<TempPathInitializer>();
            container.Register<TraceListenersInitializer>();
            container.Register<BlobCorsInitializer>();
            container.Register<StatisticsInitializer>();
        }
    }
}