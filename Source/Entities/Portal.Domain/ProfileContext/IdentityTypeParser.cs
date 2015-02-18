// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.ProfileContext
{
    public static class IdentityTypeParser
    {
        /// <summary>
        ///     Converts provider value to the type.
        /// </summary>
        /// <param name="provider">Provider value.</param>
        /// <returns>Provider type.</returns>
        public static ProviderType ParseProviderType(string provider)
        {
            if (provider.StartsWith("Yahoo"))
            {
                return ProviderType.Yahoo;
            }

            if (provider == "Google")
            {
                return ProviderType.Google;
            }

            if (provider.Contains("WindowsLive"))
            {
                return ProviderType.WindowsLive;
            }

            if (provider.StartsWith("Facebook"))
            {
                return ProviderType.Facebook;
            }

            if (provider.StartsWith("Twitter"))
            {
                return ProviderType.Twitter;
            }

            if (provider.ToUpperInvariant() == "VK")
            {
                return ProviderType.Vk;
            }

            if (provider == "Odnoklassniki")
            {
                return ProviderType.Odnoklassniki;
            }

            throw new ArgumentOutOfRangeException("provider");
        }
    }
}