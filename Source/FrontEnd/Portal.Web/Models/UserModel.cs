// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.Domain.ProfileContext;
using Portal.Resources.Api;

namespace Portal.Web.Models
{
    public sealed class UserModel : TokenData
    {
        [Required]
        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Name { get; set; }

        [Required]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        public override string Email { get; set; }
    }
}