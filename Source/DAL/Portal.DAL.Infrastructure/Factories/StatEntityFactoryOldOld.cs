using System;
using Portal.DAL.Authentication;
using Portal.DAL.Context;
using Portal.DAL.Converters;
using Portal.DAL.Entities.Table;
using Portal.DAL.Factories;

namespace Portal.DAL.Infrastructure.Factories
{
    public class StatEntityFactoryOldOld : IStatEntityFactoryOld
    {
        private readonly IAuthenticator _authenticator;
        private readonly IHttpContextRepository _httpContextRepository;
        private readonly ITableValueConverter _tableValueConverter;

        public StatEntityFactoryOldOld(IHttpContextRepository httpContextRepository, IAuthenticator authenticator, ITableValueConverter tableValueConverter)
        {
            _httpContextRepository = httpContextRepository;
            _authenticator = authenticator;
            _tableValueConverter = tableValueConverter;
        }

        public StatHttpMessageEntity CreateHttpMessageEntity(string eventId, DateTime dateTime)
        {
            string url = _httpContextRepository.GetUrl();
            string method = _httpContextRepository.GetHttpMethod();
            string[] userLanguages = _httpContextRepository.GetUserLanguages();

            string action = _tableValueConverter.StringsToKey(url, method);
            string convertedUserLanguages = _tableValueConverter.ArrayToString(userLanguages);

            return new StatHttpMessageEntity
                {
                    Action = action,
                    DateTime = dateTime,
                    AnonymousId = _authenticator.GetAnonymousId(),
                    EventId = eventId,
                    HttpMethod = method,
                    IsAuthenticated = _authenticator.IsAuthenticated(),
                    StatusCode = _httpContextRepository.GetStatusCode(),
                    Url = url,
                    UrlReferrer = _httpContextRepository.GetUrlReferrer(),
                    UserAgent = _httpContextRepository.GetUserAgent(),
                    UserHostAddress = _httpContextRepository.GetUserHostAddress(),
                    UserHostName = _httpContextRepository.GetUserHostName(),
                    UserId = _authenticator.GetUserId(),
                    UserLanguages = convertedUserLanguages
                };
        }

        public StatUserRegistrationEntity CreateUserRegestrationEntity(string eventId, string identityProvider, DateTime dateTime)
        {
            return new StatUserRegistrationEntity
                {
                    EventId = eventId,
                    UserId = _authenticator.GetUserId(),
                    ProductName = GetProductName(),
                    IdentityProvider = identityProvider,
                    DateTime = dateTime
                };
        }

        public StatWatchingEntity CreateWatchingEntity(string eventId, string projectId, DateTime dateTime)
        {
            return new StatWatchingEntity
                {
                    EventId = eventId,
                    ProjectId = projectId,
                    UserId = _authenticator.GetUserId(),
                    AnonymousId = _authenticator.GetAnonymousId(),
                    IsAuthenticated = _authenticator.IsAuthenticated(),
                    DateTime = dateTime,
                    UrlReferrer = _httpContextRepository.GetUrlReferrer()
                };
        }

        public StatProjectUploadingEntity CreateProjectUploadingEntity(string eventId, string projectId, DateTime dateTime)
        {
            return new StatProjectUploadingEntity
                {
                    EventId = eventId,
                    ProjectId = projectId,
                    ProductName = GetProductName(),
                    UserId = _authenticator.GetUserId(),
                    DateTime = dateTime
                };
        }

        public StatProjectDeletionEntity CreateProjectDeletionEntity(string eventId, string projectId, DateTime dateTime)
        {
            return new StatProjectDeletionEntity
                {
                    EventId = eventId,
                    ProjectId = projectId,
                    ProductName = GetProductName(),
                    UserId = _authenticator.GetUserId(),
                    DateTime = dateTime
                };
        }

        private string GetProductName()
        {
            string userAgent = _httpContextRepository.GetUserAgent();
            string productName = _tableValueConverter.UserAgentToProductName(userAgent);
            return productName;
        }
    }
}