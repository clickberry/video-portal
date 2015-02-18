// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.Twitter;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class TwitterRequestModel : TwitterRequest
    {
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "TwitterInvalidToken")]
        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Token { get; set; }

        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "TwitterInvalidTokenSecret")]
        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string TokenSecret { get; set; }
    }
}