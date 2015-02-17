using System;
using System.IO;
using Portal.FileSystem.Infrastructure;
using Portal.Repository.Screenshot;

namespace IntegrationTestInfrastructure.EncodeProcessManager
{
    public class FakeScreenshotRepository : IScreenshotRepository
    {
        private readonly string _blobDestination;

        public FakeScreenshotRepository(string blobDestination)
        {
            _blobDestination = blobDestination;
        }

        #region IScreenshotRepository Members

        public void DeleteScreenshot(string userId, string projectId)
        {
        }

        public void Upload(string userId, string projectId, string localFilePath, long millisecond, string mimeType, bool isPublic)
        {
            string fileHash = FileUtils.GetFileHash(localFilePath);
            string destinationFilePath = Path.Combine(_blobDestination, fileHash, "file");
            File.Copy(localFilePath, destinationFilePath);
        }

        #endregion

        public void DeleteScreenshots(string userId, string projectId)
        {
            
        }

        public bool ExistsVideoScreenshots(string originalVideoHash)
        {
            return false;
        }

        public void UploadScreenshot(string originalVideoHash, string localFilePath, long millisecond, string mimeType)
        {
            var fileName = Path.GetFileNameWithoutExtension(originalVideoHash);
            var newFileName = "{0}_{1}.jpg";
            newFileName = String.Format(newFileName, fileName, millisecond);
            var destinationFilePath = Path.Combine(_blobDestination, newFileName);

            File.Copy(localFilePath, destinationFilePath);
        }

        public void AddScreenshots(string userId, string projectId, string originalVideoHash, bool isPublic)
        {
            
        }
    }
}