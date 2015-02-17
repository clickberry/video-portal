using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Api.Factory;
using Portal.Api.Models;

namespace Portal.WebApi.Tests.Factory
{
    [TestClass]
    public class ProductModelFactoryTest
    {
        private const string ProjectName = "my name";
        private const string ProjectDescription = "my description";
        private const string VideoName = "video.mp4";
        private const string VideoUri = @"c:\video.mp4";
        private const string DataName = "project.avsx";
        private const string DataUri = @"c:\project.avsx";
        private const string Public = @"true";

        [TestMethod]
        public void TestProjectPostModelConstruction()
        {
            // Arrange
            var values = new Dictionary<string, string>
                             {
                                 {"name", ProjectName},
                                 {"description", ProjectDescription},
                                 {"data", DataName},
                                 {"dataUri", DataUri},
                                 {"video", VideoName},
                                 {"videoUri", VideoUri},
                                 {"public", Public}
                             };

            // Act
            ProjectPostModel result = ProjectModelFactory.CreatePostModel(values);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ProjectPostModel));
            Assert.AreEqual(result.Name, ProjectName);
            Assert.AreEqual(result.Description, ProjectDescription);
            Assert.AreEqual(result.Data, DataName);
            Assert.AreEqual(result.DataUri, DataUri);
            Assert.AreEqual(result.Video, VideoName);
            Assert.AreEqual(result.VideoUri, VideoUri);
            Assert.AreEqual(result.Public, "true");
        }

        [TestMethod]
        public void TestProjectPutModelConstruction()
        {
            // Arrange
            var values = new Dictionary<string, string>
                             {
                                 {"name", ProjectName},
                                 {"description", ProjectDescription},
                                 {"data", DataName},
                                 {"dataUri", DataUri},
                                 {"video", VideoName},
                                 {"videoUri", VideoUri},
                                 {"public", Public}
                             };

            // Act
            ProjectPutModel result = ProjectModelFactory.CreatePutModel(values);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ProjectPutModel));
            Assert.AreEqual(result.Name, ProjectName);
            Assert.AreEqual(result.Description, ProjectDescription);
            Assert.AreEqual(result.Data, DataName);
            Assert.AreEqual(result.DataUri, DataUri);
            Assert.AreEqual(result.Video, VideoName);
            Assert.AreEqual(result.VideoUri, VideoUri);
            Assert.AreEqual(result.Public, Public);
        }
    }
}