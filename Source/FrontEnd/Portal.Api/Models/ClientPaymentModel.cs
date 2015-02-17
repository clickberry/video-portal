// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class ClientPaymentModel
    {
        [Required]
        [Range(50, 99999999, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "SubscriptionPaymentInvalidAmount")]
        public int AmountInCents { get; set; }

        [StringLength(200, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public string Description { get; set; }

        [Required]
        public string TokenId { get; set; }
    }
}