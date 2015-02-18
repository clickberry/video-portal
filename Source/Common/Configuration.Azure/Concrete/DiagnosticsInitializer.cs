// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Configures receiving of the diagnostics information.
    /// </summary>
    public sealed class DiagnosticsInitializer : IInitializable
    {
        public void Initialize()
        {
            // Event log setup
            const string myLogName = "Clickberry";
            const string eventSourceName = "Portal .NET";

            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, myLogName);
            }

            // Hack: Otherwise log will have default small size
            const long logSize = 3999936L;
            var newLog = new EventLog
            {
                Source = eventSourceName,
                MaximumKilobytes = logSize // Must be multiple of 64
            };

            Debug.Assert(newLog.MaximumKilobytes == logSize);
        }
    }
}