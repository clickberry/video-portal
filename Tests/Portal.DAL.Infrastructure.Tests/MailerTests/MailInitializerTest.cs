using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Tests.MailerTests
{
    [TestClass]
    public class MailInitializerTest
    {
        private List<EmailAddress> _resipientList;
        private Mock<IEmailBuilder> _emailBuilder;
        private Mock<EmailBase> _email;
        private EmailInitializer _emailInitializer;

        [TestInitialize]
        public void Initialize()
        {
            _resipientList = new List<EmailAddress>()
                {
                    new EmailAddress(),
                    new EmailAddress()
                };

            _emailBuilder = new Mock<IEmailBuilder>();
            _email = new Mock<EmailBase>();
            _emailInitializer = new EmailInitializer();

            _email.Setup(p => p.To).Returns(new List<EmailAddress>());
            _email.Setup(p => p.Cc).Returns(new List<EmailAddress>());
            _email.Setup(p => p.Bcc).Returns(new List<EmailAddress>());
        }

        [TestMethod]
        public void InitializeToRecipientsTest()
        {
            //Arrange
            var receiveList = new List<EmailAddress>();

            _email.Setup(p => p.To).Returns(_resipientList);
            _emailBuilder.Setup(m => m.AddToRecipient(It.IsAny<EmailAddress>())).Callback<EmailAddress>(receiveList.Add);
            
            //Act
            _emailInitializer.Initialize(_emailBuilder.Object,_email.Object);

            //Assert
            Assert.AreEqual(_resipientList.Count, receiveList.Count);
            Assert.IsTrue(_resipientList.All(p=>(receiveList.Any(r=>p==r))));
        }

        [TestMethod]
        public void InitializeCcRecipientsTest()
        {
            //Arrange
            var receiveList = new List<EmailAddress>();

            _email.Setup(p => p.Cc).Returns(_resipientList);
            _emailBuilder.Setup(m => m.AddCcRecipient(It.IsAny<EmailAddress>())).Callback<EmailAddress>(receiveList.Add);
            
            //Act
            _emailInitializer.Initialize(_emailBuilder.Object, _email.Object);

            //Assert
            Assert.AreEqual(_resipientList.Count, receiveList.Count);
            Assert.IsTrue(_resipientList.All(p => (receiveList.Any(r => p == r))));
        }

        [TestMethod]
        public void InitializeBccRecipientsTest()
        {
            //Arrange
            var receiveList = new List<EmailAddress>();

            _email.Setup(p => p.Bcc).Returns(_resipientList);
            _emailBuilder.Setup(m => m.AddBccRecipient(It.IsAny<EmailAddress>())).Callback<EmailAddress>(receiveList.Add);

            //Act
            _emailInitializer.Initialize(_emailBuilder.Object, _email.Object);

            //Assert
            Assert.AreEqual(_resipientList.Count, receiveList.Count);
            Assert.IsTrue(_resipientList.All(p => (receiveList.Any(r => p == r))));
        }

        [TestMethod]
        public void InitializeSenderTest()
        {
            //Arrange
            var sender = new EmailAddress();

            _email.Setup(p => p.From).Returns(sender);

            //Act
            _emailInitializer.Initialize(_emailBuilder.Object, _email.Object);

            //Assert
           _emailBuilder.Verify(m=>m.SetSender(sender));
        }

        [TestMethod]
        public void InitializeContentTest()
        {
            //Arrange
            const string subject = "subject";
            const string content = "content";
            const string contentType = "contentType";

            _email.Setup(m => m.Subject).Returns(subject);
            _email.Setup(m => m.Content).Returns(content);
            _email.Setup(m => m.ContentType).Returns(contentType);

            //Act
            _emailInitializer.Initialize(_emailBuilder.Object, _email.Object);

            //Assert
            _emailBuilder.Verify(m => m.SetContent(subject,content,contentType));
        }
    }
}
