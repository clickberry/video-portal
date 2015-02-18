// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.Domain.BackendContext.Enum;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public abstract class PipelineStep<T> : PipelineStepBase<T> where T : StepData, new()
    {
        protected PipelineStep(IStepMediator mediator, IEncodeWebClient webClient) : base(mediator, webClient)
        {
        }

        public override bool CanExecute()
        {
            if (StepData.EncoderState != EncoderState.Completed)
            {
                StepData nextStepData = CreateStepData();

                Mediator.Send(nextStepData, this);

                return false;
            }
            return true;
        }

        private StepData CreateStepData()
        {
            var nextStepData = new StepData
            {
                EncoderState = StepData.EncoderState,
                ErrorMessage = StepData.ErrorMessage
            };
            return nextStepData;
        }
    }
}