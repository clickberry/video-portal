using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Portal.DAL.Entities.Storage;
using Portal.DAL.FileSystem;
using Portal.Domain.FileContext;

namespace Portal.BackEnd.Encoder.Test.IntegrationTests.Infrastructure
{
    public class FakeFileSystem: IFileSystem
    {
        public Task<DomainUserFile> AddFileAsync(string userId, string localFileUri, string contentType)
        {
            var tcs = new TaskCompletionSource<DomainUserFile>();
            var destinationPath = ConfigurationManager.AppSettings.Get("DestinationPath");
            var fileExtension = contentType.Split('/')[1];
            var fileName = String.Format("file.{0}", fileExtension);
            var filePath = Path.Combine(destinationPath, fileName);
            File.Copy(localFileUri, filePath, true);
            tcs.SetResult(new DomainUserFile());

            return tcs.Task;
        }

        public Task<DomainUserFile> AddFileAsync(string userId, string fileHash)
        {
            throw new NotImplementedException();
        }

        public Task<DomainUserFile> GetByIdAsync(string fileId)
        {
            throw new NotImplementedException();
        }

        public Task DownloadFileByIdAsync(string fileId, string localFileUri)
        {
            var tcs = new TaskCompletionSource<object>();
            File.Copy(fileId, localFileUri, true);
            tcs.SetResult(null);

            return tcs.Task;
        }

        public Task<StorageFile> AddArtifactByHashAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<StorageFile> AddFileByHashAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<StorageFile> UploadArtifactFromStreamAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<StorageFile> UploadFileFromStreamAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<StorageFile> DownloadFileToStreamAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<StorageFile> GetFilePropertiesAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteFileAsync(StorageFile file, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}