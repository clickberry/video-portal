// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using Portal.BLL.Services;
using Portal.Domain.MailerContext;
using Portal.Resources.Emails;

namespace Configuration.Azure.TraceListeners
{
    public sealed class EmailTraceListener : TraceListener
    {
        private static readonly Regex FormatCleaner = new Regex(@"\{\d+\}", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex InstanceIdFormat = new Regex(@".+_IN_(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly IEmailSenderService _emailSender;
        private readonly string _portalVersion;
        private readonly IPortalSettings _settings;

        public EmailTraceListener(string environment, string portalVersion, IEmailSenderService emailSender, IPortalSettings settings)
        {
            _portalVersion = portalVersion;
            _emailSender = emailSender;
            _settings = settings;
            Environment = environment;
        }

        public string Environment { get; set; }

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
                string subject = string.Format("{0}[{1}] failure: {2}",
                    RoleEnvironment.CurrentRoleInstance.Role.Name,
                    roleId,
                    CleanupFormatString(format));

                var message = new StringBuilder();
                //
                message.AppendFormat("Environment: {0}<br/>", Environment);
                //
                message.AppendLine("Event Message:<br/>");
                message.AppendLine("<pre>");
                message.AppendLine(args == null ? format : string.Format(format, args));
                message.AppendLine("</pre>");
                //
                message.AppendFormat("Event Source: {0}<br/>", source);
                message.AppendFormat("Event Type: {0}<br/>", eventType);
                message.AppendFormat("Event ID: {0}<br/>", id);
                //
                message.AppendLine("Event Call Stack:<br/>");
                message.AppendLine("<pre>");
                message.AppendLine(eventCache.Callstack);
                message.AppendLine("</pre>");
                //
                message.AppendFormat("DateTime: {0}<br/>", eventCache.DateTime);
                message.AppendFormat("Timestamp: {0}<br/>", eventCache.Timestamp);
                message.AppendFormat("Version: {0}<br/>", _portalVersion);
                message.AppendFormat("Environment Machine: {0}<br/>", System.Environment.MachineName);
                message.AppendFormat("Environment User: {0}<br/>", System.Environment.UserName);
                //
                message.AppendFormat("Process ID: {0}<br/>", eventCache.ProcessId);
                message.AppendFormat("Thread ID: {0}<br/>", eventCache.ThreadId);
                Process process = Process.GetCurrentProcess();
                message.AppendFormat("Process Threads Count: {0}<br/>", process.Threads.Count);
                message.AppendFormat("Process Start Time: {0}<br/>", process.StartTime);
                message.AppendFormat("Process Working Set: {0}<br/>", process.WorkingSet64);
                //
                if (HttpContext.Current != null)
                {
                    HttpRequest request = HttpContext.Current.Request;

                    message.AppendFormat("HTTP Request HTTP Method: {0}<br/>", request.HttpMethod);
                    message.AppendFormat("HTTP Request URL: {0}<br/>", request.Url);
                    message.AppendFormat("HTTP Request Query String: {0}<br/>", request.QueryString);
                    message.AppendFormat("HTTP Request User Agent: {0}<br/>", request.UserAgent);
                    message.AppendFormat("HTTP Request User Host Address: {0}<br/>", request.UserHostAddress);
                    message.AppendFormat("HTTP Request Is Authenticated: {0}<br/>", request.IsAuthenticated);
                    message.AppendFormat("HTTP Request User: {0}<br/>", HttpContext.Current.User.Identity.Name);
                    HttpCookieCollection cookies = request.Cookies;
                    if (cookies.Count > 0)
                    {
                        message.AppendFormat("HTTP Request Cookies: {0}<br/>", cookies
                            .AllKeys
                            .Select(p =>
                            {
                                HttpCookie cookie = cookies.Get(p);
                                return cookie != null ? string.Format("{0}={1}", p, cookie.Value) : string.Empty;
                            })
                            .Aggregate((p, q) => p + "; " + q));
                    }
                    message.AppendFormat("HTTP Request ContentType: {0}<br/>", request.ContentType);

                    if (request.Form.Count > 0)
                    {
                        message.AppendLine("HTTP Request Form Values:<br/>");
                        foreach (string key in request.Form.AllKeys)
                        {
                            message.AppendFormat("  {0}={1}<br/>", key, request.Form.Get(key));
                        }
                    }
                }

                // Build email message
                var email = new SendEmailDomain
                {
                    Address = _settings.EmailAddressAlerts,
                    DisplayName = Emails.SenderDisplayName,
                    Body = message.ToString(),
                    Emails = new List<string> { _settings.EmailAddressErrors },
                    Subject = subject
                };

                // Send
                _emailSender.SendEmailAsync(email).Wait();
            }
            catch (Exception e)
            {
                Debug.Fail(string.Format("Unable to send e-mail: {0}", e));
            }
        }
    }
}