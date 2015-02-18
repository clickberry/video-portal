// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class FinishStep : PipelineStepBase<UploadStepData>
    {
        private readonly ITempFileManager _tempFileManager;

        public FinishStep(IStepMediator mediator, IEncodeWebClient webClient, ITempFileManager tempFileManager) : base(mediator, webClient)
        {
            _tempFileManager = tempFileManager;

            Mediator.AddFinishStep(this);
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            _tempFileManager.DeleteAllTempFiles();
            WebClient.FinishTask(StepData.EncoderState, StepData.FileId, StepData.ErrorMessage);
        }
    }
}