// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.Notifications;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class NotificationModel : Notification
    {
        [Required]
        public override string ProjectId { get; set; }

        [Required]
        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Title { get; set; }
    }
}