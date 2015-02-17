// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Data;
using Portal.Domain.BackendContext.Enum;
using Wrappers;

namespace Portal.BackEnd.Encoder.Pipeline.Step
{
    public class CreatorStep : PipelineStep<GettingEntityStepData>
    {
        private readonly IEncodeCreatorFactory _creatorFactory;
        private readonly ITempFileManager _tempFileManager;

        public CreatorStep(IStepMediator mediator, IEncodeWebClient webClient, IEncodeCreatorFactory creatorFactory, ITempFileManager tempFileManager)
            : base(mediator, webClient)
        {
            Mediator.AddCreatorStep(this);

            _creatorFactory = creatorFactory;
            _tempFileManager = tempFileManager;
        }

        public override void Execute(CancellationTokenSourceWrapper tokenSource)
        {
            IEncodeCreator creator = _creatorFactory.Create(StepData.EncodeData);

            IFfmpegParser ffmpegParser = creator.CreateFfmpegParser();
            IDataReceivedHandler dataReceivedHandler = creator.CreateDataReceivedHandler(ffmpegParser);
            IEncodeStringFactory encodeStringFactory = creator.CreateEncodeStringFactory();
            IEncodeStringBuilder encodeStringBuilder = creator.CreateEncodeStringBuilder(_tempFileManager, encodeStringFactory);

            CreatorStepData nextStepData = CreateStepData(dataReceivedHandler, encodeStringBuilder);

            Mediator.Send(nextStepData, this);
        }

        private CreatorStepData CreateStepData(IDataReceivedHandler dataReceivedHandler, IEncodeStringBuilder encodeStringBuilder)
        {
            var nextStepData = new CreatorStepData
            {
                EncoderState = EncoderState.Completed,
                DataReceivedHandler = dataReceivedHandler,
                EncodeStringBuilder = encodeStringBuilder
            };
            return nextStepData;
        }
    }
}