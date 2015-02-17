// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;

namespace Portal.BackEnd.Encoder.Pipeline.Data
{
    public class CreatorStepData : StepData
    {
        public IDataReceivedHandler DataReceivedHandler { get; set; }

        public IEncodeStringBuilder EncodeStringBuilder { get; set; }

        public string VideoFileId { get; set; }
    }
}