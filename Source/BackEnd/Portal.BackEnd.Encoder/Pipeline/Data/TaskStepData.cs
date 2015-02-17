// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.BackendContext.Enum;

namespace Portal.BackEnd.Encoder.Pipeline.Data
{
    public class TaskStepData : StepData
    {
        public string TaskId { get; set; }

        public TypeOfTask TypeOfTask { get; set; }

        public string Resource { get; set; }
    }
}