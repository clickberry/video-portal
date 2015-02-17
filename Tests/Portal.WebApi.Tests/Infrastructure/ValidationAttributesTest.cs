using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Asp.Infrastructure.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Api.Models;

namespace Portal.WebApi.Tests.Infrastructure
{
    [TestClass]
    public class ValidationAttributesTest
    {
        [TestMethod]
        public void VideoValidationAttributeTest()
        {
            // Arrange
            var video = new List<string>();
            var other = new List<string>();

            // Act
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                try
                {
                    var attribute = new VideoFileAttribute("VideoUri");
                    attribute.Validate(file, new ValidationContext(new ProjectPostModel {VideoUri = file}, null, null));
                    video.Add(file);
                }
                catch
                {
                    other.Add(file);
                }
            }

            // Assert
            Assert.AreEqual(video.Count, 1);
        }

        [TestMethod]
        public void FileValidationAttributeTest()
        {
            // Arrange
            var images = new List<string>();
            var other = new List<string>();

            // Act
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                try
                {
                    var attribute = new FileFormatAttribute {FileUriProperty = "FileUri", FormatProperty = "Format"};
                    attribute.Validate(file, new ValidationContext(new FilePostModel {FileUri = file}, null, null));
                    images.Add(file);
                }
                catch
                {
                    other.Add(file);
                }
            }

            // Assert
            Assert.AreEqual(images.Count, 1);
        }
    }
}