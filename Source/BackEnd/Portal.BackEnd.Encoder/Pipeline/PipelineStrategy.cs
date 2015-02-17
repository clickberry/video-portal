// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Configuration;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Pipeline.Step;
using Portal.DAL.FileSystem;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Pipeline
{
    public class PipelineStrategy : IPipelineStrategy
    {
        private readonly IEncodeCreatorFactory _creatorFactory;
        private readonly IFfmpeg _ffmpeg;
        private readonly IFileSystem _fileSystem;
        private readonly IFileWrapper _fileWrapper;
        private readonly IPortalBackendSettings _settings;
        private readonly IStepMediator _stepMediator;
        private readonly ITempFileManager _tempFileManager;
        private readonly IWatchDogTimer _watchDogTimer;
        private readonly IEncodeWebClient _webClient;

        public PipelineStrategy(
            IStepMediator stepMediator, IEncodeWebClient webClient,
            IEncodeCreatorFactory creatorFactory, IFfmpeg ffmpeg,
            IWatchDogTimer watchDogTimer, IFileSystem fileSystem,
            ITempFileManager tempFileManager, IPortalBackendSettings settings,
            IFileWrapper fileWrapper)
        {
            _stepMediator = stepMediator;
            _webClient = webClient;
            _creatorFactory = creatorFactory;
            _ffmpeg = ffmpeg;
            _watchDogTimer = watchDogTimer;
            _fileSystem = fileSystem;
            _tempFileManager = tempFileManager;
            _settings = settings;
            _fileWrapper = fileWrapper;
        }

        public IEnumerable<IPipelineStep> CreateSteps()
        {
            var stepList = new List<IPipelineStep>
            {
                new GetTaskStep(_stepMediator, _webClient),
                new InitializingWebClientStep(_stepMediator, _webClient),
                new GettingEntityStep(_stepMediator, _webClient),
                new DownloadStep(_stepMediator, _tempFileManager, _fileSystem, _fileWrapper),
                new CreatorStep(_stepMediator, _webClient, _creatorFactory, _tempFileManager),
                new EncodeStep(_stepMediator, _webClient, _ffmpeg, _watchDogTimer),
                new UploadStep(_stepMediator, _webClient, _tempFileManager, _settings, _fileSystem, _fileWrapper),
                new FinishStep(_stepMediator, _webClient, _tempFileManager)
            };

            return stepList;
        }
    }
}