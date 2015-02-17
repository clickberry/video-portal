using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Api.Infrastructure.MediaFormatters;
using Portal.Api.Models;

namespace Portal.WebApi.Tests.Infrastructure
{
    /// <summary>
    /// Summary description for DictionaryExtensionsTest
    /// </summary>
    [TestClass]
    public class InfrastructureTest
    {
        private static readonly string Body = string.Format(
            @"------WebKitFormBoundaryAIv1HQdnxqjt9a5y{0}" +
            @"Content-Disposition: form-data; name=""Name""{0}{0}" +
            @"My cool project{0}" +
            @"------WebKitFormBoundaryAIv1HQdnxqjt9a5y{0}" +
            @"Content-Disposition: form-data; name=""Video""; filename=""video.mp4""{0}" +
            @"Content-Type: video/mp4{0}{0}" +
            @"my_video{0}" +
            @"------WebKitFormBoundaryAIv1HQdnxqjt9a5y{0}" +
            @"Content-Disposition: form-data; name=""Data""; filename=""project.avsx""{0}" +
            @"Content-Type: application/octet-stream{0}{0}" +
            @"avsx_project{0}" +
            @"------WebKitFormBoundaryAIv1HQdnxqjt9a5y--{0}",
            Environment.NewLine);

        [TestMethod]
        public void TestHttpMultipartFormHandler()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Body));
            HttpContent httpContent = new StreamContent(stream);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "----WebKitFormBoundaryAIv1HQdnxqjt9a5y"));
            httpContent.Headers.ContentLength = 465;

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Content = httpContent;

            // For returning non-async stuff, use a TaskCompletionSource to avoid thread switches
            // Save file
            var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());

            var task = requestMock.Object.Content.ReadAsMultipartAsync(provider);
            task.Wait();

            // Act
            IDictionary<string, string> result = HttpMultipartForm.HandleMultipartBody(provider);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ContainsKey("Name"));
            Assert.IsTrue(result.ContainsKey("Data"));
            Assert.IsTrue(result.ContainsKey("Video"));
            Assert.IsTrue(result.ContainsKey("DataUri"));
            Assert.IsTrue(result.ContainsKey("DataContentType"));
            Assert.IsTrue(result.ContainsKey("VideoUri"));
            Assert.IsTrue(result.ContainsKey("VideoContentType"));
            Assert.IsFalse(string.IsNullOrEmpty(result["Name"]));
            Assert.IsFalse(string.IsNullOrEmpty(result["Data"]));
            Assert.IsFalse(string.IsNullOrEmpty(result["Video"]));
            Assert.IsFalse(string.IsNullOrEmpty(result["DataUri"]));
            Assert.IsFalse(string.IsNullOrEmpty(result["DataContentType"]));
            Assert.IsFalse(string.IsNullOrEmpty(result["VideoUri"]));
            Assert.IsFalse(string.IsNullOrEmpty(result["VideoContentType"]));
            Assert.IsTrue(File.Exists(result["DataUri"]));
            Assert.IsTrue(File.Exists(result["VideoUri"]));
            File.Delete(result["DataUri"]);
            File.Delete(result["VideoUri"]);
        }

        [TestMethod]
        public void TestProjectMultipartFormFormatter()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Body));

            HttpContent httpContent = new StreamContent(stream);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "----WebKitFormBoundaryAIv1HQdnxqjt9a5y"));
            httpContent.Headers.ContentLength = 465;

            var formatter = new MultipartFormFormatter();
            Task<object> task = null;
            Exception exception = null;

            // Act
            try
            {
                task = formatter.ReadFromStreamAsync(typeof (ProjectPostModel), stream, httpContent, null);
                task.Wait();
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.IsNull(exception);
            Assert.IsNotNull(task);
            Assert.IsFalse(task.IsCanceled || task.IsFaulted);
            Assert.IsNotNull(task.Result);
            Assert.IsInstanceOfType(task.Result, typeof (ProjectPostModel));
            Assert.IsTrue(formatter.CanReadType(typeof (ProjectPostModel)));
            Assert.IsFalse(formatter.CanWriteType(typeof (ProjectPostModel)));

            var project = (ProjectPostModel) task.Result;
            Assert.AreEqual("My cool project", project.Name);
            Assert.AreEqual("video.mp4", project.Video);
            Assert.AreEqual("project.avsx", project.Data);
            Assert.IsNull(project.Public);
            Assert.IsTrue(File.Exists(project.DataUri));
            Assert.IsTrue(File.Exists(project.VideoUri));

            File.Delete(project.DataUri);
            File.Delete(project.VideoUri);
        }
    }
}