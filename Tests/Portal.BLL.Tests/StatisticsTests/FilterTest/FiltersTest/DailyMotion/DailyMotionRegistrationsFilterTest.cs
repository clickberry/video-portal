using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.DailyMotion;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.DailyMotion
{
    [TestClass]
    public class DailyMotionRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.DailyMotion };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var dailyMotionRegistrationsFilter = new DailyMotionRegistrationsFilter();
            dailyMotionRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            dailyMotionRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            dailyMotionRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            dailyMotionRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.DailyMotionRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.DailyMotion };
            var domainReport = new DomainReport();

            var dailyMotionRegistrationsFilter = new DailyMotionRegistrationsFilter();

            //Act
            dailyMotionRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            dailyMotionRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.DailyMotionRegistrations);
        }
    }
}