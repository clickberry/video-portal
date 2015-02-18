// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

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
using MongoDB.Driver;
using MongoRepository;
using Portal.Api;
using Portal.BLL.Billing;
using Portal.BLL.Concrete.Billing;
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
using Portal.BLL.Concrete.Statistics;
using Portal.BLL.Concrete.Statistics.Aggregator;
using Portal.BLL.Concrete.Statistics.Helper;
using Portal.BLL.Concrete.Statistics.Reporter;
using Portal.BLL.Concrete.Subscriptions;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Statistics;
using Portal.BLL.Statistics.Aggregator;
using Portal.BLL.Statistics.Helper;
using Portal.BLL.Statistics.Reporter;
using Portal.BLL.Subscriptions;
using Portal.DAL;
using Portal.DAL.Authentication;
using Portal.DAL.Azure;
using Portal.DAL.Azure.Comment;
using Portal.DAL.Azure.Context;
using Portal.DAL.Azure.FileSystem;
using Portal.DAL.Azure.Project;
using Portal.DAL.Azure.Statistics;
using Portal.DAL.Azure.Subscriptions;
using Portal.DAL.Azure.User;
using Portal.DAL.Comment;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.Factories;
using Portal.DAL.FileSystem;
using Portal.DAL.Infrastructure.Authentication;
using Portal.DAL.Infrastructure.Context;
using Portal.DAL.Infrastructure.Factories;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Infrastructure.Multimedia;
using Portal.DAL.Infrastructure.NotificationHub;
using Portal.DAL.Mailer;
using Portal.DAL.Multimedia;
using Portal.DAL.NotificationHub;
using Portal.DAL.Project;
using Portal.DAL.Statistics;
using Portal.DAL.Subscriptions;
using Portal.DAL.User;
using Portal.Domain.FileContext;
using Portal.Domain.Notifications;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProjectContext;
using Portal.Mappers;
using Portal.Mappers.Statistics;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Wrappers.Implementation;
using Wrappers.Interface;

[assembly: PreApplicationStartMethod(typeof (SimpleInjectorInitializer), "Initialize")]

namespace Portal.Api
{
    /// <summary>
    ///     Simple Injector Initializer.
    /// </summary>
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var config = GlobalConfiguration.Configuration;

            var container = new Container();
            container.Options.PropertySelectionBehavior = new ImportPropertySelectionBehavior();

