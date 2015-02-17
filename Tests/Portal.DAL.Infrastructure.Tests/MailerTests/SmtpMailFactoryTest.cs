using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Infrastructure.Mailer.Wrappers;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Tests.MailerTests
{
    [TestClass]
    public class MailFactoryTest
    {
        [TestMethod]
        public void CreateMailMessageTest()
        {
            //Arrange
            var mailClientSettings = new MailClientSettings();
            var factory = new SmtpEmailFactory(mailClientSettings);

            //Actl
            var mailMessage = factory.CreateEmailBuilder();

            //Assert
            Assert.IsInstanceOfType(mailMessage,typeof(MailMessageBuilder));
        }

        [TestMethod]
        public void CreateMailClientTest()
        {
            //Arrange
            var mailClientSettings = new MailClientSettings()
            {
                Host = "host",
                UserName = "userName",
                Password = "password",
                Port = 587,
                Timeout = 20000
            };
            var factory = new SmtpEmailFactory(mailClientSettings);

            //Act
            var mailClient = factory.CreateMailClient();

            //Assert
            Assert.IsInstanceOfType(mailClient, typeof(SmtpClientWrapper));
        }
    }
}
