// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.BackEnd.Encoder.Status;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class EncodeStep : PipelineStep<CreatorStepData>
    {
        private readonly IFfmpeg _ffmpeg;
        private readonly IWatchDogTimer _watchDogTimer;

        public EncodeStep(IStepMediator mediator, IEncodeWebClient webClient, IFfmpeg ffmpeg, IWatchDogTimer watchDogTimer) :
            base(mediator, webClient)
        {
            Mediator.AddEncodeStep(this);

            _ffmpeg = ffmpeg;
            _watchDogTimer = watchDogTimer;
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            string contentType = StepData.EncodeStringBuilder.GetContentType();
            string arguments = StepData.EncodeStringBuilder.GetFfmpegArguments();

            RegisterProcessCallback();

            EncoderStatus encoderStatus = StartFfmpeg(tokenSource, arguments);
            EncodeStepData nextStepData = CreateStepData(encoderStatus, contentType);

            SetStatus();

            Mediator.Send(nextStepData, this);
        }

        private void SetStatus()
        {
            if (!_watchDogTimer.IsOverflowing)
            {
                WebClient.SetStatus(100);
            }
        }

        private EncoderStatus StartFfmpeg(CancellationTokenSourceWrapper tokenSource, string arguments)
        {
            _watchDogTimer.Start(tokenSource);
            EncoderStatus encoderStatus = _ffmpeg.Start(arguments, tokenSource, StepData.DataReceivedHandler.ProcessData).Result;
            _watchDogTimer.Stop();

            return encoderStatus;
        }

        private void RegisterProcessCallback()
        {
            StepData.DataReceivedHandler.Register(_watchDogTimer.Reset);
            StepData.DataReceivedHandler.Register(WebClient.SetStatus);
        }

        private EncodeStepData CreateStepData(EncoderStatus encoderStatus, string contentType)
        {
            if (_watchDogTimer.IsOverflowing)
            {
                return new EncodeStepData
                {
                    EncoderState = EncoderState.Hanging,
                    ErrorMessage = "Ffmpeg is Hanging"
                };
            }

            return new EncodeStepData
            {
                EncoderState = encoderStatus.EncoderState,
                ErrorMessage = encoderStatus.ErrorMessage,
                ContentType = contentType
            };
        }
    }
}