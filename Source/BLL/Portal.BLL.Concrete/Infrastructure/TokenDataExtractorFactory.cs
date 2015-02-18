// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Configuration;
using Portal.BLL.Concrete.Infrastructure.Security;
using Portal.BLL.Infrastructure;
using Portal.DTO.User;

namespace Portal.BLL.Concrete.Infrastructure
{
    /// <summary>
    ///     Token data extractor's factory.
    /// </summary>
    public sealed class TokenDataExtractorFactory : ITokenDataExtractorFactory
    {
        private readonly IPortalFrontendSettings _settings;

        public TokenDataExtractorFactory(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        ///     Creates a new token data extractor.
        /// </summary>
        /// <param name="type">Token type.</param>
        /// <returns>Token data extractor.</returns>
        public ITokenDataExtractor CreateTokenDataExtractor(TokenType type)
        {
            switch (type)
            {
                case TokenType.Acs:
                    return new SwtTokenDataExtractor();

                case TokenType.Facebook:
                    return new FacebookTokenDataExtractor();

                case TokenType.Twitter:
                    return new TwitterTokenDataExtractor(_settings);

                case TokenType.Vk:
                    return new VkTokenDataExtractor();

                case TokenType.Odnoklassniki:
                    return new OdnoklassnikiTokenDataExtractor(_settings);

                case TokenType.Claims:
                    return new ClaimsTokenDataExtractor();

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}