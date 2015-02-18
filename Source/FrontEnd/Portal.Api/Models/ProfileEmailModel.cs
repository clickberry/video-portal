// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.User;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class ProfileEmailModel : ProfileEmail
    {
        [Required]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        public override string Email { get; set; }
    }
}