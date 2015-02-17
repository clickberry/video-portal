using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.EventLogger;
using Portal.BLL.Statistics.EventLogger;
using Portal.DTO.Projects;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using TestFake;
using Wrappers.Interface;

namespace Portal.BLL.Tests.StatisticsTests.EventLoggerTest
{
    [TestClass]
    public class ProjectUploadingEventServiceTest
    {
        [TestMethod]
        public void LogProjectUploadingTest()
        {
            //Arrange
            const string projectId = "projectId";
            const string projectName = "projectName";
            const ProjectType projectType = ProjectType.Tag;
            const ProjectSubtype projectSubtype = ProjectSubtype.Friend;

            var timeSpan = new DateTime(2013, 7, 16);
            var actionDomain = new DomainActionData();
            var project = new FakeDomainStatProject();

            var eventLogger = new Mock<IEventLogger>();
            var statDomainFactory = new Mock<IStatDomainFactory>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();

            var projectUploadingEventService = new ProjectUploadingEventService(eventLogger.Object,statDomainFactory.Object,dateTimeWrapper.Object);
            statDomainFactory.Setup(m => m.CreateProject(actionDomain, projectId, projectName, projectType.ToString(), projectSubtype.ToString())).Returns(project);
            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(timeSpan);

            //Act
            projectUploadingEventService.LogProjectUploading(actionDomain, projectId, projectName, projectType, projectSubtype);
            
            //Assert
            eventLogger.Verify(m => m.TrackProjectUploadingEvent(project, timeSpan), Times.Once());
        }
    }
}