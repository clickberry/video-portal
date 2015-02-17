// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Resources.Api;
using Portal.Resources.Web;

namespace Portal.Web.Models
{
    public sealed class ChangePasswordModel
    {
        public string UserName { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        [Display(ResourceType = typeof (InterfaceMessages), Name = "AccountPassword")]
        public string UserPassword { get; set; }
    }
}