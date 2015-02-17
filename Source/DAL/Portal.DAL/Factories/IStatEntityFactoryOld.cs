using System;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Factories
{
    public interface IStatEntityFactoryOld
    {
        StatHttpMessageEntity CreateHttpMessageEntity(string eventId, DateTime dateTime);
        StatUserRegistrationEntity CreateUserRegestrationEntity(string eventId, string identityProvider, DateTime dateTime);
        StatWatchingEntity CreateWatchingEntity(string eventId, string projectId, DateTime dateTime);
        StatProjectUploadingEntity CreateProjectUploadingEntity(string eventId, string projectId, DateTime dateTime);
        StatProjectDeletionEntity CreateProjectDeletionEntity(string eventId, string projectId, DateTime dateTime);
    }
}