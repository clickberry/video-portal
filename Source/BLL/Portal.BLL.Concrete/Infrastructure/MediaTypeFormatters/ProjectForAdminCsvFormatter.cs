// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Portal.DTO.Admin;

namespace Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters
{
    /// <summary>
    ///     Csv media formatter for ProjectForAdmin objects.
    /// </summary>
    public class ProjectForAdminCsvFormatter : MediaTypeFormatter
    {
        private const string CsvFileName = "videos.csv";


        public ProjectForAdminCsvFormatter()
        {
            // Add the supported media type.
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));

            // support for QueryStringMapping
            this.AddQueryStringMapping("$format", "csv", "text/csv");
        }


        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == typeof (AdminProject) || typeof (IEnumerable<AdminProject>).IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, new CancellationToken());
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            if (type == typeof (AdminProject))
            {
                var project = (AdminProject)value;
                await WriteAsync(project, writeStream, cancellationToken);
            }
            else if (typeof (IEnumerable<AdminProject>).IsAssignableFrom(type))
            {
                var projects = (IEnumerable<AdminProject>)value;
                foreach (AdminProject project in projects)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await WriteAsync(project, writeStream, cancellationToken);
                }
            }
            else
            {
                throw new NotSupportedException("Cannot serialize type");
            }
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);

            headers.Add("Content-Disposition", string.Format("attachment; filename={0}", CsvFileName));
        }

        #region helpers

        private static async Task WriteAsync(AdminProject project, Stream stream, CancellationToken cancellationToken)
        {
            var stringWriter = new StringWriter();
            await stringWriter.WriteLineAsync(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                project.Id.ToCsvValue(),
                project.Name.ToCsvValue(),
                project.Created.ToCsvValue(),
                project.UserId.ToCsvValue(),
                project.UserName.ToCsvValue(),
                project.ProductType.ToCsvValue(),
                project.Product.ToCsvValue()));

            byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        #endregion
    }
}