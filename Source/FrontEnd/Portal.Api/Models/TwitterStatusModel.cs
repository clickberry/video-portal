// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.Twitter;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class TwitterStatusModel : TwitterStatus
    {
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "TwitterInvalidStatus")]
        public override string Message { get; set; }

        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Token { get; set; }

        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string TokenSecret { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [NonWhiteSpaceString]
        [Url]
        public override string ScreenshotUrl { get; set; }
    }
}