// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.User;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class ChangePasswordModel : ChangePassword
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        public override string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        public override string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsPasswordsDoesNotMatch")]
        public override string ConfirmPassword { get; set; }
    }
}