using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Providers;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.ProfileContext;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Providers
{
    [TestClass]
    public class OdnoklassnikiRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { IdentityProvider = IdentityType.Odnoklassniki.ToString() };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { IdentityProvider = "IdentityProvider" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var odnoklassnikiRegistrationsFilter = new OdnoklassnikiRegistrationsFilter();
            odnoklassnikiRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            odnoklassnikiRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            odnoklassnikiRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            odnoklassnikiRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.OdnoklassnikiRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { IdentityProvider = IdentityType.Odnoklassniki.ToString() };
            var domainReport = new DomainReport();

            var odnoklassnikiRegistrationsFilter = new OdnoklassnikiRegistrationsFilter();

            //Act
            odnoklassnikiRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            odnoklassnikiRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.OdnoklassnikiRegistrations);
        }
    }
}