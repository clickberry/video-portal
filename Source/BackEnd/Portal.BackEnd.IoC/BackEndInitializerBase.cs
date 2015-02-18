// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading;
using Configuration;
using Configuration.Azure.Concrete;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using Portal.BackEnd.Encoder;
using Portal.BackEnd.Encoder.Factory;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.LocalFileSystem;
using Portal.BackEnd.Encoder.MiddleEndClient;
using Portal.BackEnd.Encoder.Pipeline;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.BLL.Concrete.Infrastructure;
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
using Portal.DAL.Mailer;
using Portal.DAL.Project;
using Portal.DAL.User;
using Portal.Mappers;
using RestSharp;
using RestSharp.Deserializers;
using SimpleInjector;
using Wrappers.Implementation;
using Wrappers.Interface;

namespace Portal.BackEnd.IoC
{
    public abstract class BackEndInitializerBase
    {
        public abstract void Initialize(Container container);

        protected void Initialize(Container container, IRestClient restClient)
        {
            // Configuration provider
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IPortalBackendSettings, PortalBackendSettings>();
            container.Register<IPortalSettings, PortalSettings>();
            container.RegisterSingle<IMapper, PortalMapper>();

            //Settings

            container.Register(() => restClient);
            container.RegisterSingle(() => new CancellationTokenSource());
            container.Register<IFileSystemWrapper, FileSystemWrapper>();
            container.Register<IFileWrapper, FileWrapper>();
            container.Register<ITempFileManager, TempFileManager>();

            // Azure SDK
            container.Register(() => CloudStorageAccount.Parse(container.GetInstance<IPortalBackendSettings>().DataConnectionString).CreateCloudTableClient());
            container.Register(() => CloudStorageAccount.Parse(container.GetInstance<IPortalBackendSettings>().DataConnectionString).CreateCloudBlobClient());

            container.Register(() => new MongoUrl(container.GetInstance<IPortalBackendSettings>().MongoConnectionString));
            container.Register<IUserRepository, UserRepository>();
            container.Register<IProjectRepository, ProjectRepository>();
            container.Register<IFileRepository, FileRepository>();
            container.Register<IRepositoryFactory, RepositoryFactory>();
            container.Register<IFileSystem, FileSystem>();

            // Email
            container.Register(() => container.GetInstance<IPortalBackendSettings>().MailSettings);
            container.Register<IEmailInitializer, EmailInitializer>();
            container.Register<IEmailFactory, SmtpEmailFactory>();
            container.Register<IMailerRepository, MailerRepository>();
            container.Register<IStringEncryptor, StringEncryptor>();
            container.Register<IEmailSenderService, EmailSenderService>();

            //Factory
            container.Register<IEncodeCreatorFactory, EncodeCreatorFactory>();
            container.Register<ITokenSourceFactory, TokenSourceFactory>();

            //Ffmpeg
            container.Register<IFfmpeg, FfmpegProcess>();
            container.Register<IProcessAsync, ProcessAsync>();

            //MiddleEndClient
            container.Register<IDeserializer, JsonDeserializer>();
            container.Register<IEncodeDeserializer, EncodeDeserializer>();
            container.RegisterSingle<IEncodeWebClient, EncodeWebClient>();
            container.Register<IRestHelper, RestHelper>();

            //PipelineStep
            container.Register<GetTaskStep>();
            container.Register<CreatorStep>();
            container.Register<EncodeStep>();
            container.Register<FinishStep>();
            container.Register<DownloadStep>();

            //Pipeline
            container.Register<IPipelineStrategy, PipelineStrategy>();
            container.Register<IEncodePipeline, EncodePipeline>();

            container.RegisterSingle<IStepMediator, StepMediator>();

            container.Register<IWatchDogTimer, WatchDogTimer>();
            container.Register<EncodeManager>();

            //Wrappers
            container.Register<IDateTimeWrapper, DateTimeWrapper>();

            // Initializers
            container.Register<TraceListenersInitializer>();
        }
    }
}