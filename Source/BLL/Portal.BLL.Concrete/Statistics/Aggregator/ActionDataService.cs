// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Aggregator;
using Portal.DAL.Authentication;
using Portal.DAL.Context;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Aggregator
{
    public class ActionDataService : IActionDataService
    {
        private readonly IAuthenticator _authenticator;
        private readonly IHttpContextRepository _httpContextRepository;

        public ActionDataService(IHttpContextRepository httpContextRepository, IAuthenticator authenticator)
        {
            _httpContextRepository = httpContextRepository;
            _authenticator = authenticator;
        }

        public DomainActionData GetActionData(string userAgent, int statusCode)
        {
            return new DomainActionData
            {
                IsAuthenticated = _authenticator.IsAuthenticated(),
                AnonymousId = _authenticator.GetAnonymousId(),
                UserId = _authenticator.GetUserId(),
                UserEmail = _authenticator.GetUserEmail(),
                UserName = _authenticator.GetUserName(),
                IdentityProvider = _authenticator.GetIdentityProvider(),
                HttpMethod = _httpContextRepository.GetHttpMethod(),
                StatusCode = statusCode,
                Url = _httpContextRepository.GetUrl(),
                UrlReferrer = _httpContextRepository.GetUrlReferrer(),
                UserAgent = userAgent,
                UserHostAddress = _httpContextRepository.GetUserHostAddress(),
                UserHostName = _httpContextRepository.GetUserHostName(),
                UserLanguages = _httpContextRepository.GetUserLanguages()
            };
        }
    }
}