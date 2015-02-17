using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Repository.Infrastructure;
using Portal.Repository.User;

namespace Portal.Repository.Tests
{
    [TestClass]
    public class UserIdentityTest
    {
        const string Provider = "Email";
        const string Identity = "secretIdentifier";

        [TestMethod]
        public void TestIdentityConstruction()
        {
            // Arrange
            string testIdentityString = string.Format("{0}{2}{1}", Provider, Identity, UserIdentity.IdentitySeparator);
            UserIdentity userIdentity = null;

            // Act
            try
            {
                userIdentity = new UserIdentity(testIdentityString);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            // Assert
            Assert.IsNotNull(userIdentity);
            Assert.AreEqual(userIdentity.IdentityProvider, IdentityProviderType.Email);
            Assert.AreEqual(userIdentity.IdentityValue, Identity);
        }

        [TestMethod]
        public void TestIdentityConstructionFromEncodedString()
        {
            // Arrange
            string encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(Identity));

            string testIdentityString = string.Format("{0}{2}{1}", Provider, encodedString, UserIdentity.IdentitySeparator);
            UserIdentity userIdentity = null;

            // Act
            try
            {
                userIdentity = new UserIdentity(testIdentityString, true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            // Assert
            Assert.IsNotNull(userIdentity);
            Assert.AreEqual(userIdentity.IdentityProvider, IdentityProviderType.Email);
            Assert.AreEqual(userIdentity.IdentityValue, Identity);
            Assert.AreEqual(userIdentity.EncodedIdentity, testIdentityString);
        }

        [TestMethod]
        public void TestConstructionWithInvalidIdentityString()
        {
            // Arrange
            string testIdentityString = string.Format("{0}{1}", Provider, Identity);
            ArgumentException exception = null;

            // Act
            try
            {
                new UserIdentity(testIdentityString);
                Assert.Fail("It should not constructed.");
            }
            catch (ArgumentException e)
            {
                exception = e;
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            // Assert
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void TestConstructionWithDashAtTheEndOfIdentityString()
        {
            // Arrange
            string testIdentityString = string.Format("{0}{1}-", Provider, Identity);
            ArgumentException exception = null;

            // Act
            try
            {
                new UserIdentity(testIdentityString);
                Assert.Fail("It should not constructed.");
            }
            catch (ArgumentException e)
            {
                exception = e;
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            // Assert
            Assert.IsNotNull(exception);
        }
    }
}