            Register(container);

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
                new TraceListenersInitializer(container.GetInstance<IPortalFrontendSettings>(), container.GetInstance<IEmailSenderService>()).Initialize();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to initialize trace listeners: {0}", e);
                throw;
            }
        }

        private static void Register(Container container)
        {
            // Infrastructure
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalFrontendSettings, PortalFrontendSettings>();
            container.Register<IPortalSettings, PortalSettings>();
            container.RegisterSingle<IMapper, PortalMapper>();

            // BLL
            container.Register<IService<DomainUserFile>, UserFileService>();
            container.Register<IProcessedVideoHandler, ProcessedVideoHandler>();
            container.Register<IService<DomainProjectProcessedScreenshot>, ProjectProcessedScreenshotService>();
            container.Register<IService<DomainProjectProcessedVideo>, ProjectProcessedVideoService>();
            container.Register(() => new Lazy<IService<DomainProjectProcessedVideo>>(container.GetInstance<IService<DomainProjectProcessedVideo>>));
            container.Register<IProjectAvsxService, ProjectAvsxService>();
            container.Register<IProjectScreenshotService, ProjectScreenshotService>();
            container.Register<IProjectVideoService, ProjectVideoService>();
            container.Register<IProjectService, ProjectService>();
            container.Register<IUserService, UserService>();
            container.Register<IExternalVideoService, ExternalVideoService>();
            container.Register<IPasswordService, PasswordService>();
            container.Register<IPasswordRecoveryService, PasswordRecoveryService>();
            container.Register<IRecoveryLinkService>(() => new RecoveryLinkService(container.GetInstance<IStringEncryptor>()));
            container.Register<IAuthenticationService, AuthenticationService>();
            container.Register<IService<DomainNotification>, PushNotificationService>();
            container.Register<ITokenDataExtractorFactory, TokenDataExtractorFactory>();
            container.Register<ITwitterServiceService, TwitterServiceService>();
            container.Register<IFacebookService, FacebookService>();
            container.Register<ILastProjectService, LastProjectService>();
            container.Register<IPortalRoleService, PortalRoleService>();
            container.Register<IAdminProjectService, AdminProjectService>();
            container.Register<IAdminUserService, AdminUserService>();
            container.Register<IAdminClientService, AdminClientService>();
            container.Register<IBalanceService, BalanceService>();
            container.Register<IUrlTrackingStatService, UrlTrackingStatService>();
            container.Register<ICompanyService, CompanyService>();
            container.Register<IPendingClientService, PendingClientService>();
            container.Register<ISubscriptionService, SubscriptionService>();
            container.Register<ICommentService, CommentService>();
            container.Register<IAdminClientSubscriptionService, AdminClientSubscriptionService>();
            container.Register<IBillingEventHandler, BillingEventHandler>();
            container.Register<ICsvPublishServiceFactory, CsvPublishServiceFactory>();

            // BLL Billing
            container.Register<IBillingCardService>(
                () => new StripeBillingCardService(container.GetInstance<IPortalFrontendSettings>().StripeApiKey, container.GetInstance<IMapper>()));
            container.Register<IBillingChargeService>(
                () => new StripeBillingChargeService(container.GetInstance<IPortalFrontendSettings>().StripeApiKey, container.GetInstance<IMapper>()));
            container.Register<IBillingCustomerService>(
                () => new StripeBillingCustomerService(container.GetInstance<IPortalFrontendSettings>().StripeApiKey, container.GetInstance<IMapper>()));
            container.Register<IBillingEventService>(
                () => new StripeBillingEventService(container.GetInstance<IPortalFrontendSettings>().StripeApiKey, container.GetInstance<IMapper>()));
            container.Register<IBillingEventLogService, BillingEventLogService>();

            // BLL Infrastructure
            container.Register<ICryptoService, CryptoService>();
            container.Register<IFileUriProvider>(() =>
            {
                var storageAccount = container.GetInstance<CloudStorageAccount>();

                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    return new DistributedFileUriProvider(storageAccount, context.Request.Url);
                }

                return new DistributedFileUriProvider(
                    container.GetInstance<CloudStorageAccount>(),
                    new Uri(container.GetInstance<IPortalFrontendSettings>().PortalUri));
            });
            container.Register<IProjectUriProvider>(() =>
            {
                var uriProvider = new ProjectUriProvider(container.GetInstance<IPortalFrontendSettings>().PortalUri);
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    uriProvider.BaseUri = context.Request.Url;
                }
                return uriProvider;
            });
            container.Register<IUserUriProvider>(() =>
            {
                var uriProvider = new UserUriProvider(container.GetInstance<IPortalFrontendSettings>().PortalUri);
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    uriProvider.BaseUri = context.Request.Url;
                }
                return uriProvider;
            });
            container.Register<IProductIdExtractor, ProductIdExtractor>();
            container.Register<IProductWriterForAdmin, ProductWriterForAdmin>();
            container.Register<IUserAvatarProvider, UserAvatarProvider>();
            container.Register<ISocialNetworkNotificationFactory, SocialNetworkNotificationFactory>();

            // BLL Statistics
            container.Register<ICassandraStatisticsService, CassandraStatisticsService>(Lifestyle.Singleton);
            container.Register<IProjectViewsService, ProjectViewsService>(Lifestyle.Singleton);
            container.Register<IProjectLikesService, ProjectLikesService>(Lifestyle.Singleton);
            container.Register<IProjectAbuseService, ProjectAbuseService>(Lifestyle.Singleton);

            // SLL
            container.Register<IWatchProjectService, WatchProjectService>();
            container.Register<IExampleProjectService, ExampleProjectService>();

            // DAL
            container.Register(() => new MongoUrl(container.GetInstance<IPortalFrontendSettings>().MongoConnectionString));
            container.Register<IRepositoryFactory, RepositoryFactory>();
            container.Register<IFileSystem, FileSystem>();
            container.Register<IAuthenticator, Authenticator>();
            container.Register<IPasswordRecoveryFactory, PasswordRecoveryFactory>();
            container.Register<ITrackingStatRepository, TrackingStatRepository>();
            container.Register<IBalanceHistoryRepository, BalanceHistoryRepository>();
            container.Register<ICompanyRepository, CompanyRepository>();
            container.Register<IBillingEventRepository, BillingEventRepository>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IProjectRepository, ProjectRepository>();
            container.Register<IFileRepository, FileRepository>();
            container.Register<ICommentRepository, CommentRepository>();
            container.Register<IRepository<PendingClientEntity>, MongoRepository<PendingClientEntity>>();

            // DAL Statistics
            container.Register<ICassandraClient, CassandraClient>(Lifestyle.Singleton);
            container.Register<ICassandraSession, CassandraSession>(Lifestyle.Singleton);
            container.Register<IItemCountsRepository, ItemCountsRepository>(Lifestyle.Singleton);
            container.Register<IUserCountsRepository, UserCountsRepository>(Lifestyle.Singleton);
            container.Register<IItemSignalsRepository, ItemSignalsRepository>(Lifestyle.Singleton);
            container.Register<IUserSignalsRepository, UserSignalsRepository>(Lifestyle.Singleton);
            container.Register<IUserSignalsUnorderedRepository, UserSignalsUnorderedRepository>(Lifestyle.Singleton);
            container.Register<IAffinityGroupCountsRepository, AffinityGroupCountsRepository>(Lifestyle.Singleton);
            container.Register<IAffinityGroupItemCountsRepository, AffinityGroupItemCountsRepository>(Lifestyle.Singleton);
            container.Register<IAffinityGroupMostSignaledRepository, AffinityGroupMostSignaledRepository>(Lifestyle.Singleton);
            container.Register<IAffinityGroupMostSignaledVersionRepository, AffinityGroupMostSignaledVersionRepository>(Lifestyle.Singleton);
            container.Register<ITimeSeriesRawRepository, TimeSeriesRawRepository>(Lifestyle.Singleton);
            container.Register<ITimeSeriesRollupsDayRepository, TimeSeriesRollupsDayRepository>(Lifestyle.Singleton);
            container.Register<ITimeSeriesRollupsHourRepository, TimeSeriesRollupsHourRepository>(Lifestyle.Singleton);
            container.Register<ITimeSeriesRollupsMinuteRepository, TimeSeriesRollupsMinuteRepository>(Lifestyle.Singleton);

            // Media
            container.Register(() => new Lazy<Task<IMediaInfo>>(MediaInfo.Create));
            container.Register<IResolutionCalculator, ResolutionCalculator>();
            container.Register<IProcessedVideoList, ProcessedVideoList>();
            container.Register<IAdjustmentVideoMetadata, AdjustmentVideoMetadata>();
            container.Register<IAdjustmentAudioMetadata, AdjustmentAudioMetadata>();
            container.Register<IVideoMetadataParser, VideoMetadataParser>();
            container.Register<IVideoMetadataProvider, VideoMetadataProvider>();
            container.Register(() => new Lazy<IVideoMetadataProvider>(container.GetInstance<IVideoMetadataProvider>));
            container.Register<IProcessedEntityGenerator<DomainProcessedVideo>, ProcessedVideoGenerator>();
            container.Register<IProcessedEntityGenerator<DomainProcessedScreenshot>, ProcessedScreenshotGenerator>();
            container.Register<IProcessedEntityManager, ProcessedEntityManager>();
            container.Register<IComparator, Comparator>();
            container.Register<IProcessedVideoBuilder, ProcessedVideoBuilder>();
            container.Register<IScreenshotAdjusterParamFactory, ScreenshotAdjusterParamFactory>();
            container.Register<IAdjustmentScreenshotMetadata, AdjustmentScreenshotMetadata>();
            container.Register<IProcessedScreenshotBuilder, ProcessdScreenshotBuilder>();
            container.Register<IMultimediaAdjusterParamFactory, MultimediaAdjusterParamFactory>();
            container.Register<IScreenshotAdjuster, ScreenshotAdjuster>();
            container.Register<IVideoAdjuster, VideoAdjuster>();
            container.Register<IAudioAdjuster, AudioAdjuster>();

            // Email
            container.Register(() => container.GetInstance<IPortalFrontendSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();
            container.Register<IEmailNotificationService, EmailNotificationService>();

            // Aggregator
            container.Register<IStatProjectDeletionService, StatProjectDeletionService>();
            container.Register<IStatProjectUploadingService, StatProjectUploadingService>();
            container.Register<IStatUserRegistrationService, StatUserRegistrationService>();
            container.Register<IStatUserLoginService, StatUserLoginService>();
            container.Register<IStatWatchingService, StatWatchingService>();
            container.Register<IStatProjectStateService, StatProjectStateService>();
            container.Register<IActionDataService, ActionDataService>();
            container.Register<ITableValueConverter, TableValueConverter>();
            container.Register<IStatEntityFactory, StatEntityFactory>();
            container.Register<IHttpContextRepository, HttpContextRepository>();
            container.Register<IReportMapper, ReportMapper>();

            // Azure SDK
            container.Register(() => CloudStorageAccount.Parse(container.GetInstance<IPortalFrontendSettings>().DataConnectionString));
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudTableClient());
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudBlobClient());

            // Push Notifications
            container.Register<IClientNotificationHub>(() =>
            {
                var settings = container.GetInstance<IPortalFrontendSettings>();
                return new ClientNotificationHub(settings.NotificationHubConnectionString, settings.NotificationHubName);
            });

            // Report
            container.Register<IStandardReportBuilder, StandardReportBuilder>();
            container.Register<IIntervalHelper, IntervalHelper>();
            container.Register<IStandardReportService, StandardReportService>();
            container.Register<IStandardReportRepository, StandardReportRepository>();

            // TDD Staff
            container.Register<IDateTimeWrapper, DateTimeWrapper>();
            container.Register<IGuidWrapper, GuidWrapper>();
            container.Register<IFileSystemWrapper, FileSystemWrapper>();
        }
    }
}