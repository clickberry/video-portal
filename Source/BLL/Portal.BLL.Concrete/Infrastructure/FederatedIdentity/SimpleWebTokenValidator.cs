// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Portal.BLL.Concrete.Infrastructure.FederatedIdentity
{
    /// <summary>
    ///     Validates SimpleWebTokens
    /// </summary>
    /// <example>
    ///     <code>
    /// var validator = new SimpleWebTokenValidator();
    /// validator.SharedKeyBase64 = "yoursharedkeyinbase64";
    /// var swt = validator.ValidateToken(token); // this throws if it's invalid
    /// // access the claims doing swt.Claims (Dictionary)
    /// </code>
    /// </example>
    public class SimpleWebTokenValidator
    {
        public SimpleWebTokenValidator()
        {
            AllowedAudiences = new List<Uri>();
        }

        /// <summary>
        ///     Symmetric key in base64 format
        /// </summary>
        public string SharedKeyBase64 { get; set; }

        /// <summary>
        ///     Allowed audience URIs (optional)
        /// </summary>
        public List<Uri> AllowedAudiences { get; set; }

        /// <summary>
        ///     Allowed issuer (optional)
        /// </summary>
        public string AllowedIssuer { get; set; }

        public SimpleWebToken ValidateTokenFromBase64(string base64BinaryToken)
        {
            if (base64BinaryToken == null)
                throw new HttpException((int)HttpStatusCode.Unauthorized, "SWT not found");

            byte[] swtBuffer = Convert.FromBase64String(base64BinaryToken);
            string swt = Encoding.Default.GetString(swtBuffer);

            return ValidateToken(swt);
        }

        public SimpleWebToken ValidateToken(string token)
        {
            if (token == null)
                throw new HttpException((int)HttpStatusCode.Unauthorized, "SWT not found");

            var swt = new SimpleWebToken(token);
            byte[] securityKey = Convert.FromBase64String(SharedKeyBase64);

            if (securityKey == null)
                throw new HttpException((int)HttpStatusCode.Unauthorized, "Missing shared key");

            if (!IsHmacValid(swt.RawToken, securityKey))
                throw new HttpException((int)HttpStatusCode.Unauthorized, "Invalid signature");

            if (swt.IsExpired)
                throw new HttpException((int)HttpStatusCode.Unauthorized, "Token expired");

            if (AllowedAudiences != null && AllowedAudiences.Count > 0)
            {
                Uri swtAudienceUri;
                if (!Uri.TryCreate(swt.Audience, UriKind.RelativeOrAbsolute, out swtAudienceUri))
                    throw new HttpException((int)HttpStatusCode.Unauthorized, "Invalid audience");

                if (AllowedAudiences.All(uri => uri != swtAudienceUri))
                    throw new HttpException((int)HttpStatusCode.Unauthorized, "Audience not found");
            }

            if (!string.IsNullOrEmpty(AllowedIssuer))
            {
                if (!AllowedIssuer.Equals(swt.Issuer, StringComparison.Ordinal))
                {
                    throw new HttpException((int)HttpStatusCode.Unauthorized, "Invalid issuer");
                }
            }

            return swt;
        }

        private static bool IsHmacValid(string swt, byte[] sha256HmacKey)
        {
            string[] swtWithSignature = swt.Split(new[] { String.Format("&{0}=", SwtConstants.HmacSha256) }, StringSplitOptions.None);
            if (swtWithSignature.Length != 2)
                return false;

            using (var hmac = new HMACSHA256(sha256HmacKey))
            {
                byte[] locallyGeneratedSignatureInBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(swtWithSignature[0]));
                string locallyGeneratedSignature = HttpUtility.UrlEncode(Convert.ToBase64String(locallyGeneratedSignatureInBytes));

                return String.Equals(locallyGeneratedSignature, swtWithSignature[1], StringComparison.InvariantCulture);
            }
        }
    }

    public class SimpleWebToken
    {
        public SimpleWebToken(string rawToken)
        {
            RawToken = rawToken;
            Parse();
        }

        public bool IsExpired
        {
            get
            {
                ulong expiresOn = ExpiresOn.ToEpochTime();
                ulong currentTime = DateTime.UtcNow.ToEpochTime();

                return currentTime > expiresOn;
            }
        }

        public string Audience { get; private set; }
        public Dictionary<string, string> Claims { get; private set; }
        public DateTime ExpiresOn { get; private set; }
        public string Issuer { get; private set; }
        public string RawToken { get; private set; }

        public override string ToString()
        {
            return RawToken;
        }

        private void Parse()
        {
            Claims = new Dictionary<string, string>();

            foreach (string rawNameValue in RawToken.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (rawNameValue.StartsWith(SwtConstants.HmacSha256 + "="))
                    continue;

                string[] nameValue = rawNameValue.Split('=');

                if (nameValue.Length != 2)
                {
                    string message = string.Format("Invalid token contains a name/value pair missing an = character: '{0}'", rawNameValue);
                    throw new InvalidSecurityTokenException(message);
                }

                string key = HttpUtility.UrlDecode(nameValue[0]);

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                if (Claims.ContainsKey(key))
                {
                    throw new InvalidSecurityTokenException("Duplicated name token.");
                }

                string values = HttpUtility.UrlDecode(nameValue[1]);

                if (string.IsNullOrEmpty(values))
                {
                    continue;
                }

                switch (key)
                {
                    case SwtConstants.Audience:
                        Audience = values;
                        break;
                    case SwtConstants.ExpiresOn:
                        ExpiresOn = ulong.Parse(values).ToDateTimeFromEpoch();
                        break;
                    case SwtConstants.Issuer:
                        Issuer = values;
                        break;
                    default:
                        // We may have more than one value in SWT.
                        string value = values.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).First();
                        Claims.Add(key, value);
                        break;
                }
            }
        }
    }

    internal class SwtConstants
    {
        public const string Audience = "Audience";
        public const string Issuer = "Issuer";
        public const string ExpiresOn = "ExpiresOn";
        public const string HmacSha256 = "HMACSHA256";
    }

    internal class InvalidSecurityTokenException : Exception
    {
        public InvalidSecurityTokenException(string message)
            : base(message)
        {
        }
    }

    #region BSD License

    /* 
        Copyright (c) 2010, NETFx
        All rights reserved.

        Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

        * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

        * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

        * Neither the name of Clarius Consulting nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
        */

    #endregion

    internal static class DateTimeEpochExtensions
    {
        /// <summary>
        ///     Converts the given date value to epoch time.
        /// </summary>
        public static ulong ToEpochTime(this DateTime dateTime)
        {
            DateTime date = dateTime.ToUniversalTime();
            TimeSpan ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return Convert.ToUInt64(ts.TotalSeconds);
        }

        /// <summary>
        ///     Converts the given date value to epoch time.
        /// </summary>
        public static ulong ToEpochTime(this DateTimeOffset dateTime)
        {
            DateTimeOffset date = dateTime.ToUniversalTime();
            TimeSpan ts = date - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

            return Convert.ToUInt64(ts.TotalSeconds);
        }

        /// <summary>
        ///     Converts the given epoch time to a <see cref="DateTime" /> with <see cref="DateTimeKind.Utc" /> kind.
        /// </summary>
        public static DateTime ToDateTimeFromEpoch(this ulong secondsSince1970)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);
        }

        /// <summary>
        ///     Converts the given epoch time to a UTC <see cref="DateTimeOffset" />.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffsetFromEpoch(this ulong secondsSince1970)
        {
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(secondsSince1970);
        }
    }
}