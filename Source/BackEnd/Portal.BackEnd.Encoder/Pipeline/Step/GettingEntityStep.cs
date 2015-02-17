// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.Domain.BackendContext.Entity.Base;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class GettingEntityStep : PipelineStep<TaskStepData>
    {
        public GettingEntityStep(IStepMediator stepMediator, IEncodeWebClient encodeWebClient)
            : base(stepMediator, encodeWebClient)
        {
            stepMediator.AddGettingEntityStep(this);
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            IEncodeData encodeData = WebClient.GetEntity(StepData.TypeOfTask);
            GettingEntityStepData nextStepData = CreateStepData(encodeData);

            Mediator.Send(nextStepData, this);
        }

        private GettingEntityStepData CreateStepData(IEncodeData encodeData)
        {
            var nextStepData = new GettingEntityStepData
            {
                EncoderState = EncoderState.Completed,
                EncodeData = encodeData
            };
            return nextStepData;
        }
    }
}