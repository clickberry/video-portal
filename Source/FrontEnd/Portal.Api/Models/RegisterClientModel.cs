// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.Domain.SubscriptionContext;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class RegisterClientModel : DomainPendingClient
    {
        [Required]
        [Email(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsInvalidEmail")]
        public override string Email { get; set; }

        [Compare("Email", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsEmailsDoesNotMatch")]
        public string ConfirmEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustBeInRange", MinimumLength = 6)]
        public override string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "AccountsPasswordsDoesNotMatch")]
        public string ConfirmPassword { get; set; }

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