// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.Subscriptions;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class ClientModel : Client
    {
        [Required]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        public override string Email { get; set; }

        [Country(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProfileInvalidCountry")]
        public override string Country { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string CompanyName { get; set; }

        [Ein(OtherPropertyName = "Country", OtherPropertyValue = "US", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProfileInvalidEin")]
        public override string Ein { get; set; }

        [StringLength(320, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Address { get; set; }

        [Required]
        [ZipCode(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProfileInvalidZipCode")]
        public override string ZipCode { get; set; }

        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string ContactPerson { get; set; }
    }
}