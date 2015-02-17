// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether value is valid ISO 3166-1 alpha-2.
    ///     http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class CountryAttribute : ValidationAttribute
    {
        private static readonly HashSet<string> Values;

        /// <summary>
        ///     Constructor.
        /// </summary>
        static CountryAttribute()
        {
            Values = new HashSet<string>(GetCountries(), StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> GetCountries()
        {
            return from ri in
                from ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                select new RegionInfo(ci.LCID)
                group ri by ri.TwoLetterISORegionName
                into g
                select g.Key;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var stringValue = (string)value;
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            if (!Values.Contains(stringValue))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return null;
        }
    }
}