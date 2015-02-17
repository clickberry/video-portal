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
using Portal.Domain.Admin;
using Portal.Domain.ProfileContext;
using Portal.DTO.Admin;

namespace Portal.BLL.Concrete.Infrastructure.MediaTypeFormatters
{
    /// <summary>
    ///     Csv media formatter for ProjectForAdmin objects.
    /// </summary>
    public class UserForAdminCsvFormatter : MediaTypeFormatter
    {
        private const string CsvFileName = "users.csv";

        public UserForAdminCsvFormatter()
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
            if (type == typeof (AdminUser) || typeof (IEnumerable<AdminUser>).IsAssignableFrom(type))
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
            if (type == typeof (AdminUser))
            {
                var user = (AdminUser)value;
                await WriteAsync(user, writeStream, cancellationToken);
            }
            else if (typeof (IEnumerable<AdminUser>).IsAssignableFrom(type))
            {
                var users = (IEnumerable<AdminUser>)value;
                foreach (AdminUser user in users)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await WriteAsync(user, writeStream, cancellationToken);
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

        private static async Task WriteAsync(AdminUser user, Stream stream, CancellationToken cancellationToken)
        {
            var stringWriter = new StringWriter();

            // Memberships
            var membershipsBuilder = new StringBuilder();
            var memberships = user.Memberships;
            if (!string.IsNullOrEmpty(user.Email))
            {
                memberships.Insert(0, new DomainUserMembershipForAdmin { Identity = user.Email, Provider = ProviderType.Email.ToString() });
            }
            foreach (DomainUserMembershipForAdmin membership in memberships)
            {
                membershipsBuilder.AppendFormat("{0}: {1};", membership.Provider, membership.Identity);
            }

            // Storage space
            var usedStorageSpaceMegabytes = (int)Math.Round((double)user.UsedStorageSpace/1024/1024);
            var maximumStorageSpaceMegabytes = (int)Math.Round((double)user.MaximumStorageSpace/1024/1024);

            await stringWriter.WriteLineAsync(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                user.AppName.ToCsvValue(),
                user.Id.ToCsvValue(),
                user.UserName.ToCsvValue(),
                user.Created.ToCsvValue(),
                membershipsBuilder.ToCsvValue(),
                usedStorageSpaceMegabytes.ToCsvValue(),
                maximumStorageSpaceMegabytes.ToCsvValue(),
                user.VideosCount.ToCsvValue(),
                user.ProductName.ToCsvValue()));

            byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        #endregion
    }
}