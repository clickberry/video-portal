// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.Resources.Api;

namespace Portal.Web.Models
{
    public sealed class PasswordRecoveryModel
    {
        [Required]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}