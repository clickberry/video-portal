using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Filter.Filters.Providers;
using Portal.BLL.Statistics.Filter;
using Portal.Domain.ProfileContext;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Tests.StatisticsTests.FilterTest.FiltersTest.Providers
{
    [TestClass]
    public class VkRegistrationsFilterTest
    {
        [TestMethod]
        public void CallTest()
        {
            //Arrange
            var domainStatUserRegistration1 = new DomainStatUserRegistration() { IdentityProvider = IdentityType.Vk.ToString() };
            var domainStatUserRegistration2 = new DomainStatUserRegistration() { IdentityProvider = "IdentityProvider" };
            var domainStatUserRegistration3 = new DomainStatUserRegistration();

            var domainReport = new DomainReport();

            var statUserRegistrationFilter = new Mock<IStatUserRegistrationFilter>();

            var vkRegistrationsFilter = new VkRegistrationsFilter();
            vkRegistrationsFilter.Set(statUserRegistrationFilter.Object);

            //Act
            vkRegistrationsFilter.Call(domainStatUserRegistration1, domainReport);
            vkRegistrationsFilter.Call(domainStatUserRegistration2, domainReport);
            vkRegistrationsFilter.Call(domainStatUserRegistration3, domainReport);

            //Assert
            Assert.AreEqual(1, domainReport.VkRegistrations);
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration1, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration2, domainReport), Times.Once());
            statUserRegistrationFilter.Verify(m => m.Call(domainStatUserRegistration3, domainReport), Times.Once());
        }

        [TestMethod]
        public void CallWhenNotSetFelterTest()
        {
            //Arrange
            var domainStatUserRegistration = new DomainStatUserRegistration() { IdentityProvider = IdentityType.Vk.ToString() };
            var domainReport = new DomainReport();

            var vkRegistrationsFilter = new VkRegistrationsFilter();

            //Act
            vkRegistrationsFilter.Call(domainStatUserRegistration, domainReport);
            vkRegistrationsFilter.Call(domainStatUserRegistration, domainReport);

            //Assert
            Assert.AreEqual(2, domainReport.VkRegistrations);
        }
    }
}