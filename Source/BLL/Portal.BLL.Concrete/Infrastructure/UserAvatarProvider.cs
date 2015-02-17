// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;
using Configuration;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProfileContext;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class UserAvatarProvider : IUserAvatarProvider
    {
        private readonly IPortalFrontendSettings _settings;

        public UserAvatarProvider(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public string GetAvatar(DomainUser user, string scheme = "http")
        {
            return GetAvatar(user != null ? user.Email : null, scheme);
        }

        public string GetAvatar(string email, string scheme = "http")
        {
            var uriBuilder = new UriBuilder(scheme)
            {
                Host = "www.gravatar.com",
                Path = string.Format("/avatar/{0}.png", !string.IsNullOrEmpty(email) ? GetMd5Hash(email) : string.Empty),
                Query = string.Format("d={0}", _settings.DefaultAvatarUri)
            };

            return uriBuilder.Uri.ToString();
        }

        private static string GetMd5Hash(string value)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] secretBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                return BitConverter.ToString(secretBytes).Replace("-", string.Empty).ToLowerInvariant();
            }
        }
    }
}