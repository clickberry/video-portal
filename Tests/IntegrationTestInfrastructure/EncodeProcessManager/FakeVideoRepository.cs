using System;
using System.Collections.Generic;
using System.IO;
using Portal.Domain;
using Portal.FileSystem.Infrastructure;
using Portal.Repository.Azure.Infrastructure;
using Portal.Repository.Video;
using File = System.IO.File;

namespace IntegrationTestInfrastructure.EncodeProcessManager
{
    public class FakeVideoRepository : IVideoRepository
    {
        private readonly string _blobDestination;
        private readonly string _blobSource;

        public FakeVideoRepository(string blobSource, string blobDestination)
        {
            _blobSource = blobSource;
            _blobDestination = blobDestination;
        }

        #region IVideoRepository Members

        public VideoMediaInfo GetVideoMediaInfo(IDictionary<Enum, object> parameters)
        {
            return parameters.GetVideoMediaInfo();
        }

        public void FillMediaInfoTables(IDictionary<Enum, object> parameters, string videoId)
        {
        }

        public void DeleteWatchVideos(string userId, string projectId)
        {
        }

        public void DownloadOriginalVideo(string originalVideoHash, string localFilePath)
        {
            File.Copy(originalVideoHash, localFilePath);
        }

        public bool ExistsEncodedVideo(string originalVideoHash)
        {
            return false;
        }
        
        public void AddWatchVideos(string userId, string projectId, string originalVideoHash, bool isPublic)
        {
            
        }

        public void UploadEncodedVideo(string originalVideoHash, string localFilePath, string mediaContainer, string videoCodec, string audioCodec, int frameWidth, int frameHeight, string mimeType, DateTime startEncode, DateTime finishEncode)
        {
            var fileName = Path.GetFileNameWithoutExtension(originalVideoHash);
            var newFileName = "{0}_{1}.{2}";
            newFileName = String.Format(newFileName, fileName, frameHeight, mediaContainer);
            var destinationFilePath = Path.Combine(_blobDestination, newFileName);

            File.Copy(localFilePath, destinationFilePath);
        }

        public void SetEncodingState(string projectId, EncodingState state, EncodingStage stage, string errorMessage = null)
        {
            
        }
        
        #endregion
    }
}