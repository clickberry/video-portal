// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public abstract class PipelineStepBase<T> : IPipelineStep where T : StepData, new()
    {
        protected PipelineStepBase(IStepMediator mediator, IEncodeWebClient webClient)
        {
            Mediator = mediator;
            WebClient = webClient;
        }

        protected IStepMediator Mediator { get; private set; }

        protected IEncodeWebClient WebClient { get; private set; }

        protected T StepData { get; private set; }

        public void SetData(StepData stepData)
        {
            StepData = stepData as T ?? new T
            {
                EncoderState = stepData.EncoderState,
                ErrorMessage = stepData.ErrorMessage
            };
        }

        public abstract void Execute(CancellationTokenSourceWrapper tokenSource);

        public virtual bool CanExecute()
        {
            return true;
        }
    }
}