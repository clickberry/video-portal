// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace VkSdk
{
    public sealed class VkClient
    {
        private const string AuthorizationUrl = "https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&response_type=code";
        private const string AccessTokenUrl = "https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&code={2}&redirect_uri={3}";
        private const string ApiUrl = "https://api.vk.com/method/{0}?{1}&access_token={2}";
        private readonly string _applicationId;
        private readonly string _applicationSecret;
        private string _accessToken;

        public VkClient()
        {
        }

        public VkClient(string applicationId, string applicationSecret)
        {
            _applicationId = applicationId;
            _applicationSecret = applicationSecret;
        }

        public void AuthenticateWith(string accessToken)
        {
            _accessToken = accessToken;
        }

        public object GetAccessToken(string code, string redirectUrl)
        {
            var client = new HttpClient();
            HttpResponseMessage result = client.GetAsync(string.Format(AccessTokenUrl, _applicationId, _applicationSecret, code, redirectUrl)).Result;

            string response = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject(response);
        }

        public Uri GetAuthorizationUri(string scope = "", string redirectUrl = "")
        {
            return new Uri(string.Format(AuthorizationUrl, _applicationId, scope, redirectUrl));
        }

        public object Get(string method, object parameters)
        {
            Dictionary<string, string> properties = parameters.ToDictionary();
            string arguments = string.Join("&", properties.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

            var client = new HttpClient();
            HttpResponseMessage result = client.GetAsync(string.Format(ApiUrl, method, arguments, _accessToken)).Result;

            string response = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject(response);
        }
    }
}