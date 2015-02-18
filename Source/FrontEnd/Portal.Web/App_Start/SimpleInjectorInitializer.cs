// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Asp.Infrastructure.Extensions;
using Asp.Infrastructure.Filters;
using Authentication;
using Configuration;
using Configuration.Azure.Concrete;
using MediaInfoLibrary;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using MongoRepository;
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
using Portal.BLL.Concrete.Subscriptions;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Statistics;
using Portal.BLL.Statistics.Aggregator;
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
using Portal.DAL.Infrastructure.Analytics;
using Portal.DAL.Infrastructure.Authentication;
using Portal.DAL.Infrastructure.Context;
using Portal.DAL.Infrastructure.Factories;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Infrastructure.Multimedia;
using Portal.DAL.Mailer;
using Portal.DAL.Multimedia;
using Portal.DAL.Project;
using Portal.DAL.Statistics;
using Portal.DAL.Subscriptions;
using Portal.DAL.User;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProjectContext;
using Portal.Mappers;
using Portal.Mappers.Statistics;
using Portal.Web;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using Wrappers.Implementation;
using Wrappers.Interface;

[assembly: PreApplicationStartMethod(typeof (SimpleInjectorInitializer), "Initialize")]

namespace Portal.Web
{
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.PropertySelectionBehavior = new ImportPropertySelectionBehavior();

            Register(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();

            try
            {
                container.Verify();
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to initialize IoC container: {0}", e);
                throw;
            }

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

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
            container.RegisterSingle<IAnalytics>(() => new GoogleAnalytics(container.GetInstance<IPortalFrontendSettings>().GoogleAnalyticsId));

            // BLL
            container.Register<IProcessedVideoHandler, ProcessedVideoHandler>();
            container.Register<IService<DomainProjectProcessedScreenshot>, ProjectProcessedScreenshotService>();
            container.Register<IService<DomainProjectProcessedVideo>, ProjectProcessedVideoService>();
            container.Register(() => new Lazy<IService<DomainProjectProcessedVideo>>(container.GetInstance<IService<DomainProjectProcessedVideo>>));
            container.Register<IProjectAvsxService, ProjectAvsxService>();
            container.Register<IProjectScreenshotService, ProjectScreenshotService>();
            container.Register<IProjectVideoService, ProjectVideoService>();
            container.Register<IProjectService, ProjectService>();
            container.Register<IUserService, UserService>();
            container.Register<IAdminUserService, AdminUserService>();
            container.Register<IExternalVideoService, ExternalVideoService>();
            container.Register<IPasswordService, PasswordService>();
            container.Register<IPasswordRecoveryService, PasswordRecoveryService>();
            container.Register<IRecoveryLinkService, RecoveryLinkService>();
            container.Register<IAuthenticationService, AuthenticationService>();
            container.Register<ILastProjectService, LastProjectService>();
            container.Register<IProcessedEntityManager, ProcessedEntityManager>();
            container.Register<IProductIdExtractor, ProductIdExtractor>();
            container.Register<IPendingClientService, PendingClientService>();
            container.Register<ICompanyService, CompanyService>();

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
            container.Register<IUserAvatarProvider, UserAvatarProvider>();
            container.Register<IProductWriterForAdmin, ProductWriterForAdmin>();
            container.Register<IUserAgentVerifier, UserAgentVerifier>();
            container.Register<ITokenDataExtractorFactory, TokenDataExtractorFactory>();
            container.Register<ISocialNetworkNotificationFactory, SocialNetworkNotificationFactory>();

            // BLL Statistics
            container.Register<ICassandraStatisticsService, CassandraStatisticsService>(Lifestyle.Singleton);
            container.Register<IProjectLikesService, ProjectLikesService>(Lifestyle.Singleton);

            // SLL
            container.Register<IWatchProjectService, WatchProjectService>();
            container.Register<IExampleProjectService, ExampleProjectService>();

            // DAL
            container.Register(() => new MongoUrl(container.GetInstance<IPortalFrontendSettings>().MongoConnectionString));
            container.Register<IRepositoryFactory, RepositoryFactory>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IProjectRepository, ProjectRepository>();
            container.Register<IFileRepository, FileRepository>();
            container.Register<IFileSystem, FileSystem>();
            container.Register<IAuthenticator, Authenticator>();
            container.Register<IPasswordRecoveryFactory, PasswordRecoveryFactory>();
            container.Register<ICommentRepository, CommentRepository>();
            container.Register<IRepository<PendingClientEntity>, MongoRepository<PendingClientEntity>>();
            container.Register<IBalanceHistoryRepository, BalanceHistoryRepository>();
            container.Register<ICompanyRepository, CompanyRepository>();
            container.Register<IBillingEventRepository, BillingEventRepository>();

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
            container.Register<IActionDataService, ActionDataService>();
            container.Register<ITableValueConverter, TableValueConverter>();
            container.Register<IStatEntityFactory, StatEntityFactory>();
            container.Register<IHttpContextRepository, HttpContextRepository>();
            container.Register<IReportMapper, ReportMapper>();

            // Azure SDK
            container.Register(() => CloudStorageAccount.Parse(container.GetInstance<IPortalFrontendSettings>().DataConnectionString));
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudTableClient());
            container.Register(() => container.GetInstance<CloudStorageAccount>().CreateCloudBlobClient());

            // Custom Identity Providers
            container.RegisterSingle<IIdentityFactory, IdentityFactory>();

            // TDD staff
            container.Register<IDateTimeWrapper, DateTimeWrapper>();
            container.Register<IGuidWrapper, GuidWrapper>();

            CdnResources.SetConfigurationProvider(new PortalFrontendSettings(new ConfigurationProvider()));
        }
    }
}