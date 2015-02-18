// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.BackendContext.Entity;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProcessedVideoContext.States;

namespace Portal.Mappers.Mappings
{
    public sealed class ProcessedVideoMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<DomainProcessedVideo, ProcessedVideoEntity>()
                .ConvertUsing(video =>
                {
                    if (video == null)
                    {
                        return null;
                    }

                    return new ProcessedVideoEntity
                    {
                        AttemptsCount = video.AttemptsCount,
                        AudioBitrate = video.AudioParam.AudioBitrate,
                        AudioCodec = video.AudioParam.AudioCodec,
                        Completed = video.Completed,
                        ContentType = video.ContentType,
                        Created = video.Created,
                        DestinationFileId = video.DestinationFileId,
                        FrameRate = video.VideoParam.FrameRate,
                        IsAudioCopy = video.IsAudioCopy,
                        IsVideoCopy = video.IsVideoCopy,
                        KeyFrameRate = video.VideoParam.KeyFrameRate,
                        MediaContainer = video.VideoParam.MediaContainer,
                        Modified = video.Modified,
                        Id = video.ObjectId,
                        OutputFormat = video.OutputFormat,
                        ProcessingState = (int)video.State,
                        Progress = video.Progress,
                        SourceFileId = video.SourceFileId,
                        Started = video.Started,
                        TaskId = video.TaskId,
                        UserId = video.UserId,
                        VideoBitrate = video.VideoParam.VideoBitrate,
                        VideoCodec = video.VideoParam.VideoCodec,
                        VideoHeight = video.VideoParam.VideoHeight,
                        VideoProfile = video.VideoParam.VideoProfile,
                        VideoWidth = video.VideoParam.VideoWidth,
                        VideoRotation = video.VideoParam.VideoRotation,
                    };
                });

            Mapper.CreateMap<ProcessedVideoEntity, DomainProcessedVideo>()
                .ConvertUsing(video =>
                {
                    if (video == null)
                    {
                        return null;
                    }

                    return new DomainProcessedVideo((TaskState)video.ProcessingState)
                    {
                        AttemptsCount = video.AttemptsCount,
                        AudioParam = new AudioParam
                        {
                            AudioBitrate = video.AudioBitrate,
                            AudioCodec = video.AudioCodec
                        },
                        Completed = video.Completed,
                        ContentType = video.ContentType,
                        Created = video.Created,
                        DestinationFileId = video.DestinationFileId,
                        SourceFileId = video.SourceFileId,
                        IsAudioCopy = video.IsAudioCopy,
                        IsVideoCopy = video.IsVideoCopy,
                        Modified = video.Modified,
                        ObjectId = video.Id,
                        OutputFormat = video.OutputFormat,
                        Progress = video.Progress,
                        Started = video.Started,
                        TaskId = video.TaskId,
                        UserId = video.UserId,
                        VideoParam = new VideoParam
                        {
                            FrameRate = video.FrameRate,
                            KeyFrameRate = video.KeyFrameRate,
                            MediaContainer = video.MediaContainer,
                            VideoBitrate = video.VideoBitrate,
                            VideoCodec = video.VideoCodec,
                            VideoHeight = video.VideoHeight,
                            VideoProfile = video.VideoProfile,
                            VideoWidth = video.VideoWidth,
                            VideoRotation = video.VideoRotation
                        }
                    };
                });
        }
    }
}