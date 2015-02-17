using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.ImageShack;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.ImageShack
{
    [TestClass]
    public class ImageShackRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { ProductName = ProductName.ImageShack };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { ProductName = "ProductName" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var imageShackRegistrationsFilter = new ImageShackRegistrationsFilter();
            imageShackRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            imageShackRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            imageShackRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            imageShackRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.ImageShackRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { ProductName = ProductName.ImageShack };
            var domainReport = new DomainReport();

            var imageShackRegistrationsFilter = new ImageShackRegistrationsFilter();

            //Act
            imageShackRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            imageShackRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.ImageShackRegistrations);
        }
    }
}