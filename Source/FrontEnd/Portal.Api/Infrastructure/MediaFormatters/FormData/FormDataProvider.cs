// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using Portal.DTO.Common;

namespace Portal.Api.Infrastructure.MediaFormatters.FormData
{
    public class FormDataProvider : IFormDataProvider
    {
        private readonly MultipartFormDataStreamProvider _streamProvider;
        private readonly IFormatterLogger _formatterLogger;

        public FormDataProvider(MultipartFormDataStreamProvider streamProvider, IFormatterLogger formatterLogger)
        {
            _streamProvider = streamProvider;
            _formatterLogger = formatterLogger;
        }

        /// <summary>
        ///     Gets a value indicating whether field exists.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="isRequired"></param>
        /// <returns>Whether field exists.</returns>
        public bool Contains(string name, bool isRequired = false)
        {
            if (_streamProvider.FormData.AllKeys.Any(p => string.Equals(name, p, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (_streamProvider.FileData.Any(p => string.Equals(p.Headers.ContentDisposition.Name.Trim('"'), name, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (isRequired)
            {
                _formatterLogger.LogError(name, "Invalid value.");
            }

            return false;
        }

        /// <summary>
        ///     Gets a string value by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Value.</returns>
        public string GetValue(string name)
        {
            string key = _streamProvider.FormData.AllKeys.FirstOrDefault(p => string.Equals(name, p, StringComparison.OrdinalIgnoreCase));
            return string.IsNullOrEmpty(key) ? null : _streamProvider.FormData[name];
        }

        /// <summary>
        ///     Gets a value by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value.</returns>
        public T GetValue<T>(string name, T defaultValue)
        {
            string key = _streamProvider.FormData.AllKeys.FirstOrDefault(p => string.Equals(name, p, StringComparison.OrdinalIgnoreCase));
            string value = string.IsNullOrEmpty(key) ? null : _streamProvider.FormData[name];

            if (string.IsNullOrEmpty(value))
            {
                _formatterLogger.LogError(name, "Invalid value.");
                return defaultValue;
            }

            try
            {
                if (typeof (T).IsEnum)
                {
                    return (T)Enum.Parse(typeof (T), value, true);
                }

                return (T)Convert.ChangeType(value, typeof (T));
            }
            catch (Exception)
            {
                _formatterLogger.LogError(name, "Invalid value.");
                return defaultValue;
            }
        }

        /// <summary>
        ///     Gets a file entity by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>File.</returns>
        public FileEntity GetFile(string name)
        {
            MultipartFileData fileData;

            // This try/catch is to return 'Bad request' when multiple files with the same name specified
            try
            {
                fileData = _streamProvider.FileData.Single(p => string.Equals(name, p.Headers.ContentDisposition.Name.Trim('"'), StringComparison.OrdinalIgnoreCase));
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            if (!TryAccess(fileData.LocalFileName))
            {
                return null;
            }

            return new FileEntity
            {
                ContentType = fileData.Headers.ContentType.MediaType,
                Name = fileData.Headers.ContentDisposition.FileName.Trim('"'),
                Uri = fileData.LocalFileName,
                Length = new FileInfo(fileData.LocalFileName).Length
            };
        }

        private bool TryAccess(string path)
        {
            // Issue in the async IO:
            // http://aspnetwebstack.codeplex.com/workitem/282

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (File.OpenRead(path))
                    {
                        return true;
                    }
                }
                catch (IOException)
                {
                }
            }

            return false;
        }
    }
}