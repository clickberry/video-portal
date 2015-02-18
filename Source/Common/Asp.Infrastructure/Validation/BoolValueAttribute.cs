// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether values is true or false string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class BoolValueAttribute : ValidationAttribute
    {
        private readonly List<string> _values;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public BoolValueAttribute()
        {
            _values = new List<string> { "true", "false" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var stringValue = (string)value;
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            if (_values.Contains(stringValue.ToLowerInvariant()))
            {
                return null;
            }

            string message = FormatErrorMessage(context.DisplayName);
            return new ValidationResult(message);
        }
    }
}