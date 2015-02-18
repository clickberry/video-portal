// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Mappers.Statistics
{
    public interface ITableValueConverter
    {
        string StringsToKey(params string[] args);
        string ArrayToString(string[] array);
        string UserAgentToProductName(string userAgent);
        string UserAgentToVersion(string userAgent);
        string DateToPartitionKey(DateTime dateTime);
        string DateTimeToTick(DateTime dateTime);
        string DateTimeToComparerTick(DateTime dateTime);
        string DateTimeToTickWithGuid(DateTime dateTime);
        string ChangeGuidPart(string tickWithGuid);
        string GetTickPart(string tickWithGuid);
        DateTime TickToDateTime(string tick);
    }
}