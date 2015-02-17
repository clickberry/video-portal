using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain.StatisticContext;

namespace Portal.Domain.Tests.StatisticsTests
{
    [TestClass]
    public class DomainStatUserTest
    {
        [TestMethod]
        public void CreateUserTest()
        {
            //Arrange
            const string userId = "userId";
            const string userEmail = "userEmail";
            const string userName = "userName";
            const string identityProvider = "identityProvider";
            const string productName = "productName";
            const string productVersion = "productVersion";
            
            //Act
            var user = new DomainStatUser(userId, userEmail, userName, identityProvider, productName, productVersion);

            //Assert
            Assert.AreEqual(userId,user.UserId);
            Assert.AreEqual(userEmail,user.Email);
            Assert.AreEqual(userName, user.UserName);
            Assert.AreEqual(identityProvider, user.IdentityProvider);
            Assert.AreEqual(productName, user.ProductName);
            Assert.AreEqual(productVersion, user.ProductVersion);
        }
    }
}
