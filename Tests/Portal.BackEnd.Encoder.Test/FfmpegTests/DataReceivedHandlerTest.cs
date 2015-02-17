using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;

namespace Portal.BackEnd.Encoder.Test.FfmpegTests
{
    [TestClass]
    public class DataReceivedHandlerTest
    {
        [TestMethod]
        public void ProcessDataTest()
        {
            //Arrange
            const string data1 = "data1";
            const string data2 = "data1";
            const double duration = 123.34;
            const double encodeTime = 12.334;

            var parser = new Mock<IFfmpegParser>();
            var handler = new DataReceivedHandler(parser.Object);
            var percent = 0;
            var action=new Action<int>((i)=>percent=i);

            handler.Register(action);

            parser.Setup(m => m.ParseDuration(data1)).Returns(duration);
            parser.Setup(m => m.ParseEncodeTime(data2)).Returns(encodeTime);
            
            //Act
            handler.ProcessData(data1);
            handler.ProcessData(data2);

            //Assert
            Assert.AreEqual(10, percent);
        }

        [TestMethod]
        public void InvokeRegisteredActionWithoutParametersTest()
        {
            //Arrange
            var parser = new Mock<IFfmpegParser>();
            var handler = new DataReceivedHandler(parser.Object);
            var wasInvoked = false;

            Action action = () => { wasInvoked = true; };

            handler.Register(action);

            //Act
            handler.ProcessData("data");

            //Assert
            Assert.IsTrue(wasInvoked);
        }
    }
}
