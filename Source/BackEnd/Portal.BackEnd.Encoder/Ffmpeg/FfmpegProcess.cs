// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Status;
using Portal.Domain.BackendContext.Enum;
using Wrappers;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Ffmpeg
{
    public class FfmpegProcess : IFfmpeg
    {
        private readonly IProcessAsync _process;
        private readonly ITempFileManager _tempFileManager;

        public FfmpegProcess(IProcessAsync process, ITempFileManager tempFileManager)
        {
            _process = process;
            _tempFileManager = tempFileManager;
        }

        public async Task<EncoderStatus> Start(string arguments, CancellationTokenSourceWrapper tokenSource, Action<string> processedData)
        {
            try
            {
                EncoderStatus encoderStatus;
                await _process.Start(arguments, processedData, tokenSource.Token);
                if (_tempFileManager.ExistsEncodingFile())
                {
                    encoderStatus = CreateEncoderStatus(EncoderState.Completed, String.Empty);
                }
                else
                {
                    tokenSource.Cancel();
                    encoderStatus = CreateEncoderStatus(EncoderState.Failed, "Output file was not created.");
                }
                return encoderStatus;
            }
            catch (OperationCanceledException ex)
            {
                tokenSource.Cancel();
                EncoderStatus encoderStatus = CreateEncoderStatus(EncoderState.Cancelled, ex.Message);
                return encoderStatus;
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                EncoderStatus encoderStatus = CreateEncoderStatus(EncoderState.Failed, ex.Message);
                return encoderStatus;
            }
        }

        private EncoderStatus CreateEncoderStatus(EncoderState encoderState, string errorMessage)
        {
            return new EncoderStatus
            {
                EncoderState = encoderState,
                ErrorMessage = errorMessage
            };
        }
    }
}