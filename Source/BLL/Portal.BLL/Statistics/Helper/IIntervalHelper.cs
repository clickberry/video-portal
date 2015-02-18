// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.BLL.Statistics.Helper
{
    public interface IIntervalHelper
    {
        Interval GetLastDay(DateTime dateTime);
        Interval GetLastWeek(DateTime dateTime);
        Interval GetLastMonth(DateTime dateTime);
        Interval GetAllDays(DateTime dateTime);

        DateTime GetLastDate(int daysBefore, DateTime currentTime);
        int GetMillisecondsToEndDay(TimeSpan appenedTime, DateTime currentTime);
        Interval GetInterval(DateTime start, DateTime end);
    }
}