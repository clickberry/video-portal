using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Test.FfmpegTests
{
    [TestClass]
    public class VideoFfmpegParserTest
    {
        [TestMethod]
        public void ParseDurationTest()
        {
            //Arrange
            const int hour =3;
            const int min = 54;
            const double sec = 23;
            const string ffmpegDurationTime = "  Duration: {0}:{1}:{2}, start: 0.000000, bitrate: 891 kb/s";

            var str = String.Format(ffmpegDurationTime, hour.ToString("d2"), min.ToString("d2"), sec.ToString("f2"));
            var parser = new VideoFfmpegParser();

            //Act
            var result = parser.ParseDuration(str);

            //Assert
            Assert.AreEqual(hour * 3600 + min * 60 + sec, result);
        }

        [TestMethod]
        public void ParseDurationWithIncorrectStringTest()
        {
            //Arrange
            const string str = "incorrect string";

            var parser = new VideoFfmpegParser();

            //Act
            var result = parser.ParseDuration(str);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseDurationWithNullStringTest()
        {
            //Arrange
            var parser = new VideoFfmpegParser();

            //Act
            var result = parser.ParseDuration(null);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseEncodeTimeTest()
        {
            //Arrange
            const int hour=1;
            const int min = 34;
            const double sec = 23.54;
            const string ffmpegEncodeTime = "frame=  702 fps= 11 q=0.0 size=     775kB time={0}:{1}:{2} bitrate= 234.1kbits/s";

            var str = String.Format(ffmpegEncodeTime, hour.ToString("d2"), min.ToString("d2"), sec.ToString("f2"));
            var parser = new VideoFfmpegParser();

            //Act
            var result = parser.ParseEncodeTime(str);

            //Assert
            Assert.AreEqual(hour*3600+min*60+sec,result);
        }

        [TestMethod]
        public void ParseEncodeTimeWithIncorrectStringTest()
        {
            //Arrange
            const string str = "incorrect string";

            var parser = new VideoFfmpegParser();

            //Act
            var result = parser.ParseEncodeTime(str);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseEncodeTimeWithNullStringTest()
        {
            //Arrange
            var parser = new VideoFfmpegParser();

            //Act
            var result = parser.ParseEncodeTime(null);

            //Assert
            Assert.AreEqual(0, result);
        }
    }
}
