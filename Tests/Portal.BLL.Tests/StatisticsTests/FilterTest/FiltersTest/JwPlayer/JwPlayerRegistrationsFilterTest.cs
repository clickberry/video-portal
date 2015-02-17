using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.JwPlayer;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.JwPlayer
{
    [TestClass]
    public class JwPlayerRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.JwPlayer };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var jwPlayerRegistrationsFilter = new JwPlayerRegistrationsFilter();
            jwPlayerRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            jwPlayerRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            jwPlayerRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            jwPlayerRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.JwPlayerRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.JwPlayer };
            var domainReport = new DomainReport();

            var jwPlayerRegistrationsFilter = new JwPlayerRegistrationsFilter();

            //Act
            jwPlayerRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            jwPlayerRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.JwPlayerRegistrations);
        }
    }
}