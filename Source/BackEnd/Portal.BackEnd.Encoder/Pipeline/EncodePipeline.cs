// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BackEnd.Encoder.Interface;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline
{
    public class EncodePipeline : IEncodePipeline
    {
        private readonly IPipelineStrategy _piplineStrategy;
        private readonly ITokenSourceFactory _tokenSourceFactory;

        public EncodePipeline(IPipelineStrategy pipelineStrategy, ITokenSourceFactory tokenSourceFactory)
        {
            _piplineStrategy = pipelineStrategy;
            _tokenSourceFactory = tokenSourceFactory;
        }

        public void Run()
        {
            CancellationTokenSourceWrapper tokenSource = _tokenSourceFactory.CreateTokenSource();
            IEnumerable<IPipelineStep> stepList = _piplineStrategy.CreateSteps();
            foreach (IPipelineStep pipelineStep in stepList)
            {
                if (pipelineStep.CanExecute())
                {
                    pipelineStep.Execute(tokenSource);
                }
            }
        }
    }
}