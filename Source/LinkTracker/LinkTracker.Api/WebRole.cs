// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Configuration;
using Configuration.Azure.Concrete;
using Microsoft.WindowsAzure.ServiceRuntime;
using MongoDB.Driver;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Services;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Azure.Context;
using Portal.DAL.Context;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.Mappers;
using SimpleInjector;

namespace LinkTracker.Api
{
    public class WebRole : RoleEntryPoint
    {
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
                    container.GetInstance<DiagnosticsInitializer>(),
                    container.GetInstance<AspInitializer>()
                };

                foreach (IInitializable initializer in initializers)
                {
                    initializer.Initialize();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to start role: {0}", e);
                throw;
            }

            return base.OnStart();
        }

        private static void Initialize(Container container)
        {
            // Infrastructure
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalSettings, PortalSettings>();
            container.RegisterSingle<IMapper, PortalMapper>();

            // DAL
            container.Register(() => new MongoUrl(container.GetInstance<IPortalSettings>().MongoConnectionString));
            container.Register<IRepositoryFactory, RepositoryFactory>();

            // Email
            container.Register(() => container.GetInstance<IPortalSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();

            // INITIALIZERS
            container.Register<DiagnosticsInitializer>();
            container.Register<TraceListenersInitializer>();
            container.Register<AspInitializer>();
        }
    }
}