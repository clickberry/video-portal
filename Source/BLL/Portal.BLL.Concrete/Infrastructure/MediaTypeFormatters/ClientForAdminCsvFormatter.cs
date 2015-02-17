// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    ///     Csv media formatter for ClientForAdmin objects.
    /// </summary>
    public class ClientForAdminCsvFormatter : MediaTypeFormatter
    {
        private const string CsvFileName = "clients.csv";


        public ClientForAdminCsvFormatter()
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
            if (type == typeof (AdminClient) || typeof (IEnumerable<AdminClient>).IsAssignableFrom(type))
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
            if (type == typeof (AdminClient))
            {
                var client = (AdminClient)value;
                await WriteAsync(client, writeStream, cancellationToken);
            }
            else if (typeof (IEnumerable<AdminClient>).IsAssignableFrom(type))
            {
                var clients = (IEnumerable<AdminClient>)value;
                foreach (AdminClient client in clients)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await WriteAsync(client, writeStream, cancellationToken);
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

        private static async Task WriteAsync(AdminClient client, Stream stream, CancellationToken cancellationToken)
        {
            var stringWriter = new StringWriter();
            await stringWriter.WriteLineAsync(string.Format("{0},{1},{2},{3},{4},{5}",
                client.Id.ToCsvValue(),
                client.Email.ToCsvValue(),
                client.Name.ToCsvValue(),
                client.Created.ToCsvValue(),
                (client.Balance/100).ToCsvValue(),
                client.State.ToCsvValue()));

            byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        #endregion
    }
}