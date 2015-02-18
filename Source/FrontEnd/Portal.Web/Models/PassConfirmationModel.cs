// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Resources.Api;
using Portal.Resources.Web;

namespace Portal.Web.Models
{
    public class PassConfirmationModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        [Display(ResourceType = typeof (InterfaceMessages), Name = "AccountPassword")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsPasswordsDoesNotMatch")]
        public string Confirmation { get; set; }
    }
}