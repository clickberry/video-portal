using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Providers;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.ProfileContext;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Providers
{
    [TestClass]
    public class WindowsLiveRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { IdentityProvider = IdentityType.WindowsLive.ToString() };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { IdentityProvider = "IdentityProvider" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var windowsLiveRegistrationsFilter = new WindowsLiveRegistrationsFilter();
            windowsLiveRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            windowsLiveRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            windowsLiveRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            windowsLiveRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.WindowsLiveRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { IdentityProvider = IdentityType.WindowsLive.ToString() };
            var domainReport = new DomainReport();

            var windowsLiveRegistrationsFilter = new WindowsLiveRegistrationsFilter();

            //Act
            windowsLiveRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            windowsLiveRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.WindowsLiveRegistrations);
        }
    }
}