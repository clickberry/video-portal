// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.Builder;
using Portal.BLL.Concrete.Multimedia.Factory;
using Portal.Domain.BackendContext.Constant;
using Portal.Domain.EncoderContext;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Infrastructure.ProcessedEntity
{
    public sealed class ProcessedScreenshotGenerator : IProcessedEntityGenerator<DomainProcessedScreenshot>
    {
        private readonly IProcessedScreenshotBuilder _processedScreenshotBuilder;
        private readonly IScreenshotAdjusterParamFactory _screenshotAdjusterParamFactory;

        public ProcessedScreenshotGenerator(IScreenshotAdjusterParamFactory screenshotAdjusterParamFactory, IProcessedScreenshotBuilder processedScreenshotBuilder)
        {
            _screenshotAdjusterParamFactory = screenshotAdjusterParamFactory;
            _processedScreenshotBuilder = processedScreenshotBuilder;
        }

        public List<DomainProcessedScreenshot> Generate(IVideoMetadata videoMetadata)
        {
            ScreenshotAdjusterParam screenshotParam = _screenshotAdjusterParamFactory.CreateScreenshotAdjusterParam(videoMetadata);
            DomainProcessedScreenshot processedScreenshot = _processedScreenshotBuilder.BuildProcessedScreenshot(screenshotParam, MetadataConstant.JpegFormat, ContentType.JpegContent);
            IEnumerable<DomainProcessedScreenshot> processedScreenshots = CreateProcessedScreenshots(processedScreenshot);
            var list = new List<DomainProcessedScreenshot>(processedScreenshots);

            return list;
        }

        private IEnumerable<DomainProcessedScreenshot> CreateProcessedScreenshots(DomainProcessedScreenshot processedScreenshot)
        {
            yield return processedScreenshot;
        }
    }
}