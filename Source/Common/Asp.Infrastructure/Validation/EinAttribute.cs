// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Checks US Employer Identification Number.
    ///     http://en.wikipedia.org/wiki/Employer_Identification_Number
    /// </summary>
    public sealed class EinAttribute : RegularExpressionAttribute
    {
        public EinAttribute() : base(@"^\d{2}-\d{7}$")
        {
        }

        public string OtherPropertyName { get; set; }

        /// <summary>
        ///     Gets or sets a required property value.
        /// </summary>
        public string OtherPropertyValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return ValidationResult.Success;
            }

            // check prerequisites
            if (context == null ||
                context.ObjectInstance == null ||
                string.IsNullOrEmpty(OtherPropertyName))
            {
                return new ValidationResult("Invalid context or property name.");
            }

            // check whether property exists
            PropertyInfo property = context.ObjectType.GetProperty(OtherPropertyName);
            if (property == null)
            {
                return new ValidationResult(string.Format("Invalid property name '{0}'", OtherPropertyName));
            }

            var actualOtherPropertyValue = property.GetValue(context.ObjectInstance) as string;

            // check equality of actual and expected value
            if (string.Compare(actualOtherPropertyValue, OtherPropertyValue, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return ValidationResult.Success;
            }

            return base.IsValid(value, context);
        }
    }
}