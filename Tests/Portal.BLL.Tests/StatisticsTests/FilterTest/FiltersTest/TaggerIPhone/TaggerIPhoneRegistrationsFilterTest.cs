using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerIPhone
{
    [TestClass]
    public class TaggerIPhoneRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.TaggerIPhone };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var taggerIPhoneRegistrationsFilter = new TaggerIPhoneRegistrationsFilter();
            taggerIPhoneRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            taggerIPhoneRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            taggerIPhoneRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            taggerIPhoneRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TaggerIPhoneRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.TaggerIPhone };
            var domainReport = new DomainReport();

            var taggerIPhoneRegistrationsFilter = new TaggerIPhoneRegistrationsFilter();

            //Act
            taggerIPhoneRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            taggerIPhoneRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerIPhoneRegistrations);
        }
    }
}