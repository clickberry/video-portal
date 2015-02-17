using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Portal.FileSystem.Abstract;
using Portal.FileSystem.Azure;
using Portal.FileSystem.Azure.Blob;
using Portal.FileSystem.Azure.Table;
using Portal.FileSystem.Entities;

namespace Portal.FileSystem.Tests
{
    [TestClass]
    public class StorageProviderTest
    {
        private string _fileId;
        private IStorageProvider _storageProvider;
        private string _filePath;
        private string _userId;

        [TestInitialize]
        public void Initialize()
        {
            _storageProvider = new AzureStorageProvider
                                  {
                                      BlobFileManager = new BlobFileManager(CloudStorageAccount.DevelopmentStorageAccount.CreateCloudBlobClient()),
                                      TableContext = new TableContext(CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient())
                                  };

            _userId = "UserId";
            _filePath = Path.GetTempFileName();

            using (StreamWriter fileStream = File.CreateText(_filePath))
            {
                fileStream.WriteLine("Test text");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(_filePath);
        }

        [TestMethod]
        public void TestStorageProvider()
        {
            // TestAddStorageFile

            // Act
            Task<StorageFile> addResult = _storageProvider.AddFileAsync(_userId, _filePath, "application/octet-stream");
            addResult.Wait();

            // Assert
            Assert.IsFalse(addResult.IsFaulted);
            Assert.AreNotEqual(addResult.Result.FileId, new Guid());
            Assert.AreEqual(addResult.Result.Users.Count, 0);
            Assert.AreEqual(addResult.Result.Groups.Count, 0);

            _fileId = addResult.Result.FileId;
            
            // TestExistsStorageFile

            // Act
            var existAfterAddResult = _storageProvider.ExistsFileAsync(_filePath);
            existAfterAddResult.Wait();

            // Assert
            Assert.IsFalse(existAfterAddResult.IsFaulted);
            Assert.IsTrue(existAfterAddResult.Result);
        
            // TestGetStorageFile

            // Act
            Task<StorageFile> getAfterAddResult = _storageProvider.GetByIdAsync(_fileId);
            getAfterAddResult.Wait();

            // Assert
            Assert.IsFalse(getAfterAddResult.IsFaulted);
            Assert.IsNotNull(getAfterAddResult.Result);
            Assert.AreNotEqual(getAfterAddResult.Result.FileId, new Guid());
            Assert.AreEqual(getAfterAddResult.Result.OwnerUserId, _userId);
            Assert.AreEqual(getAfterAddResult.Result.Users.Count, 0);
            Assert.AreEqual(getAfterAddResult.Result.Groups.Count, 0);
        
            // TestUpdateStorageFile

            // Arrange
            var storageFile = new StorageFile(_fileId, string.Empty, _userId){Users = new List<string> {"vasya"}};

            // Act
            var updateResult = _storageProvider.UpdateAsync(storageFile);
            updateResult.Wait();

            // Assert
            Assert.IsFalse(updateResult.IsFaulted);
        
            // TestGetStorageFileAfterUpdate

            // Act
            Task<StorageFile> getAfterUpdateResult = _storageProvider.GetByIdAsync(_fileId);
            getAfterUpdateResult.Wait();

            // Assert
            Assert.IsFalse(getAfterUpdateResult.IsFaulted);
            Assert.AreNotEqual(getAfterUpdateResult.Result.FileId, new Guid());
            Assert.IsTrue(getAfterUpdateResult.Result.Users.Contains("vasya"));
        
            // TestDeleteStorageFile

            // Arrange
            storageFile = new StorageFile(_fileId, string.Empty, _userId);

            // Act
            var deleteResult = _storageProvider.DeleteAsync(storageFile.FileId, true);
            deleteResult.Wait();

            // Assert
            Assert.IsFalse(deleteResult.IsFaulted);
        
            // TestExistsStorageFileAfterDelete

            // Act
            var existsAfterDeleteResult = _storageProvider.ExistsFileAsync(_filePath);
            existsAfterDeleteResult.Wait();

            // Assert
            Assert.IsFalse(existsAfterDeleteResult.IsFaulted);
            Assert.IsFalse(existsAfterDeleteResult.Result);
        
            // TestGetStorageFileAfterDelete

            // Act
            Task<StorageFile> getAfterDeleteResult = _storageProvider.GetByIdAsync(_fileId);
            getAfterDeleteResult.Wait();

            // Assert
            Assert.IsFalse(getAfterDeleteResult.IsFaulted);
            Assert.IsNull(getAfterDeleteResult.Result);
        }
    }
}