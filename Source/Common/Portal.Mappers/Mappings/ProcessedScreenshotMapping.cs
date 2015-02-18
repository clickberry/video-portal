// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Globalization;
using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.Mappers.Mappings
{
    public sealed class ProcessedScreenshotMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<DomainProcessedScreenshot, ProcessedScreenshotEntity>()
                .ConvertUsing(p =>
                {
                    if (p == null)
                    {
                        return null;
                    }

                    return new ProcessedScreenshotEntity
                    {
                        AttemptsCount = p.AttemptsCount,
                        Completed = p.Completed,
                        ContentType = p.ContentType,
                        Created = p.Created,
                        DestinationFileId = p.DestinationFileId,
                        Height = p.ScreenshotParam.ImageHeight,
                        ImageFormat = p.ImageFormat,
                        Modified = p.Modified,
                        Id = p.ObjectId,
                        ProcessingState = (int)p.State,
                        Progress = p.Progress,
                        SourceFileId = p.SourceFileId,
                        Started = p.Started,
                        TaskId = p.TaskId,
                        TimeOffset = p.ScreenshotParam.TimeOffset.ToString(CultureInfo.InvariantCulture),
                        UserId = p.UserId,
                        VideoRotation = p.ScreenshotParam.VideoRotation,
                        Width = p.ScreenshotParam.ImageWidth
                    };
                });


            Mapper.CreateMap<ProcessedScreenshotEntity, DomainProcessedScreenshot>()
                .ConvertUsing(p =>
                {
                    if (p == null)
                    {
                        return null;
                    }

                    return new DomainProcessedScreenshot
                    {
                        AttemptsCount = p.AttemptsCount,
                        Completed = p.Completed,
                        ContentType = p.ContentType,
                        Created = p.Created,
                        DestinationFileId = p.DestinationFileId,
                        SourceFileId = p.SourceFileId,
                        ImageFormat = p.ImageFormat,
                        Modified = p.Modified,
                        ObjectId = p.Id,
                        Progress = p.Progress,
                        ScreenshotParam = new ScreenshotParam
                        {
                            ImageHeight = p.Height,
                            ImageWidth = p.Width,
                            TimeOffset = double.Parse(p.TimeOffset, CultureInfo.InvariantCulture),
                            VideoRotation = p.VideoRotation
                        },
                        Started = p.Started,
                        TaskId = p.TaskId,
                        UserId = p.UserId
                    };
                });
        }
    }
}