// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.Facebook;

namespace Portal.Api.Models
{
    public sealed class FacebookTokenModel : FacebookToken
    {
        [Required]
        public override string Token { get; set; }
    }
}