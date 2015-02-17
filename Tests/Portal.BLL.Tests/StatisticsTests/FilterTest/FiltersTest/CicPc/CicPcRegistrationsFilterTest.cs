using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.CicPc;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.CicPc
{
    [TestClass]
    public class CicPcRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.CicPc };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var cicPcRegistrationsFilter = new CicPcRegistrationsFilter();
            cicPcRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            cicPcRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            cicPcRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            cicPcRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.CicPcRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.CicPc };
            var domainReport = new DomainReport();

            var cicPcRegistrationsFilter = new CicPcRegistrationsFilter();

            //Act
            cicPcRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            cicPcRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.CicPcRegistrations);
        }
    }
}