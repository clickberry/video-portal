using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTestInfrastructure.EncodeProcessManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Portal.Domain;
using Portal.Repository.Azure.Infrastructure;
using Portal.Repository.Azure.Queue;
using Portal.Repository.Common;
using PortalEncoder;
using PortalProcessManager;
using PortalProcessManager.Data;
using PortalProcessManager.Process;
using Wrappers;

namespace PortalProcessManagerTest
{
    [TestClass]
    public class EncodeProcessManagerTest
    {
        private string _blobDestination;
        private string _blobSource;

        [TestInitialize]
        public void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            _blobSource = ConfigurationManager.AppSettings.Get("BlobSource");
            _blobDestination = ConfigurationManager.AppSettings.Get("BlobDestination");
        }

        [TestMethod]
        public void EncodeTest()
        {
            //Arrange
            var cloudStorageConfiguration = new CloudStorageConfiguration(CloudStorageAccount.DevelopmentStorageAccount, new Version());

            var queueVideoRepository = new QueueVideoRepository(cloudStorageConfiguration.StorageAccount.CreateCloudQueueClient());
            var videoRepository = new FakeVideoRepository(_blobSource, _blobDestination);
            var screenshotRepository = new FakeScreenshotRepository(_blobDestination);
            var mediaInfoReader = new MediaInfoReader();
            var encoder = new Encoder();
            var fileSystemWrapper = new FileSystemWrapper();

            var queueProcess = new QueueProcess(1000, queueVideoRepository);
            var downloadProcess = new DownloadProcess(5, videoRepository, fileSystemWrapper);
            var encoderProcess = new EncodeProcess(5, encoder, videoRepository, mediaInfoReader, queueVideoRepository, fileSystemWrapper);
            var uploadProcess = new UploadProcess(5, videoRepository, screenshotRepository, fileSystemWrapper);
            var finishProcess = new FinishProcess(queueVideoRepository, videoRepository, fileSystemWrapper);

            var queueContainer = new ProcessContainer<object, QueueInformation, DownloadInformation>(queueProcess, downloadProcess);
            var downloadContainer = new ProcessContainer<QueueInformation, DownloadInformation, EncodeInformation>(downloadProcess, encoderProcess);
            var encoderContainer = new ProcessContainer<DownloadInformation, EncodeInformation, UploadInformation>(encoderProcess, uploadProcess);
            var uploadContainer = new ProcessContainer<EncodeInformation, UploadInformation, object>(uploadProcess, finishProcess);

            var processManager = new EncodeProcessManager(queueVideoRepository.DeleteMessageLocal);
            processManager.Add(queueContainer);
            processManager.Add(downloadContainer);
            processManager.Add(encoderContainer);
            processManager.Add(uploadContainer);

            var timer = new Timer(UpdateMessages, queueVideoRepository, (int) queueVideoRepository.InvisibleTime.TotalMilliseconds/2, (int) queueVideoRepository.InvisibleTime.TotalMilliseconds/2);

            //Act & Assert
            Task task = processManager.Start();
            StartQueueWork();

            Thread.Sleep(30000);


            while (queueVideoRepository.ApproximateMessageCount > 0)
            {
                Thread.Sleep(60000);
            }

            //Thread.Sleep(50*60*1000);

            processManager.Stop();

            task.Wait();
        }

        private void UpdateMessages(object state)
        {
            var queueVideoRepository = (QueueVideoRepository) state;
            queueVideoRepository.UpdateMessages();
        }

        private void StartQueueWork()
        {
            var cloudStorageConfiguration = new CloudStorageConfiguration(CloudStorageAccount.DevelopmentStorageAccount, new Version());
            var queueVideoRepository = new QueueVideoRepository(cloudStorageConfiguration.StorageAccount.CreateCloudQueueClient());

            string[] filePathes = Directory.GetFiles(_blobSource);

            foreach (string filePath in filePathes)
            {
                var message = new VideoMessage
                    {
                        VideoFileHash = filePath
                    };

                queueVideoRepository.AddMessage(message);
            }
        }
    }
}