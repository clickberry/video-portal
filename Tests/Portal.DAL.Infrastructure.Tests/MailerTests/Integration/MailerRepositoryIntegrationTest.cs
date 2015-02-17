using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.DAL.Infrastructure.Mailer;
using Portal.Domain.MailerContext;

namespace Portal.DAL.Infrastructure.Tests.MailerTests.Integration
{
    [TestClass]
    public class MailerRepositoryIntegrationTest
    {
        [TestMethod]
        public void SendMailTest()
        {
            //Arrange
            const string fromAddress = "Gmail@gmail.com";
            const string toAddress = "altuhov@clickberry.com";
           
            const string password = "media2011";
            const string userName = "clickberry";

            const string fromDisplayName = "fromDisplayName";
            const string subject = "Cool Subject";

            const string host = "smtp.sendgrid.net";
            const int port = 587;
            const int timeout = 20000;

            const string body1 = @"<html><head></head><body><p>Hello World!!!</p></body></html>";

            var mailClientSettings = new MailClientSettings()
                {
                    EnableSsl = true,
                    Host = host,
                    Port = port,
                    Timeout = timeout,
                    UserName = userName,
                    Password = password
                };
            var fromEmail = new EmailAddress()
                {
                    Address = fromAddress,
                    DisplayName = fromDisplayName
                };

            var mailInitializer = new EmailInitializer();
            var factory = new SmtpEmailFactory(mailClientSettings);
            var mailerRepository = new MailerRepository(factory,mailInitializer);
            
            var email = new Email(subject, body1, ContentType.Html, fromEmail);

            email.To.Add(new EmailAddress() { Address = toAddress, DisplayName = "ToKostuyn" });
            email.Cc.Add(new EmailAddress() { Address = toAddress, DisplayName = "CcKostuyn" });
            email.Bcc.Add(new EmailAddress() { Address = toAddress, DisplayName = "BccKostuyn" });

            //Act
            var task = mailerRepository.SendMail(email);
            task.Wait();
        }

        [TestMethod]
        public void SendMailsTest()
        {
            //Arrange
            const string fromAddress = "Gmail@gmail.com";
            const string toAddress = "altuhov@clickberry.com";

            const string password = "media2011";
            const string userName = "clickberry";

            const string fromDisplayName = "fromDisplayName";
            const string subject = "Cool Subject";

            const string host = "smtp.sendgrid.net";
            const int port = 587;
            const int timeout = 20000;

            const string body1 = @"<html><head></head><body><p>Hello World!!!</p></body></html>";

            var mailClientSettings = new MailClientSettings()
                {
                    EnableSsl = true,
                    Host = host,
                    Port = port,
                    Timeout = timeout,
                    UserName = userName,
                    Password = password
                };
            var fromEmail = new EmailAddress()
                {
                    Address = fromAddress,
                    DisplayName = fromDisplayName
                };

            var mailInitializer = new EmailInitializer();
            var factory = new SmtpEmailFactory(mailClientSettings);
            var mailerRepository = new MailerRepository(factory, mailInitializer);

            var email1 = new Email(subject, body1, ContentType.Html, fromEmail);
            var email2 = new Email("subject", body1, ContentType.Html, fromEmail);

            email1.To.Add(new EmailAddress() {Address = toAddress, DisplayName = "ToKostuyn"});
            email2.To.Add(new EmailAddress() {Address = toAddress, DisplayName = "ToToKostuyn"});

            var emailList = new List<EmailBase> {email1, email2};

            //Act
            var task = mailerRepository.SendMails(emailList);
            task.Wait();
        }
    }
}
