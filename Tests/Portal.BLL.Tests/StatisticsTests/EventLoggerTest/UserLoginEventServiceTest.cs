using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.EventLogger;
using Portal.BLL.Statistics.EventLogger;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using TestFake;
using Wrappers.Interface;

namespace Portal.BLL.Tests.StatisticsTests.EventLoggerTest
{
    [TestClass]
    public class UserLoginEventServiceTest
    {
        [TestMethod]
        public void LogUserLoginEventTest()
        {
            //Arrange
            var timeSpan = new DateTime(2013, 7, 16);
            var actionDomain = new DomainActionData();
            var user = new FakeDomainStatUser();

            var eventLogger = new Mock<IEventLogger>();
            var statDomainFactory = new Mock<IStatDomainFactory>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();

            var userRegistrationEventService = new UserLoginEventService(eventLogger.Object, statDomainFactory.Object, dateTimeWrapper.Object);

            statDomainFactory.Setup(m => m.CreateUser(actionDomain)).Returns(user);
            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(timeSpan);

            //Act
            userRegistrationEventService.LogUserLogin(actionDomain);

            //Assert
            eventLogger.Verify(m => m.Identify(user, timeSpan), Times.Once());
            eventLogger.Verify(m => m.TrackUserLoginEvent(user, timeSpan), Times.Once());
        }
    }
}