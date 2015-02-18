// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Configuration.Azure.TraceListeners
{
    public sealed class JiraTraceListener : TraceListener
    {
        private static readonly Regex FormatCleaner = new Regex(@"\{\d+\}", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex InstanceIdFormat = new Regex(@".+_IN_(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly string _portalVersion;

        public JiraTraceListener(string jiraIssueCollectorUri, string environment, string portalVersion)
        {
            _portalVersion = portalVersion;
            JiraIssueCollectorUri = jiraIssueCollectorUri;
            Priority = 2;
            Environment = environment;
        }

        public string Environment { get; set; }

        public string JiraIssueCollectorUri { get; set; }

        public int Priority { get; set; }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        private static string CleanupFormatString(string format)
        {
            return FormatCleaner.Replace(format, string.Empty).Trim(new[] { ' ', ':' });
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            Trace(eventCache, source, eventType, id, message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            Trace(eventCache, source, eventType, id, format, args);
        }

        private void Trace(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, object[] args = null)
        {
            if (eventType != TraceEventType.Critical && eventType != TraceEventType.Error) return;

            Match match = InstanceIdFormat.Match(RoleEnvironment.CurrentRoleInstance.Id);
            string roleId = match.Success ? match.Groups[1].Value : string.Empty;

            try
            {
                string summary = string.Format("{0}[{1}] failure: {2}",
                    RoleEnvironment.CurrentRoleInstance.Role.Name,
                    roleId,
                    CleanupFormatString(format));

                var description = new StringBuilder();
                //
                description.AppendLine("Event Message:");
                description.AppendLine("{code}");
                description.AppendLine(args == null ? format : string.Format(format, args));
                description.AppendLine("{code}");
                //
                description.AppendFormat("Event Source: {0}\n", source);
                description.AppendFormat("Event Type: {0}\n", eventType);
                description.AppendFormat("Event ID: {0}\n", id);
                //
                description.AppendLine("Event Call Stack:");
                description.AppendLine("{code}");
                description.AppendLine(eventCache.Callstack);
                description.AppendLine("{code}");
                //
                description.AppendFormat("DateTime: {0}\n", eventCache.DateTime);
                description.AppendFormat("Timestamp: {0}\n", eventCache.Timestamp);
                description.AppendFormat("Version: {0}\n", _portalVersion);
                description.AppendFormat("Environment Machine: {0}\n", System.Environment.MachineName);
                description.AppendFormat("Environment User: {0}\n", System.Environment.UserName);
                //
                description.AppendFormat("Process ID: {0}\n", eventCache.ProcessId);
                description.AppendFormat("Thread ID: {0}\n", eventCache.ThreadId);
                Process process = Process.GetCurrentProcess();
                description.AppendFormat("Process Threads Count: {0}\n", process.Threads.Count);
                description.AppendFormat("Process Start Time: {0}\n", process.StartTime);
                description.AppendFormat("Process Working Set: {0}\n", process.WorkingSet64);
                //
                if (HttpContext.Current != null)
                {
                    HttpRequest request = HttpContext.Current.Request;

                    description.AppendFormat("HTTP Request HTTP Method: {0}\n", request.HttpMethod);
                    description.AppendFormat("HTTP Request URL: {0}\n", request.Url);
                    description.AppendFormat("HTTP Request Query String: {0}\n", request.QueryString);
                    description.AppendFormat("HTTP Request User Agent: {0}\n", request.UserAgent);
                    description.AppendFormat("HTTP Request User Host Address: {0}\n", request.UserHostAddress);
                    description.AppendFormat("HTTP Request Is Authenticated: {0}\n", request.IsAuthenticated);
                    description.AppendFormat("HTTP Request User: {0}\n", HttpContext.Current.User.Identity.Name);
                    HttpCookieCollection cookies = request.Cookies;
                    if (cookies.Count > 0)
                    {
                        description.AppendFormat("HTTP Request Cookies: {0}\n", cookies
                            .AllKeys
                            .Select(p =>
                            {
                                HttpCookie cookie = cookies.Get(p);
                                return cookie != null ? string.Format("{0}={1}", p, cookie.Value) : string.Empty;
                            })
                            .Aggregate((p, q) => p + "; " + q));
                    }
                    description.AppendFormat("HTTP Request ContentType: {0}\n", request.ContentType);

                    if (request.Form.Count > 0)
                    {
                        description.AppendLine("HTTP Request Form Values:");
                        foreach (string key in request.Form.AllKeys)
                        {
                            description.AppendFormat("  {0}={1}\n", key, request.Form.Get(key));
                        }
                    }
                }

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("summary", summary),
                    new KeyValuePair<string, string>("environment", Environment),
                    new KeyValuePair<string, string>("description", description.ToString()),
                    new KeyValuePair<string, string>("priority", Priority.ToString(CultureInfo.InvariantCulture))
                });

                using (var client = new HttpClient())
                {
                    client.PostAsync(JiraIssueCollectorUri, content).Wait();
                }
            }
            catch (Exception e)
            {
                Debug.Fail(string.Format("Unable to report issue to jira: {0}", e));
            }
        }
    }
}