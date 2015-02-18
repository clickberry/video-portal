// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Portal.BLL.Services;
using Portal.Domain;

namespace Portal.BLL.Concrete.Services
{
    public abstract class CsvPublishServiceBase : ICsvPublishService
    {
        private const string CsvContainerName = "csvexports";

        private readonly CloudBlobClient _blobClient;
        private readonly MediaTypeFormatter _mediaTypeFormatter;
        protected int CsvBatchSize = 1000;

        protected CsvPublishServiceBase(CloudBlobClient blobClient,
            MediaTypeFormatter mediaTypeFormatter)
        {
            if (mediaTypeFormatter == null)
            {
                throw new ArgumentNullException("mediaTypeFormatter");
            }
            _mediaTypeFormatter = mediaTypeFormatter;

            if (blobClient == null)
            {
                throw new ArgumentNullException("blobClient");
            }

            _blobClient = blobClient;
        }

        public Uri GetAccessCsvUri(string fileName)
        {
            // Get container reference
            CloudBlobContainer container = _blobClient.GetContainerReference(CsvContainerName);
            container.CreateIfNotExists();

            // Get blob reference
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            // Get the SAS token to use for blob access
            var policy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24)
            };

            string blobToken = container.GetSharedAccessSignature(policy);
            var uri = new Uri(blob.Uri.AbsoluteUri + blobToken);

            return uri;
        }

        public async Task PublishAsync(DataQueryOptions filter, string fileName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                // Get container reference
                CloudBlobContainer container = _blobClient.GetContainerReference(CsvContainerName);
                await container.CreateIfNotExistsAsync(cancellationTokenSource.Token);

                // Delete old if exists
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
                await blob.DeleteIfExistsAsync(cancellationTokenSource.Token);

                // Create temp file and serialize data to it
                using (var stream = new MemoryStream())
                {
                    // Prepare data filter
                    int limit = filter.Take.HasValue && filter.Take.Value > 0 ? filter.Take.Value : Int32.MaxValue;

                    if (filter.Take.HasValue && filter.Take.Value > 0)
                    {
                        // limit specified
                        if (filter.Take.Value > CsvBatchSize)
                        {
                            // splitting data into batches
                            filter.Take = CsvBatchSize;
                        }
                    }
                    else
                    {
                        filter.Take = CsvBatchSize;
                    }

                    filter.Skip = filter.Skip.HasValue && filter.Skip.Value > 0 ? filter.Skip.Value : 0;

                    // Adding utf-8 BOM. Details http://stackoverflow.com/questions/4414088/how-to-getbytes-in-c-sharp-with-utf8-encoding-with-bom/4414118#4414118
                    byte[] bom = Encoding.UTF8.GetPreamble();
                    await stream.WriteAsync(bom, 0, bom.Length, cancellationTokenSource.Token);

                    // Batch serialization
                    var serializationTokenSource = new CancellationTokenSource();
                    while (!cancellationTokenSource.IsCancellationRequested && !serializationTokenSource.IsCancellationRequested)
                    {
                        // loading batch
                        object data = await OnDataRequest(filter, serializationTokenSource);

                        // serializing batch
                        await _mediaTypeFormatter.WriteToStreamAsync(data.GetType(), data, stream, null, null, cancellationTokenSource.Token);

                        if (filter.Skip + filter.Take == limit)
                        {
                            // all loaded
                            break;
                        }

                        // next batch
                        filter.Skip += CsvBatchSize;
                        if (filter.Skip + CsvBatchSize >= limit)
                        {
                            filter.Take = limit - filter.Skip;
                        }
                    }

                    // Upload result to blob
                    stream.Position = 0;
                    await blob.UploadFromStreamAsync(stream, cancellationTokenSource.Token);
                }

                // Set content type
                await blob.FetchAttributesAsync(cancellationTokenSource.Token);
                blob.Properties.ContentType = "text/csv";
                await blob.SetPropertiesAsync(cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to publish csv file: {0}", e);
            }
        }

        protected abstract Task<object> OnDataRequest(DataQueryOptions filter, CancellationTokenSource cancellationTokenSource);
    }
}