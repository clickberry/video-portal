using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Tests.MailerTests
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void CreateEmailTest()
        {
            //Arrange
            const string content = "content";
            const string contentType = "contentType";
            const string subject = "subject";
            var from = new EmailAddress();
            
            //Act
            var email = new Email(subject, content, contentType, from);
            
            //Assert
            Assert.IsNotNull(email.To);
            Assert.IsNotNull(email.Cc);
            Assert.IsNotNull(email.Bcc);
            Assert.AreEqual(subject,email.Subject);
            Assert.AreEqual(content, email.Content);
            Assert.AreEqual(contentType, email.ContentType);
            Assert.AreEqual(from, email.From);
        }
    }
}
