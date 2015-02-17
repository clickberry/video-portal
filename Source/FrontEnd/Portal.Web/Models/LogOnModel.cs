// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.User;
using Portal.Resources.Api;
using Portal.Resources.Web;

namespace Portal.Web.Models
{
    public sealed class LogOnModel : LoginUserData
    {
        [Required]
        [Display(ResourceType = typeof (InterfaceMessages), Name = "AccountEmail")]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        public override string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        [Display(ResourceType = typeof (InterfaceMessages), Name = "AccountPassword")]
        public override string Password { get; set; }

        [Display(ResourceType = typeof (InterfaceMessages), Name = "AccountRememberMe")]
        public bool RememberMe { get; set; }
    }
}