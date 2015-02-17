// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.DAL.Multimedia;
using Portal.Domain.EncoderContext;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Infrastructure.ProcessedEntity
{
    public class ProcessedEntityManager : IProcessedEntityManager
    {
        private readonly IProcessedEntityGenerator<DomainProcessedScreenshot> _processedScreenshotGenerator;
        private readonly IProcessedEntityGenerator<DomainProcessedVideo> _processedVideoGenerator;
        private readonly Lazy<IVideoMetadataProvider> _videoMetadataProvider;

        public ProcessedEntityManager(Lazy<IVideoMetadataProvider> videoMetadataProvider,
            IProcessedEntityGenerator<DomainProcessedVideo> processedVideoGenerator,
            IProcessedEntityGenerator<DomainProcessedScreenshot> processedScreenshotGenerator)
        {
            _videoMetadataProvider = videoMetadataProvider;
            _processedVideoGenerator = processedVideoGenerator;
            _processedScreenshotGenerator = processedScreenshotGenerator;
        }

        public async Task<ProcessedMediaModel> GetProcessedMediaModel(string fileUri, string projectId)
        {
            if (String.IsNullOrEmpty(fileUri))
            {
                return null;
            }

            var generatorsExceptions = new List<Exception>();
            var videoMetadata = (VideoMetadata)(await _videoMetadataProvider.Value.GetMetadata(fileUri));
            List<DomainProcessedVideo> processedVideos = GetProcessedVideos(videoMetadata, generatorsExceptions);
            List<DomainProcessedScreenshot> processedScreenshots = GetProcessedScreenshots(videoMetadata, generatorsExceptions);

            CheckError(generatorsExceptions, processedVideos, processedScreenshots, projectId);

            return new ProcessedMediaModel(processedVideos, processedScreenshots, videoMetadata);
        }

        private List<DomainProcessedScreenshot> GetProcessedScreenshots(IVideoMetadata videoMetadata, List<Exception> generatorsExceptions)
        {
            List<DomainProcessedScreenshot> processedScreenshots = null;

            try
            {
                processedScreenshots = _processedScreenshotGenerator.Generate(videoMetadata);
            }
            catch (AggregateException e)
            {
                generatorsExceptions.AddRange(e.InnerExceptions);
            }
            catch (Exception e)
            {
                generatorsExceptions.Add(e);
            }
            return processedScreenshots;
        }

        private List<DomainProcessedVideo> GetProcessedVideos(IVideoMetadata videoMetadata, List<Exception> generatorsExceptions)
        {
            List<DomainProcessedVideo> processedVideos = null;

            try
            {
                processedVideos = _processedVideoGenerator.Generate(videoMetadata);
            }
            catch (AggregateException e)
            {
                generatorsExceptions.AddRange(e.InnerExceptions);
            }
            catch (Exception e)
            {
                generatorsExceptions.Add(e);
            }
            return processedVideos;
        }

        private void CheckError(List<Exception> errorList, List<DomainProcessedVideo> processedVideos, List<DomainProcessedScreenshot> processedScreenshots, string projectId)
        {
            if (errorList.Count != 0)
            {
                throw new AggregateException(String.Format("File is not supported. ProjectId is {0}.", projectId), errorList);
            }

            if (processedVideos == null)
            {
                throw new NullReferenceException(String.Format("Failed to receive generated videos. ProjectId is {0}", projectId));
            }

            if (processedScreenshots == null)
            {
                throw new NullReferenceException(String.Format("Failed to receive generated screenshots. ProjectId is {0}", projectId));
            }
        }
    }
}