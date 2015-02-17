// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.User
{
    /// <summary>
    ///     Identity provider data.
    /// </summary>
    public class IpData
    {
        /// <summary>
        ///     Token type.
        /// </summary>
        public virtual TokenType Type { get; set; }

        /// <summary>
        ///     Token value.
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        ///     Token secret value.
        /// </summary>
        public virtual string TokenSecret { get; set; }
    }
}