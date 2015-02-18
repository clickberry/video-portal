// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;

namespace MediaInfoLibrary
{
    public static class MediaInfoExtentions
    {
        private static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;

        public static async Task<string> GetStringValue(this IMediaInfo mediaInfo, StreamKind streamKind, string parameter)
        {
            return (await mediaInfo.Get(streamKind, 0, parameter)).Trim();
        }

        public static async Task<int> GetIntegerValue(this IMediaInfo mediaInfo, StreamKind streamKind, string parameter)
        {
            string textValue = await mediaInfo.Get(streamKind, 0, parameter);
            int integerValue;
            int.TryParse(textValue, NumberStyles.Integer, CultureInfo, out integerValue);
            return integerValue;
        }

        public static async Task<long> GetDurationValue(this IMediaInfo mediaInfo, StreamKind streamKind, string parameter)
        {
            string textValue = await mediaInfo.Get(streamKind, 0, parameter);
            TimeSpan timeSpanValue;
            TimeSpan.TryParse(textValue, CultureInfo, out timeSpanValue);
            return (long)timeSpanValue.TotalMilliseconds;
        }

        public static async Task<DateTime> GetDateValue(this IMediaInfo mediaInfo, StreamKind streamKind, string parameter)
        {
            string textValue = await mediaInfo.Get(streamKind, 0, parameter);
            textValue = textValue.Replace("UTC ", string.Empty);
            DateTime dateValue;
            DateTime.TryParse(textValue, CultureInfo, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dateValue);
            return dateValue;
        }

        public static async Task<double> GetDoubleValue(this IMediaInfo mediaInfo, StreamKind streamKind, string parameter)
        {
            string textValue = await mediaInfo.Get(streamKind, 0, parameter);
            double doubleValue;
            double.TryParse(textValue, NumberStyles.Float, CultureInfo, out doubleValue);
            return doubleValue;
        }

        public static async Task<long> GetLongValue(this IMediaInfo mediaInfo, StreamKind streamKind, string parameter)
        {
            string textValue = await mediaInfo.Get(streamKind, 0, parameter);
            long longValue;
            long.TryParse(textValue, NumberStyles.Integer, CultureInfo, out longValue);
            return longValue;
        }
    }
}