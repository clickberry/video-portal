// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.User;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    /// <summary>
    ///     Claims authentication model.
    /// </summary>
    public sealed class SessionIpModel : IpData
    {
        /// <summary>
        ///     Token type.
        /// </summary>
        public override TokenType Type { get; set; }

        /// <summary>
        ///     Token value.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidToken")]
        public override string Token { get; set; }

        /// <summary>
        ///     Token secret value.
        /// </summary>
        public override string TokenSecret { get; set; }
    }
}