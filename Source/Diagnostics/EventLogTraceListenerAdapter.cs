using System.Diagnostics;

namespace Diagnostics
{
    class EventLogTraceListenerAdapter : TraceListener
    {
        private readonly EventLogTraceListener _eventLogTraceListener;

        public EventLogTraceListenerAdapter(string eventSourceName)
        {
            const string myLogName = "Clickberry";

            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, myLogName);
            }

            var eventLog = new EventLog
            {
                Source = eventSourceName,
                MaximumKilobytes = 3999936L // Must be multiple of 64
            };
            _eventLogTraceListener = new EventLogTraceListener(eventLog);
        }


        public override void Write(string message)
        {
            _eventLogTraceListener.Write(message);
        }

        public override void WriteLine(string message)
        {
            _eventLogTraceListener.WriteLine(message);
        }
    }
}
