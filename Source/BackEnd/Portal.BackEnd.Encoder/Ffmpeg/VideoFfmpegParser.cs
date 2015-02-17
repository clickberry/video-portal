// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using System.Text.RegularExpressions;
using Portal.BackEnd.Encoder.Interface;

namespace Portal.BackEnd.Encoder.Ffmpeg
{
    public class VideoFfmpegParser : IFfmpegParser
    {
        private readonly Regex _durationRegex = new Regex(@"Duration:\s*([0-9]+).([0-9]+).([0-9.]+)");
        private readonly Regex _encodeTimeRegex = new Regex(@"time=([0-9]+).([0-9]+).([0-9.]+)");

        public double ParseDuration(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            Match match = _durationRegex.Match(str);
            return ParseTimeString(match);
        }

        public double ParseEncodeTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            Match match = _encodeTimeRegex.Match(str);
            return ParseTimeString(match);
        }

        private double ParseTimeString(Match match)
        {
            if (match.Groups.Count == 4)
            {
                int hour = int.Parse(match.Groups[1].Value)*3600;
                int min = int.Parse(match.Groups[2].Value)*60;
                double sec = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                return hour + min + sec;
            }
            return 0;
        }
    }
}