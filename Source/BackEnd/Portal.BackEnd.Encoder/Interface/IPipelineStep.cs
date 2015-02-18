// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Pipeline.Data;
using Wrappers;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IPipelineStep
    {
        void Execute(CancellationTokenSourceWrapper tokenSource);

        void SetData(StepData stepData);
        bool CanExecute();
    }
}