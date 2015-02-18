// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using Configuration;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.DAL.Entities.Storage;
using Portal.DAL.FileSystem;
using Portal.Domain.BackendContext.Enum;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class UploadStep : PipelineStep<EncodeStepData>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileWrapper _fileWrapper;
        private readonly IPortalBackendSettings _settings;
        private readonly ITempFileManager _tempFileManager;

        public UploadStep(IStepMediator mediator, IEncodeWebClient webClient, ITempFileManager tempFileManager, IPortalBackendSettings settings, IFileSystem fileSystem, IFileWrapper fileWrapper)
            : base(mediator, webClient)
        {
            _tempFileManager = tempFileManager;
            _settings = settings;
            _fileSystem = fileSystem;
            _fileWrapper = fileWrapper;

            Mediator.AddUploadStep(this);
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            UploadStepData nextStepData;
            string filePath = _tempFileManager.GetEncodingTempFilePath();

            // Uploads processed file
            try
            {
                StorageFile storageFile;

                using (Stream stream = _fileWrapper.OpenRead(filePath))
                {
                    storageFile = _fileSystem.UploadArtifactFromStreamAsync(
                        new StorageFile(stream, StepData.ContentType)
                        {
                            UserId = _settings.BackendId
                        }).Result;
                }

                nextStepData = new UploadStepData
                {
                    EncoderState = EncoderState.Completed,
                    FileId = storageFile.Id
                };
            }
            catch (AggregateException ex)
            {
                Trace.TraceError("Failed to upload processed file: {0}", ex);

                nextStepData = new UploadStepData
                {
                    EncoderState = EncoderState.Failed,
                    ErrorMessage = ex.GetBaseException().Message
                };
            }

            Mediator.Send(nextStepData, this);
        }
    }
}