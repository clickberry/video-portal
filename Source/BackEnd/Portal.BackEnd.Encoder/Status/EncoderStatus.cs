// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.BackendContext.Enum;

namespace Portal.BackEnd.Encoder.Status
{
    public class EncoderStatus
    {
        public EncoderState EncoderState { get; set; }

        public string ErrorMessage { get; set; }
    }
}