// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Data;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class GetTaskStep : PipelineStepBase<StepData>
    {
        public GetTaskStep(IStepMediator pipelineStepMediator, IEncodeWebClient webClient) : base(pipelineStepMediator, webClient)
        {
            Mediator.AddGetTaskStep(this);
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            TaskData taskData = WebClient.GetTask();
            TaskStepData nextStepData = CreateStepData(taskData);

            Mediator.Send(nextStepData, this);
        }

        private TaskStepData CreateStepData(TaskData taskData)
        {
            var nextStepData = new TaskStepData
            {
                EncoderState = EncoderState.Completed,
                Resource = taskData.Resource,
                TaskId = taskData.Id,
                TypeOfTask = taskData.Type
            };
            return nextStepData;
        }
    }
}