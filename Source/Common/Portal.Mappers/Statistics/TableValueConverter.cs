// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Text.RegularExpressions;
using Wrappers.Interface;

namespace Portal.Mappers.Statistics
{
    public class TableValueConverter : ITableValueConverter
    {
        private readonly IGuidWrapper _guidWrapper;

        public TableValueConverter(IGuidWrapper guidWrapper)
        {
            _guidWrapper = guidWrapper;
        }

        public string StringsToKey(params string[] args)
        {
            var result = new StringBuilder();
            foreach (string s in args)
            {
                string[] array = s.Split('#', '/', '\\', '?');
                foreach (string ss in array)
                {
                    result.Append(String.Format("{0}_", ss));
                }
                result = TrimEnd(result);
                result.Append(":");
            }
            result = TrimEnd(result);

            return result.ToString();
        }

        public string ArrayToString(string[] array)
        {
            if (array == null)
            {
                return null;
            }

            string result = "";
            foreach (string str in array)
            {
                result += String.Format("{0};", str);
            }
            result = result.TrimEnd(';');

            return result;
        }

        public string UserAgentToProductName(string userAgent)
        {
            if (String.IsNullOrEmpty(userAgent))
            {
                return null;
            }

            string[] result = userAgent.Split('/');
            if (result.Length < 2)
            {
                return null;
            }

            return result[0];
        }

        public string UserAgentToVersion(string userAgent)
        {
            if (String.IsNullOrEmpty(userAgent))
            {
                return null;
            }

            var regex = new Regex(@"[0-9;A-Z;a-z]+/([\d+;.]+)");
            Match match = regex.Match(userAgent);
            if (match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public string DateToPartitionKey(DateTime dateTime)
        {
            const string template = "{0:0000}_{1:00}_{2:00}";
            string result = String.Format(template, dateTime.Year, dateTime.Month, dateTime.Day);

            return result;
        }

        public string DateTimeToTick(DateTime dateTime)
        {
            string result = String.Format("{0:0000000000000000000}", DateTime.MaxValue.Ticks - dateTime.Ticks);

            return result;
        }

        public string DateTimeToComparerTick(DateTime dateTime)
        {
            string result = String.Format("{0:0000000000000000000}T", DateTime.MaxValue.Ticks - dateTime.Ticks); //T - for compare tick & tick with guid

            return result;
        }

        public string DateTimeToTickWithGuid(DateTime dateTime)
        {
            string guid = _guidWrapper.Generate();
            string result = String.Format("{0:0000000000000000000}_{1}", DateTime.MaxValue.Ticks - dateTime.Ticks, guid);

            return result;
        }

        public string ChangeGuidPart(string tickWithGuid)
        {
            string guid = _guidWrapper.Generate();
            string tick = GetTickPart(tickWithGuid);
            string result = String.Format("{0}_{1}", tick, guid);

            return result;
        }

        public string GetTickPart(string tickWithGuid)
        {
            string result = tickWithGuid.Split('_')[0];

            return result;
        }

        public DateTime TickToDateTime(string tick)
        {
            long parsingTick;
            if (long.TryParse(tick, out parsingTick))
            {
                long realTick = DateTime.MaxValue.Ticks - parsingTick;
                return new DateTime(realTick);
            }

            return new DateTime();
        }

        private StringBuilder TrimEnd(StringBuilder stringBuilder)
        {
            int startIndex = stringBuilder.Length - 1;
            return stringBuilder.Remove(startIndex, 1);
        }
    }
}