// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace OkSdk
{
    public sealed class OkClient
    {
        private const string AuthorizationUrl = "https://www.odnoklassniki.ru/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}&response_type=code";
        private const string AccessTokenUrl = "https://api.odnoklassniki.ru/oauth/token.do";
        private const string ApiUrl = "http://api.odnoklassniki.ru/fb.do?{0}";
        private readonly string _applicationId;
        private readonly string _applicationPublic;
        private readonly string _applicationSecret;
        private string _accessToken;

        public OkClient(string applicationId, string applicationSecret, string applicationPublic)
        {
            _applicationId = applicationId;
            _applicationSecret = applicationSecret;
            _applicationPublic = applicationPublic;
        }

        public void AuthenticateWith(string accessToken)
        {
            _accessToken = accessToken;
        }

        public object GetAccessToken(string code, string redirectUrl)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _applicationId },
                { "client_secret", _applicationSecret },
                { "code", code },
                { "redirect_uri", redirectUrl },
                { "grant_type", "authorization_code" }
            });

            HttpResponseMessage result = client.PostAsync(AccessTokenUrl, content).Result;
            string response = result.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject(response);
        }

        public Uri GetAuthorizationUri(string scope = "", string redirectUrl = "")
        {
            return new Uri(string.Format(AuthorizationUrl, _applicationId, scope, redirectUrl));
        }

        public object Get(string method, object parameters = null)
        {
            Dictionary<string, string> properties = parameters.ToDictionary();
            properties.Add("method", method);
            properties.Add("application_key", _applicationPublic);

            string arguments = string.Join(string.Empty, properties.OrderBy(p => p.Key).Select(p => string.Format("{0}={1}", p.Key, p.Value)));

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] secretBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(_accessToken + _applicationSecret));
            arguments += BitConverter.ToString(secretBytes).Replace("-", string.Empty).ToLowerInvariant();

            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(arguments));
            string hashHex = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();

            properties.Add("access_token", _accessToken);
            properties.Add("sig", hashHex);

            arguments = string.Join("&", properties.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

            var client = new HttpClient();
            string requestUrl = string.Format(ApiUrl, arguments);

            HttpResponseMessage result = client.GetAsync(requestUrl).Result;
            string response = result.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject(response);
        }
    }
}