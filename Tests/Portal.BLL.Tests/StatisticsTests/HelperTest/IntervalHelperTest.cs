using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Statistics.Helper;

namespace Portal.BLL.Tests.StatisticsTests.HelperTest
{
    [TestClass]
    public class IntervalHelperTest
    {
        [TestMethod]
        public void GetLastDayTest()
        {
            //Arrange
            var currentTime = new DateTime(2013, 01, 25, 12, 35, 10).AddTicks(12376584687); ;
            var helper = new IntervalHelper();

            //Act
            var interval = helper.GetLastDay(currentTime);

            //Assert
            Assert.AreEqual(new DateTime(2013, 01, 25, 0, 0, 0), interval.Start);
            Assert.AreEqual(new DateTime(2013, 01, 26, 0, 0, 0), interval.Finish);
        }

        [TestMethod]
        public void GetLastDateTest()
        {
            //Arrange
            var currentTime = new DateTime(2013, 01, 25, 12, 35, 10).AddTicks(12376584687);
            var helper = new IntervalHelper();

            //Act
            var yesterday = helper.GetLastDate(1, currentTime);
            var twoDayBefore = helper.GetLastDate(2, currentTime);

            //Assert
            Assert.AreEqual(new DateTime(2013, 01, 24, 0, 0, 0), yesterday);
            Assert.AreEqual(new DateTime(2013, 01, 23, 0, 0, 0), twoDayBefore);
        }

        [TestMethod]
        public void GetLastWeektest()
        {
            //Arrange
            var currentTime = new DateTime(2013, 01, 25, 12, 35, 10).AddTicks(12376584687); ;
            var helper = new IntervalHelper();

            //Act
            var interval = helper.GetLastWeek(currentTime);

            //Assert
            Assert.AreEqual(new DateTime(2013, 01, 19, 0, 0, 0), interval.Start);
            Assert.AreEqual(new DateTime(2013, 01, 26, 0, 0, 0), interval.Finish);
        }

        [TestMethod]
        public void GetLastMounthTest()
        {
            //Arrange
            var currentTime = new DateTime(2013, 01, 25, 12, 35, 10).AddTicks(12376584687); ;
            var helper = new IntervalHelper();

            //Act
            var interval = helper.GetLastMonth(currentTime);

            //Assert
            Assert.AreEqual(new DateTime(2012, 12, 27, 0, 0, 0), interval.Start);
            Assert.AreEqual(new DateTime(2013, 01, 26, 0, 0, 0), interval.Finish);
        }

        [TestMethod]
        public void GetMillisecondsToEndDayTest()
        {
            //Arrange
            var currentTime = new DateTime(2013, 01, 25, 12, 35, 10);
            var appenedTime = new TimeSpan(0, 0, 1, 0);

            var helper = new IntervalHelper();

            //Act
            var interval = helper.GetMillisecondsToEndDay(appenedTime, currentTime);

            //Assert
            Assert.AreEqual(41090000 + 60000, interval);
        }

        [TestMethod]
        public void GetAllDaysTest()
        {
            //Arrange
            var dateTime = new DateTime(2013, 02, 25, 12, 35, 10);

            var helper = new IntervalHelper();

            //Act
            var interval = helper.GetAllDays(dateTime);

            //Assert
            Assert.AreEqual(new DateTime(2013, 02, 26), interval.Finish);
            Assert.AreEqual(new DateTime(2013, 01, 31), interval.Start);
        }


        [TestMethod]
        public void GetIntervalTest()
        {
            //Arrange
            var start = new DateTime(2013, 8, 1);
            var end = new DateTime(2013, 8, 12);

            var helper = new IntervalHelper();

            //Act
            var interval = helper.GetInterval(start, end);

            //Assert
            Assert.AreEqual(start, interval.Start);
            Assert.AreEqual(new DateTime(2013, 8, 13), interval.Finish);
        }
    }
}