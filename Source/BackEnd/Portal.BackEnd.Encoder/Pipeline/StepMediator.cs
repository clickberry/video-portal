// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;

namespace Portal.BackEnd.Encoder.Pipeline
{
    public class StepMediator : IStepMediator
    {
        private IPipelineStep _creatorStep;
        private IPipelineStep _downloadStep;
        private IPipelineStep _encodeStep;
        private IPipelineStep _finishStep;
        private IPipelineStep _getTaskStep;
        private IPipelineStep _gettingEntityStep;
        private IPipelineStep _initializingWebClientStep;
        private IPipelineStep _uploadStep;

        public void Send(StepData stepData, IPipelineStep pipelineStep)
        {
            if (pipelineStep == _getTaskStep)
            {
                _initializingWebClientStep.SetData(stepData);
                _gettingEntityStep.SetData(stepData);
            }

            else if (pipelineStep == _gettingEntityStep)
            {
                _downloadStep.SetData(stepData);
            }

            else if (pipelineStep == _downloadStep)
            {
                _creatorStep.SetData(stepData);
            }

            else if (pipelineStep == _creatorStep)
            {
                _encodeStep.SetData(stepData);
            }

            else if (pipelineStep == _encodeStep)
            {
                _uploadStep.SetData(stepData);
            }

            else if (pipelineStep == _uploadStep)
            {
                _finishStep.SetData(stepData);
            }
        }

        public void AddGetTaskStep(IPipelineStep pipelineStep)
        {
            _getTaskStep = pipelineStep;
        }

        public void AddCreatorStep(IPipelineStep pipelineStep)
        {
            _creatorStep = pipelineStep;
        }

        public void AddEncodeStep(IPipelineStep pipelineStep)
        {
            _encodeStep = pipelineStep;
        }

        public void AddFinishStep(IPipelineStep pipelineStep)
        {
            _finishStep = pipelineStep;
        }

        public void AddDownloadStep(IPipelineStep pipelineStep)
        {
            _downloadStep = pipelineStep;
        }

        public void AddInitializingWebClientStep(IPipelineStep pipelineStep)
        {
            _initializingWebClientStep = pipelineStep;
        }

        public void AddGettingEntityStep(IPipelineStep pipelineStep)
        {
            _gettingEntityStep = pipelineStep;
        }

        public void AddUploadStep(IPipelineStep pipelineStep)
        {
            _uploadStep = pipelineStep;
        }
    }
}