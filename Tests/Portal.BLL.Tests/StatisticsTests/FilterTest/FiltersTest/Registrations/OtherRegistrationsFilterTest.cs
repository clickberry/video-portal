using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Registrations;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Registrations
{
    [TestClass]
    public class OtherRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.CicIPad };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = ProductName.CicMac };
            var domainStatUserRegistration3 = new DomainStatUserRegistration() { ProductName = ProductName.CicPc };
            var domainStatUserRegistration4 = new DomainStatUserRegistration() { ProductName = ProductName.ImageShack };
            var domainStatUserRegistration5 = new DomainStatUserRegistration() { ProductName = ProductName.Mozilla };
            var domainStatUserRegistration6 = new DomainStatUserRegistration() { ProductName = ProductName.Player };
            var domainStatUserRegistration7 = new DomainStatUserRegistration() { ProductName = ProductName.Standalone };
            var domainStatUserRegistration8 = new DomainStatUserRegistration() { ProductName = ProductName.TaggerAndroid };
            var domainStatUserRegistration9 = new DomainStatUserRegistration() { ProductName = ProductName.TaggerIPhone };
            var domainStatUserRegistration10 = new DomainStatUserRegistration() { ProductName = ProductName.DailyMotion };
            var domainStatUserRegistration11 = new DomainStatUserRegistration() { ProductName = ProductName.JwPlayer };
            var domainStatUserRegistration12 = new DomainStatUserRegistration() { ProductName = "ProductName1" };
            var domainStatUserRegistration13 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var otherRegistrationsFilter = new OtherRegistrationsFilter();
            otherRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            otherRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration4, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration5, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration6, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration7, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration8, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration9, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration10, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration11, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration12, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration13, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.OtherRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration4, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration5, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration6, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration7, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration8, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration9, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration10, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration11, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration12, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration13, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = "otherProduct" };
            var domainReport = new DomainReport();

            var otherRegistrationsFilter = new OtherRegistrationsFilter();

            //Act
            otherRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            otherRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.OtherRegistrations);
        }
    }
}