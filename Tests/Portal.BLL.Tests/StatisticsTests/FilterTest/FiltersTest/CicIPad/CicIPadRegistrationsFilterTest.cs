using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicIPad;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.CicIPad
{
    [TestClass]
    public class CicIPadRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.CicIPad };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var cicIPadRegistrationsFilter = new CicIPadRegistrationsFilter();
            cicIPadRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            cicIPadRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            cicIPadRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            cicIPadRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.CicIPadRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.CicIPad };
            var domainReport = new DomainReport();

            var cicIPadRegistrationsFilter = new CicIPadRegistrationsFilter();

            //Act
            cicIPadRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            cicIPadRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicIPadRegistrations);
        }
    }
}