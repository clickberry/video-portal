// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Portal.Api.Infrastructure.MediaFormatters.FormData;
using Portal.Api.Infrastructure.MediaFormatters.ModelFactories;

namespace Portal.Api.Infrastructure.MediaFormatters
{
    /// <summary>
    ///     Parse input stream as a models.
    /// </summary>
    public sealed class MultipartFormFormatter : MediaTypeFormatter
    {
        private readonly HttpMultipartForm _multipartForm;
        private readonly string _uploadPath;

        /// <summary>
        ///     Contructor.
        /// </summary>
        public MultipartFormFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));

            _uploadPath = Path.GetTempPath();
            _multipartForm = new HttpMultipartForm();
        }

        /// <summary>
        ///     Gets a value indiating whether type can be readed.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanReadType(Type type)
        {
            return _multipartForm.CanRead(type);
        }

        /// <summary>
        ///     Gets a value indicating whether type can be writed.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanWriteType(Type type)
        {
            return false;
        }

        /// <summary>
        ///     Reads input stream as ProjectModel class.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="readStream">Input stream.</param>
        /// <param name="content"></param>
        /// <param name="formatterLogger"></param>
        /// <returns></returns>
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var provider = new MultipartFormDataStreamProvider(_uploadPath);

            try
            {
                await content.ReadAsMultipartAsync(provider);
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Failed multipart message reading: {0}", e);
                return null;
            }

            return _multipartForm.GetModel(type, new FormDataProvider(provider, formatterLogger));
        }
    }
}