// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Asp.Infrastructure.Validation
{
    public class EmailListAttribute : ValidationAttribute
    {
        private readonly EmailAttribute _emailAttribute = new EmailAttribute();
        private readonly int _maxEmailLength;
        private readonly int _maxListLength;

        public EmailListAttribute(int maxEmailLength, int maxListLength)
        {
            _maxEmailLength = maxEmailLength;
            _maxListLength = maxListLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var innerValue = value as List<string>;
            if (innerValue == null || innerValue.Count > _maxListLength)
                return new ValidationResult(validationContext.DisplayName);
            if (innerValue.Any(email => !_emailAttribute.IsValid(email)
                                        || string.IsNullOrWhiteSpace(email)
                                        || email.Length > _maxEmailLength))
            {
                return new ValidationResult(validationContext.DisplayName);
            }
            return ValidationResult.Success;
        }
    }
}