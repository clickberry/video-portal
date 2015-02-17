// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.Resources.Api;

namespace Portal.Web.Models
{
    public sealed class FeedbackModel
    {
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FeedbackNameRequired")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FeedbackEmailRequired")]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FeedbackInvalidEmail")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FeedbackMessageRequired")]
        public string Message { get; set; }
    }
}