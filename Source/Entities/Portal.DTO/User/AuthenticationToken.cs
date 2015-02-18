// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DTO.User
{
    public class AuthenticationToken
    {
        public virtual string Token { get; set; }

        public virtual string Type { get; set; }
    }
}