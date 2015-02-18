// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.Email;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class SendEmailModel : SendEmail
    {
        [Required]
        [EmailList(maxEmailLength: 320, maxListLength: 100)]
        public override List<string> Emails { get; set; }

        [Required]
        [StringLength(320, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Subject { get; set; }

        [Required]
        [StringLength(32768, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Body { get; set; }
    }
}