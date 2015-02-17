using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.DAL.Infrastructure.Mailer;
using Portal.DAL.Mailer;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Tests.MailerTests
{
    [TestClass]
    public class MailerRepositoryTest
    {
        [TestMethod]
        public void SendMailTest()
        {
            //Arrange
            var email = new Mock<EmailBase>();
            var mailMessage = new Mock<IEmailBuilder>();
            var mailClient = new Mock<IMailClient>();
            var mailFactory = new Mock<IEmailFactory>();
            var mailIntialezer = new Mock<IEmailInitializer>();

            mailFactory.Setup(m => m.CreateEmailBuilder()).Returns(mailMessage.Object);
            mailFactory.Setup(m => m.CreateMailClient()).Returns(mailClient.Object);

            var tcs = new TaskCompletionSource<object>();

            mailClient.Setup(m => m.Send(mailMessage.Object)).Returns(() =>
                {
                    tcs.SetResult(null);
                    return tcs.Task;
                });

            var repository = new MailerRepository(mailFactory.Object, mailIntialezer.Object);

            //Act
            var task = repository.SendMail(email.Object);

            //Assert
            Assert.AreEqual(tcs.Task, task);
            mailIntialezer.Verify(m => m.Initialize(mailMessage.Object, email.Object), Times.Once());
        }

        [TestMethod]
        public void SendMailsTest()
        {
            //Arrange
            var emailBuilder1 = new Mock<IEmailBuilder>();
            var emailBuilder2 = new Mock<IEmailBuilder>();
            var mailClient1 = new Mock<IMailClient>();
            var mailClient2 = new Mock<IMailClient>();
            var mailFactory = new Mock<IEmailFactory>();
            var mailIntialezer = new Mock<IEmailInitializer>();

            mailFactory.SetupSequence(m => m.CreateEmailBuilder())
                       .Returns(emailBuilder1.Object)
                       .Returns(emailBuilder2.Object);

            mailFactory.SetupSequence(m => m.CreateMailClient())
                       .Returns(mailClient1.Object)
                       .Returns(mailClient2.Object);
            
            var emailList = new List<EmailBase>() { new Mock<EmailBase>().Object, new Mock<EmailBase>().Object };
            var repository = new MailerRepository(mailFactory.Object, mailIntialezer.Object);

            mailClient1.Setup(m => m.Send(It.IsAny<IEmailBuilder>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetResult(null);
                    return tcs.Task;
                });
            mailClient2.Setup(m => m.Send(It.IsAny<IEmailBuilder>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetResult(null);
                    return tcs.Task;
                });
            
            //Act
            var task = repository.SendMails(emailList);
            task.Wait();

            //Assert
            mailIntialezer.Verify(m => m.Initialize(emailBuilder1.Object, emailList[0]), Times.Once());
            mailIntialezer.Verify(m => m.Initialize(emailBuilder2.Object, emailList[1]), Times.Once());
            mailClient1.Verify(m => m.Send(emailBuilder1.Object), Times.Once());
            mailClient2.Verify(m => m.Send(emailBuilder2.Object), Times.Once());
        }
    }
}
