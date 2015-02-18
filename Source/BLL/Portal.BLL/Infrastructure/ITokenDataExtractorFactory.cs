// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.DTO.User;

namespace Portal.BLL.Infrastructure
{
    /// <summary>
    ///     Token data extractor's factory.
    /// </summary>
    public interface ITokenDataExtractorFactory
    {
        /// <summary>
        ///     Creates a new token data extractor.
        /// </summary>
        /// <param name="type">Token type.</param>
        /// <returns>Token data extractor.</returns>
        ITokenDataExtractor CreateTokenDataExtractor(TokenType type);
    }
}