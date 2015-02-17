using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Standalone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Standalone
{
    [TestClass]
    public class StandaloneRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.Standalone };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var standaloneRegistrationsFilter = new StandaloneRegistrationsFilter();
            standaloneRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            standaloneRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            standaloneRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            standaloneRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.StandaloneRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.Standalone };
            var domainReport = new DomainReport();

            var standaloneRegistrationsFilter = new StandaloneRegistrationsFilter();

            //Act
            standaloneRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            standaloneRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.StandaloneRegistrations);
        }
    }
}