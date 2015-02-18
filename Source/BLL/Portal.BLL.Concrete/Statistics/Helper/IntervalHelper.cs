// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BLL.Statistics.Helper;

namespace Portal.BLL.Concrete.Statistics.Helper
{
    public class IntervalHelper : IIntervalHelper
    {
        private static readonly DateTime BeginningOfTime = new DateTime(2013, 01, 31);

        public Interval GetLastDay(DateTime dateTime)
        {
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
            var timeSpan = new TimeSpan(1, 0, 0, 0);
            DateTime start = newDateTime.Subtract(timeSpan);
            DateTime finish = newDateTime;

            return new Interval
            {
                Start = start,
                Finish = finish
            };
        }

        public Interval GetLastWeek(DateTime dateTime)
        {
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
            var timeSpan = new TimeSpan(7, 0, 0, 0);
            DateTime start = newDateTime.Subtract(timeSpan);
            DateTime finish = newDateTime;

            return new Interval
            {
                Start = start,
                Finish = finish
            };
        }

        public Interval GetLastMonth(DateTime dateTime)
        {
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
            var timeSpan = new TimeSpan(30, 0, 0, 0);
            DateTime start = newDateTime.Subtract(timeSpan);
            DateTime finish = newDateTime;

            return new Interval
            {
                Start = start,
                Finish = finish
            };
        }

        public Interval GetAllDays(DateTime dateTime)
        {
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
            DateTime start = BeginningOfTime;
            DateTime finish = newDateTime;

            return new Interval
            {
                Start = start,
                Finish = finish
            };
        }

        public DateTime GetLastDate(int daysBefore, DateTime currentTime)
        {
            TimeSpan timeSpan = TimeSpan.FromDays(daysBefore);
            var newDateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
            DateTime lastDate = newDateTime.Subtract(timeSpan);

            return lastDate;
        }

        public int GetMillisecondsToEndDay(TimeSpan appenedTime, DateTime currentTime)
        {
            TimeSpan timeSpan = new TimeSpan(1, 0, 0, 0).Add(appenedTime);
            var curTimeSpan = new TimeSpan(0, currentTime.Hour, currentTime.Minute, currentTime.Second);
            TimeSpan remainingTime = timeSpan.Subtract(curTimeSpan);

            return (int)remainingTime.TotalMilliseconds;
        }

        public Interval GetInterval(DateTime start, DateTime end)
        {
            return new Interval
            {
                Start = start,
                Finish = end.AddDays(1)
            };
        }
    }
}