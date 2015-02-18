// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Portal.DTO.User;

namespace Portal.Api.Models
{
    public sealed class AuthenticationTokenModel : AuthenticationToken
    {
        public AuthenticationTokenModel()
        {
            Type = "Bearer";
        }

        [JsonProperty("access_token")]
        public override string Token { get; set; }

        [JsonProperty("token_type")]
        public override string Type { get; set; }
    }
}