// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Asp.Infrastructure.Filters;
using Configuration;
using Configuration.Azure.Concrete;
using MediaInfoLibrary;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEnd.Api;
using MiddleEnd.Worker.Abstract;
using MiddleEnd.Worker.Concrete;
using MongoDB.Driver;
using Portal.BLL.Concrete.Infrastructure;
using Portal.BLL.Concrete.Infrastructure.ProcessedEntity;
using Portal.BLL.Concrete.Multimedia.Adjusters;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Comparator;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.BLL.Concrete.Multimedia.Lists;
using Portal.BLL.Concrete.Services;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Azure.Context;
using Portal.DAL.Azure.FileSystem;
using Portal.DAL.Azure.Project;
using Portal.DAL.Azure.User;
using Portal.DAL.Context;
using Portal.DAL.FileSystem;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Infrastructure.Multimedia;
using Portal.DAL.Mailer;
using Portal.DAL.Multimedia;
using Portal.DAL.Project;
using Portal.DAL.User;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProjectContext;
using Portal.Mappers;
using Portal.Mappers.Statistics;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Wrappers.Implementation;
using Wrappers.Interface;

[assembly: PreApplicationStartMethod(typeof (SimpleInjectorInitializer), "Initialize")]

namespace MiddleEnd.Api
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var config = GlobalConfiguration.Configuration;

            var container = new Container();
            container.Options.PropertySelectionBehavior = new ImportPropertySelectionBehavior();

            Configure(container);

            try
            {
                container.Verify();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to initialize IoC container: {0}", e);
                throw;
            }

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            container.RegisterWebApiFilterProvider(config);

            try
            {
                new TraceListenersInitializer(container.GetInstance<IPortalMiddleendSettings>(), container.GetInstance<IEmailSenderService>()).Initialize();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to initialize trace listeners: {0}", e);
                throw;
            }
        }

        public static void Configure(Container container)
        {
            // Infratructure
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalMiddleendSettings, PortalMiddleendSettings>();
            container.Register<IPortalSettings, PortalSettings>();

            //
            // Interface Types
            //

            container.RegisterSingle<IMapper, PortalMapper>();
            container.RegisterSingle<ITaskStateManager, TaskStateManager>();
            container.RegisterSingle<ITaskKeeper, TaskKeeper>();
            container.RegisterSingle<ITaskProvider, TaskProvider>();
            container.Register<ITableValueConverter, TableValueConverter>();
            container.Register<IGuidWrapper, GuidWrapper>();

            container.Register(() => new Lazy<Task<IMediaInfo>>(MediaInfo.Create));
            container.Register<ITaskChecker, TaskChecker>();

            // BLL
            container.Register<IProjectVideoService, ProjectVideoService>();
            container.Register<IProjectAvsxService, ProjectAvsxService>();
            container.Register<IProjectScreenshotService, ProjectScreenshotService>();
            container.Register<IService<DomainProjectProcessedVideo>, ProjectProcessedVideoService>();
            container.Register(() => new Lazy<IService<DomainProjectProcessedVideo>>(container.GetInstance<IService<DomainProjectProcessedVideo>>));
            container.Register<IService<DomainProjectProcessedScreenshot>, ProjectProcessedScreenshotService>();
            container.Register<IProjectService, ProjectService>();
            container.Register<IProcessedEntityGenerator<DomainProcessedVideo>, ProcessedVideoGenerator>();
            container.Register<IVideoMetadataProvider, VideoMetadataProvider>();
            container.Register(() => new Lazy<IVideoMetadataProvider>(container.GetInstance<IVideoMetadataProvider>));
            container.Register<IVideoMetadataParser, VideoMetadataParser>();
            container.Register<IResolutionCalculator, ResolutionCalculator>();
            container.Register<IMultimediaAdjusterParamFactory, MultimediaAdjusterParamFactory>();
            container.Register<IProcessedVideoList, ProcessedVideoList>();
            container.Register<IProcessedVideoBuilder, ProcessedVideoBuilder>();
            container.Register<IVideoAdjuster, VideoAdjuster>();
            container.Register<IAdjustmentVideoMetadata, AdjustmentVideoMetadata>();
            container.Register<IAudioAdjuster, AudioAdjuster>();
            container.Register<IAdjustmentAudioMetadata, AdjustmentAudioMetadata>();
            container.Register<IComparator, Comparator>();
            container.Register<IProcessedEntityGenerator<DomainProcessedScreenshot>, ProcessedScreenshotGenerator>();
            container.Register<IScreenshotAdjusterParamFactory, ScreenshotAdjusterParamFactory>();
            container.Register<IProcessedScreenshotBuilder, ProcessdScreenshotBuilder>();
            container.Register<IScreenshotAdjuster, ScreenshotAdjuster>();
            container.Register<IAdjustmentScreenshotMetadata, AdjustmentScreenshotMetadata>();
            container.Register<IProcessedVideoHandler, ProcessedVideoHandler>();
            container.Register<ILastProjectService, LastProjectService>();
            container.Register<IProcessedEntityManager, ProcessedEntityManager>();

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

            // Azure DAL
            container.Register(() => new MongoUrl(container.GetInstance<IPortalMiddleendSettings>().MongoConnectionString));
            container.Register<IRepositoryFactory, RepositoryFactory>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IProjectRepository, ProjectRepository>();
            container.Register<IFileRepository, FileRepository>();
            container.Register<IFileSystem, FileSystem>();

            container.Register<IFileUriProvider, FileUriProvider>();
        }
    }
}