// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BLL.Infrastructure;
using Portal.DAL.Entities.Storage;
using Portal.DAL.FileSystem;
using Portal.Domain.BackendContext.Enum;
using Portal.Exceptions.CRUD;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class DownloadStep : PipelineStep<GettingEntityStepData>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileWrapper _fileWrapper;
        private readonly ITempFileManager _tempFileManager;

        public DownloadStep(IStepMediator mediator, ITempFileManager tempFileManager, IFileSystem fileSystem, IFileWrapper fileWrapper)
            : base(mediator, null)
        {
            _tempFileManager = tempFileManager;
            _fileSystem = fileSystem;
            _fileWrapper = fileWrapper;

            Mediator.AddDownloadStep(this);
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            GettingEntityStepData nextStepData;
            string localFile = _tempFileManager.GetOriginalTempFilePath();

            // Downloads file for processing
            try
            {
                using (Stream stream = _fileWrapper.OpenWrite(localFile))
                {
                    _fileSystem.DownloadFileToStreamAsync(
                        new StorageFile
                        {
                            Id = StepData.EncodeData.SourceFileId,
                            Stream = stream
                        }).Wait();
                }

                nextStepData = new GettingEntityStepData
                {
                    EncoderState = EncoderState.Completed,
                    EncodeData = StepData.EncodeData
                };
            }
            catch (AggregateException ex)
            {
                Trace.TraceError("Failed to download file {0} for processing: {1}", StepData.EncodeData.SourceFileId, ex);

                Exception exception = ex.GetBaseException();
                nextStepData = new GettingEntityStepData();

                if (exception is NotFoundException)
                {
                    nextStepData.EncoderState = EncoderState.Deleted;
                }
                else
                {
                    nextStepData.EncoderState = EncoderState.Failed;
                    nextStepData.ErrorMessage = ex.GetBaseException().Message;
                }
            }

            Mediator.Send(nextStepData, this);
        }
    }
}