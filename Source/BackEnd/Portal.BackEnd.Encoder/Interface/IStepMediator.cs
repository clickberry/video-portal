// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Pipeline.Data;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IStepMediator
    {
        void Send(StepData stepData, IPipelineStep pipelineStep);

        void AddGetTaskStep(IPipelineStep pipelineStep);

        void AddCreatorStep(IPipelineStep pipelineStep);

        void AddEncodeStep(IPipelineStep pipelineStep);

        void AddFinishStep(IPipelineStep pipelineStep);

        void AddDownloadStep(IPipelineStep pipelineStep);

        void AddInitializingWebClientStep(IPipelineStep pipelineStep);

        void AddGettingEntityStep(IPipelineStep pipelineStep);

        void AddUploadStep(IPipelineStep pipelineStep);
    }
}