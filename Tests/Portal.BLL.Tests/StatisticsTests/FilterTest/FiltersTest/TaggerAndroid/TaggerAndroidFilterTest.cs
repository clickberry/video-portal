using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerAndroid;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.TaggerAndroid
{
    [TestClass]
    public class TaggerAndroidRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.TaggerAndroid };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var taggerAndroidRegistrationsFilter = new TaggerAndroidRegistrationsFilter();
            taggerAndroidRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            taggerAndroidRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            taggerAndroidRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            taggerAndroidRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.TaggerAndroidRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.TaggerAndroid };
            var domainReport = new DomainReport();

            var taggerAndroidRegistrationsFilter = new TaggerAndroidRegistrationsFilter();

            //Act
            taggerAndroidRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            taggerAndroidRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.TaggerAndroidRegistrations);
        }
    }
}