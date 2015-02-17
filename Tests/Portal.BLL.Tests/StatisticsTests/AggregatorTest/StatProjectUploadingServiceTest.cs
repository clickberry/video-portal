using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BLL.Concrete.Statistics.Aggregator;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DTO.Projects;
using Portal.Domain.ProjectContext;
using Portal.Domain.StatisticContext;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.BLL.Tests.StatisticsTests.AggregatorTest
{
    [TestClass]
    public class StatProjectUploadingServiceTest
    {
        [TestMethod]
        public async Task AddProjectUploadingTest()
        {
            const string eventId = "eventId";
            const string projectId = "projectId";
            const string projectName = "projectName";
            const ProjectType projectType = ProjectType.Tag;
            const ProjectSubtype projectSubtype = ProjectSubtype.Friend;

            var dateTime = new DateTime(2013, 2, 13);

            var domain = new DomainActionData();
            var httpMessageEntity = new StatHttpMessageV2Entity();
            var projectUploadingEntity = new StatProjectUploadingV2Entity();

            var guidWrapper = new Mock<IGuidWrapper>();
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            var httpMessageRepository = new Mock<IRepository<StatHttpMessageV2Entity>>();
            var projectUploadingRepository = new Mock<IRepository<StatProjectUploadingV2Entity>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var statEntityFactory = new Mock<IStatEntityFactory>();

            guidWrapper.Setup(m => m.Generate()).Returns(eventId);
            dateTimeWrapper.Setup(m => m.CurrentDateTime()).Returns(dateTime);

            repositoryFactory.Setup(m => m.Create<StatHttpMessageV2Entity>(Tables.StatHttpMessageV2)).Returns(httpMessageRepository.Object);
            repositoryFactory.Setup(m => m.Create<StatProjectUploadingV2Entity>(Tables.StatProjectUploadingV2)).Returns(projectUploadingRepository.Object);

            statEntityFactory.Setup(m => m.CreateHttpMessageEntity(eventId, dateTime, domain)).Returns(httpMessageEntity);
            statEntityFactory.Setup(m => m.CreateProjectUploadingEntity(eventId, dateTime, domain, projectId, projectName, projectType,projectSubtype)).Returns(projectUploadingEntity);
            httpMessageRepository.Setup(m => m.AddAsync(httpMessageEntity, It.IsAny<CancellationToken>())).Returns(async () => httpMessageEntity);
            projectUploadingRepository.Setup(m => m.AddAsync(projectUploadingEntity, It.IsAny<CancellationToken>())).Returns(async () => projectUploadingEntity);

            var userRegistrationService = new StatProjectUploadingService(repositoryFactory.Object, statEntityFactory.Object, guidWrapper.Object, dateTimeWrapper.Object);

            //Act & Assert
            await userRegistrationService.AddProjectUploading(domain, projectId, projectName, projectType,projectSubtype);
        }
    }
}
