using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Registrations;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Registrations
{
    [TestClass]
    public class AllRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var allRegistrationsFilter = new AllRegistrationsFilter();
            allRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            allRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.AllRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration();
            var domainReport = new DomainReport();

            var allRegistrationsFilter = new AllRegistrationsFilter();

            //Act
            allRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            allRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.AllRegistrations);
        }
    }
}