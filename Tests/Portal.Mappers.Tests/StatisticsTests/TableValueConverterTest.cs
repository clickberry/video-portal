using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class TableValueConverterTest
    {
        [TestMethod]
        public void StringsToKeyTest()
        {
            //Arrange
            const string firstSegment = "one#two/two\\three?four";
            const string secondSegment = "five/six\\seven#eight?nine";

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result = tableValueConverter.StringsToKey(firstSegment, secondSegment);

            //Assert
            Assert.AreEqual("one_two_two_three_four:five_six_seven_eight_nine", result);
        }

        [TestMethod]
        public void ArrayToStringTest()
        {
            //Arrange
            var array = new[] {"1","2","3"};
            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result1 = tableValueConverter.ArrayToString(array);
            var result2 = tableValueConverter.ArrayToString(null);

            //Assert
            Assert.AreEqual("1;2;3", result1);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public void UserAgentToProductNameTest()
        {
            //Arrange
            const string productName="productName";
            var userAgent1 = String.Format("{0}/1.2.32 (Windows 7; x86)",productName);
            const string userAgent2 = "bla-bla";

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result1 = tableValueConverter.UserAgentToProductName(userAgent1);
            var result2 = tableValueConverter.UserAgentToProductName(userAgent2);
            var result3 = tableValueConverter.UserAgentToProductName(null);

            //Assert
            Assert.AreEqual(productName, result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [TestMethod]
        public void UserAgentToVersionTest()
        {
            //Arrange
            const string version1 = "1.2.32";
            const string version2 = "3.6";
            const string version3 = "2";
            const string userAgent = "bla-bla";
            var userAgent1 = String.Format("productName/{0} (Windows 7; x86)", version1);
            var userAgent2 = String.Format("productName/{0} (Windows 7; x86)", version2);
            var userAgent3 = String.Format("productName/{0} (Windows 7; x86)", version3);
            
            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result1 = tableValueConverter.UserAgentToVersion(userAgent1);
            var result2 = tableValueConverter.UserAgentToVersion(userAgent2);
            var result3 = tableValueConverter.UserAgentToVersion(userAgent3);
            var result4 = tableValueConverter.UserAgentToVersion(userAgent);
            var result5 = tableValueConverter.UserAgentToVersion(null);

            //Assert
            Assert.AreEqual(version1, result1);
            Assert.AreEqual(version2, result2);
            Assert.AreEqual(version3, result3);
            Assert.IsNull(result4);
            Assert.IsNull(result5);
        }

        [TestMethod]
        public void DateToPartitionKeyTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 3, 29, 14, 9, 50);

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result = tableValueConverter.DateToPartitionKey(dateTime);

            //Assert
            Assert.AreEqual("2013_03_29", result);
        }

        [TestMethod]
        public void DateTimeToTickTest()
        {
            //Arrange
            const long tick = 123456;
            var dateTime = new DateTime(tick);

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result = tableValueConverter.DateTimeToTick(dateTime);

            //Assert
            Assert.AreEqual(String.Format("{0}", DateTime.MaxValue.Ticks - tick), result);
        }

        [TestMethod]
        public void DateTimeToComparerTickTest()
        {
            //Arrange
            const long tick = 123456;
            var dateTime = new DateTime(tick);

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);
            
            //Act
            var result = tableValueConverter.DateTimeToComparerTick(dateTime);

            //Assert
            Assert.AreEqual(String.Format("{0}T", DateTime.MaxValue.Ticks - tick), result);//T for compare tick & tick with guid
        }

        [TestMethod]
        public void DateTimeToTickWithGuidTest()
        {
            //Arrange
            const long tick = 123456;
            const string newGuid = "newGuid";
            var dateTime = new DateTime(tick);
           
            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            guid.Setup(m => m.Generate()).Returns(newGuid);

            //Act
            var result = tableValueConverter.DateTimeToTickWithGuid(dateTime);

            //Assert
            Assert.AreEqual(String.Format("{0}_{1}",DateTime.MaxValue.Ticks - tick,newGuid), result);
        }

        [TestMethod]
        public void ChangeGuidPartTest()
        {
            //Arrange
            const string tickWithGuid = "tick_guid";
            const string newGuid = "newGuid";

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            guid.Setup(m => m.Generate()).Returns(newGuid);

            //Act
            var result = tableValueConverter.ChangeGuidPart(tickWithGuid);

            //Assert
           Assert.AreEqual("tick_newGuid", result);
        }

        [TestMethod]
        public void GetTickPartTest()
        {
            //Arrange
            const string tickWithGuid = "tick_guid";

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result = tableValueConverter.GetTickPart(tickWithGuid);

            //Assert
            Assert.AreEqual("tick", result);
        }

        [TestMethod]
        public void TickToDateTimeTest()
        {
            //Arrange
            const long tick = 2535457685897665455;
            const string tick1 = "tick";
            var tick2 = tick.ToString(CultureInfo.InvariantCulture);

            var guid = new Mock<IGuidWrapper>();
            var tableValueConverter = new TableValueConverter(guid.Object);

            //Act
            var result1 = tableValueConverter.TickToDateTime(tick1);
            var result2 = tableValueConverter.TickToDateTime(tick2);
            
            //Assert
            Assert.AreEqual(new DateTime(), result1);
            Assert.AreEqual(new DateTime(DateTime.MaxValue.Ticks-tick), result2);
        }
    }
}
