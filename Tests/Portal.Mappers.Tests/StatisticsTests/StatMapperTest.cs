using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.DAL.Entities.Table;
using Portal.Mappers.Statistics;

namespace Portal.Mappers.Tests.StatisticsTests
{
    [TestClass]
    public class StatMapperTest
    {
        [TestMethod]
        public void StatWatchingEntityToDomainTest()
        {
            //Arrange
            var watchingEntity = new StatWatchingV2Entity()
                {
                    AnonymousId = "AnonymousId",
                    DateTime = new DateTime(23456345344),
                    ProjectId = "projectId",
                    UrlReferrer = "urlReferrer",
                    UserId = "userId",
                    IsAuthenticated = true,
                    Tick = "tick",
                    EventId = "eventId"
                };

            var statMapper = new StatMapper();

            //Act
            var domain = statMapper.StatWatchingToDomain(watchingEntity);

            //Assert
            Assert.AreEqual(watchingEntity.Tick, domain.Tick);
            Assert.AreEqual(watchingEntity.AnonymousId, domain.AnonymousId);
            Assert.AreEqual(watchingEntity.DateTime, domain.DateTime);
            Assert.AreEqual(watchingEntity.EventId, domain.EventId);
            Assert.AreEqual(watchingEntity.IsAuthenticated, domain.IsAuthenticated);
            Assert.AreEqual(watchingEntity.ProjectId, domain.ProjectId);
            Assert.AreEqual(watchingEntity.UrlReferrer, domain.UrlReferrer);
            Assert.AreEqual(watchingEntity.UserId, domain.UserId);
        }

        [TestMethod]
        public void StatUserRegistrationToDomainTest()
        {
            //Arrange
            var registrationEntity = new StatUserRegistrationV2Entity()
            {
                Tick="tick",
                DateTime = new DateTime(23456345344),
                UserId = "userId",
                EventId = "eventId",
                IdentityProvider="identityProvider",
                ProductName = "productName"
            };

            var statMapper = new StatMapper();

            //Act
            var domain = statMapper.StatUserRegistrationToDomain(registrationEntity);

            //Assert
            Assert.AreEqual(registrationEntity.Tick, domain.Tick);
            Assert.AreEqual(registrationEntity.DateTime, domain.DateTime);
            Assert.AreEqual(registrationEntity.EventId, domain.EventId);
            Assert.AreEqual(registrationEntity.UserId, domain.UserId);
            Assert.AreEqual(registrationEntity.IdentityProvider, domain.IdentityProvider);
            Assert.AreEqual(registrationEntity.ProductName, domain.ProductName);
        }

        [TestMethod]
        public void StatProjectUploadingToDomainTest()
        {
            //Arrange
            var uploadingEntity = new StatProjectUploadingV2Entity()
                {
                    Tick = "tick",
                    DateTime = new DateTime(23456345344),
                    UserId = "userId",
                    EventId = "eventId",
                    ProductName = "productName",
                    ProjectId = "projectId"
                };

            var statMapper = new StatMapper();

            //Act
            var domain = statMapper.StatProjectUploadingToDomain(uploadingEntity);

            //Assert
            Assert.AreEqual(uploadingEntity.Tick, domain.Tick);
            Assert.AreEqual(uploadingEntity.DateTime, domain.DateTime);
            Assert.AreEqual(uploadingEntity.EventId, domain.EventId);
            Assert.AreEqual(uploadingEntity.UserId, domain.UserId);
            Assert.AreEqual(uploadingEntity.ProductName, domain.ProductName);
            Assert.AreEqual(uploadingEntity.ProjectId, domain.ProjectId);
        }

        [TestMethod]
        public void DomainStatProjectDeletionTest()
        {
            //Arrange
            var deletionEntity = new StatProjectDeletionV2Entity()
            {
                Tick = "tick",
                DateTime = new DateTime(23456345344),
                UserId = "userId",
                EventId = "eventId",
                ProductName = "productName",
                ProjectId = "projectId"
            };

            var statMapper = new StatMapper();

            //Act
            var domain = statMapper.StatProjectDeletionToDomain(deletionEntity);

            //Assert
            Assert.AreEqual(deletionEntity.Tick, domain.Tick);
            Assert.AreEqual(deletionEntity.DateTime, domain.DateTime);
            Assert.AreEqual(deletionEntity.EventId, domain.EventId);
            Assert.AreEqual(deletionEntity.UserId, domain.UserId);
            Assert.AreEqual(deletionEntity.ProductName, domain.ProductName);
            Assert.AreEqual(deletionEntity.ProjectId, domain.ProjectId);
        }

        [TestMethod]
        public void StatProjectStateToDomain()
        {
            //Arrange
            const bool isSuccessfulUopload = true;

            var projectStateEntity = new StatProjectStateV3Entity()
                {
                    ProjectId = "projectId",
                    ActionType = "actionType",
                    DateTime = new DateTime(23456345344),
                    Producer = "producer",
                    Version = "version"
                };

            var statMapper = new StatMapper();

            //Act
            var domain = statMapper.StatProjectStateToDomain(projectStateEntity, isSuccessfulUopload);

            //Assert
            Assert.AreEqual(projectStateEntity.ProjectId, domain.ProjectId);
            Assert.AreEqual(projectStateEntity.Producer, domain.Producer);
            Assert.AreEqual(projectStateEntity.Version, domain.Version);
            Assert.AreEqual(projectStateEntity.DateTime,domain.DateTime);
            Assert.AreEqual(isSuccessfulUopload,domain.IsSuccessfulUpload);
        }
    }
}
