// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.User;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class RegisterUserModel : CreateUserData
    {
        [Required]
        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string UserName { get; set; }

        [Required]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        public override string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        public override string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsPasswordsDoesNotMatch")]
        public override string ConfirmPassword { get; set; }
    }
}