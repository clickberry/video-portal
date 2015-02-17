// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether string contains white spaces or empty.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class NonWhiteSpaceStringAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Validation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var stringValue = (string)value;
            if (stringValue == null)
            {
                return null;
            }

            if (stringValue.Length == 0 || stringValue.All(Char.IsWhiteSpace))
            {
                string message = FormatErrorMessage(context.DisplayName);
                return new ValidationResult(message);
            }

            return null;
        }
    }
}