using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Registrations;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Registrations
{
    [TestClass]
    public class BrowserRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.Mozilla };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var browserRegistrationsFilter = new BrowserRegistrationsFilter();
            browserRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            browserRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            browserRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            browserRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.BrowserRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.Mozilla };
            var domainReport = new DomainReport();

            var browserRegistrationsFilter = new BrowserRegistrationsFilter();

            //Act
            browserRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            browserRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.BrowserRegistrations);
        }
    }
}