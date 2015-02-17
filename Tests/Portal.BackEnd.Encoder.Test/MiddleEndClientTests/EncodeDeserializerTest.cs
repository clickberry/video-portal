using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Data;
using Portal.BackEnd.Encoder.MiddleEndClient;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.BackendContext.Enum;
using RestSharp;
using RestSharp.Deserializers;

namespace Portal.BackEnd.Encoder.Test.MiddleEndClientTests
{
    [TestClass]
    public class EncodeDeserializerTest
    {
        [TestMethod]
        public void VideoDataDeserializeTest()
        {
            //Arrange
            const TypeOfTask typeOfType = TypeOfTask.Video;

            var deserializer = new Mock<IDeserializer>();
            var response = new Mock<IRestResponse>();
            var encodeDeserializer = new EncodeDeserializer(deserializer.Object);

            var data = new VideoEncodeData();

            deserializer.Setup(m => m.Deserialize<VideoEncodeData>(response.Object)).Returns(data);

            //Act 
            var encodeData = encodeDeserializer.EncodeDataDeserialize(response.Object, typeOfType);

            //Assert
            Assert.AreEqual(data, encodeData);
        }

        [TestMethod]
        public void ScreenshotDataDeserializeTest()
        {
            //Arrange
            const TypeOfTask typeOfType = TypeOfTask.Screenshot;

            var deserializer = new Mock<IDeserializer>();
            var response = new Mock<IRestResponse>();
            var encodeDeserializer = new EncodeDeserializer(deserializer.Object);

            var data = new ScreenshotEncodeData();

            deserializer.Setup(m => m.Deserialize<ScreenshotEncodeData>(response.Object)).Returns(data);

            //Act 
            var encodeData = encodeDeserializer.EncodeDataDeserialize(response.Object, typeOfType);

            //Assert
            Assert.AreEqual(data,encodeData);
        }

        [TestMethod]
        public void TaskDataDeserealizeTest()
        {
            //Arrange
            var deserializer = new Mock<IDeserializer>();
            var response = new Mock<IRestResponse>();
            var encodeDeserializer = new EncodeDeserializer(deserializer.Object);

            var data = new EncodeTaskData();

            deserializer.Setup(m => m.Deserialize<EncodeTaskData>(response.Object)).Returns(data);

            //Act 
            var encodeTaskData = encodeDeserializer.EncodeTaskDataDeserealize(response.Object);

            //Assert
            Assert.AreEqual(data, encodeTaskData);
        }
    }
}