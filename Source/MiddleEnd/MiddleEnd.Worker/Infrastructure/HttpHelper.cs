// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Portal.Domain.ProcessedVideoContext;

namespace MiddleEnd.Worker.Infrastructure
{
    public static class HttpHelper
    {
        private static readonly Dictionary<string, ProcessedEntityType> Types;

        static HttpHelper()
        {
            Types = new Dictionary<string, ProcessedEntityType>
            {
                { "task/video", ProcessedEntityType.Video },
                { "task/screenshot", ProcessedEntityType.Screenshot }
            };
        }

        /// <summary>
        ///     Gets task request entity.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static ParsedTaskRequest GetTaskRequest(HttpRequestMessage requestMessage)
        {
            return new ParsedTaskRequest
            {
                Accepts = requestMessage.Headers.Accept
                    .Select(p => GetTaskType(p.MediaType))
                    .Where(p => p != null)
                    .Select(p => p != null ? p.Value : ProcessedEntityType.Video)
                    .ToList(),
                UserAgent = requestMessage.Headers.UserAgent.ToString()
            };
        }

        public static ProcessedEntityType? GetTaskType(string contentType)
        {
            if (Types.ContainsKey(contentType))
            {
                return Types[contentType];
            }

            return null;
        }

        public static string GetContentType(ProcessedEntityType taskType)
        {
            if (Types.ContainsValue(taskType))
            {
                return Types.FirstOrDefault(p => p.Value == taskType).Key;
            }

            return null;
        }

        public static string GetControllerName(ProcessedEntityType taskType)
        {
            return taskType.ToString().ToLowerInvariant();
        }
    }
}