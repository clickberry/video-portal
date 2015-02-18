// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class ResetPasswordModel : RecoveryLinkModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsPasswordsDoesNotMatch")]
        public string Confirmation { get; set; }
    }
}