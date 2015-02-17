// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether value is valid video type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class VideoTypeAttribute : ValidationAttribute
    {
        private readonly List<string> _values;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public VideoTypeAttribute()
        {
            _values = new List<string> { "file", "sha-1" };
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