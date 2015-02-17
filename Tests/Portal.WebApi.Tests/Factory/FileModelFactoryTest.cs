using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Api.Factory;
using Portal.Api.Models;

namespace Portal.WebApi.Tests.Factory
{
    [TestClass]
    public class FileModelFactoryTest
    {
        private const string Name = "my name";
        private const string FileName = "video.mp4";
        private const string FileUri = @"c:\video.mp4";

        [TestMethod]
        public void TestProjectPostModelConstruction()
        {
            // Arrange
            var values = new Dictionary<string, string>
                             {
                                 {"name", Name},
                                 {"file", FileName},
                                 {"fileUri", FileUri},
                             };

            // Act
            var result = FileModelFactory.CreatePostModel(values);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (FilePostModel));
            Assert.AreEqual(result.Name, Name);
            Assert.AreEqual(result.File, FileName);
            Assert.AreEqual(result.FileUri, FileUri);
        }

        [TestMethod]
        public void TestProjectPutModelConstruction()
        {
            // Arrange
            var values = new Dictionary<string, string>
                             {
                                 {"name", Name},
                                 {"file", FileName},
                                 {"fileUri", FileUri},
                             };

            // Act
            var result = FileModelFactory.CreatePutModel(values);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (FilePutModel));
            Assert.AreEqual(result.Name, Name);
            Assert.AreEqual(result.File, FileName);
            Assert.AreEqual(result.FileUri, FileUri);
        }
    }
}