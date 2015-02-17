// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Portal.Api.Infrastructure.MediaFormatters
{
    /// <summary>
    ///     This formatter is needed for avoiding 500 error when MultipartFormFormatter fails.
    /// </summary>
    public class AnyOtherTypeFormFormatter : MediaTypeFormatter
    {
        public AnyOtherTypeFormFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            taskCompletionSource.SetResult(null);
            return taskCompletionSource.Task;
        }
    }
}