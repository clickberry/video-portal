// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace LinkTracker.Api.Models
{
    public class TrackingUrlModel
    {
        [Required]
        public string ProjectId { get; set; }

        [Required]
        public string SubscriptionId { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}