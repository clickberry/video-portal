// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters
{
    public static class CsvExtensions
    {
        private static readonly char[] SpecialChars = { ',', '\n', '\r', '"' };

        /// <summary>
        ///     Escapes value for using in CSV format
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToCsvValue(this object o)
        {
            if (o == null)
            {
                return string.Empty;
            }

            string field = o.ToString();
            if (field.IndexOfAny(SpecialChars) == -1)
            {
                return field;
            }

            // Delimit the entire field with quotes and replace embedded quotes with "".
            return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
        }
    }
}