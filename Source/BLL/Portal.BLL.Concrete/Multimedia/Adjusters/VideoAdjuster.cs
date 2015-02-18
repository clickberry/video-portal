// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.BLL.Concrete.Multimedia.AdjustmentParameters;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.Domain.BackendContext.Entity;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.Adjusters
{
    public class VideoAdjuster : IVideoAdjuster
    {
        private readonly IAdjustmentVideoMetadata _adjustmentVideoMetadata;

        public VideoAdjuster(IAdjustmentVideoMetadata adjustmentVideoMetadata)
        {
            _adjustmentVideoMetadata = adjustmentVideoMetadata;
        }

        public VideoParam AdjustVideoParam(VideoAdjusterParam videoAdjusterParam, string mediaContainer, IVideoSize videoSize)
        {
            var exceptionList = new List<VideoFormatException>();
            int size = videoSize.Square();

            string container = AdjustMediaContainer(videoAdjusterParam.MediaContainer, mediaContainer, exceptionList);
            string videoCodec = AdjustVideoCodec(mediaContainer, videoAdjusterParam.VideoCodec, exceptionList);
            int videoBitrate = AdjustVideoBitrate(videoAdjusterParam.VideoBitrate, size, exceptionList);
            int videoWidth = AdjustVideoWidth(videoSize.Width, exceptionList);
            int videoHeight = AdjustVideoHeight(videoSize.Height, exceptionList);
            double frameRate = AdjustFrameRate(videoAdjusterParam.FrameRate, videoAdjusterParam.FrameRateMode, exceptionList);

            IVideoSize videoRotateSize = _adjustmentVideoMetadata.AdjustVideoRotateSize(videoWidth, videoHeight, videoAdjusterParam.VideoRotation);
            string videoProfile = _adjustmentVideoMetadata.AdjustVideoProfile(mediaContainer, videoAdjusterParam.VideoProfile);
            int keyFrameRate = _adjustmentVideoMetadata.AdjustKeyFrameRate(videoAdjusterParam.KeyFrameRate);

            CheckForException(exceptionList);

            VideoParam adjustVideoParam = CreateParam(container,
                videoCodec,
                videoProfile,
                videoBitrate,
                videoRotateSize.Width,
                videoRotateSize.Height,
                frameRate,
                keyFrameRate,
                videoAdjusterParam.VideoRotation);

            return adjustVideoParam;
        }

        private VideoParam CreateParam(string container, string videoCodec, string videoProfile, int videoBitrate, int videoWidth, int videoHeight, double frameRate, int keyFrameRate,
            double videoRotation)
        {
            return new VideoParam
            {
                MediaContainer = container,
                VideoCodec = videoCodec,
                VideoProfile = videoProfile,
                VideoBitrate = videoBitrate,
                VideoWidth = videoWidth,
                VideoHeight = videoHeight,
                FrameRate = frameRate,
                KeyFrameRate = keyFrameRate,
                VideoRotation = videoRotation
            };
        }

        private void CheckForException(ICollection<VideoFormatException> exceptionList)
        {
            if (exceptionList.Count > 0)
            {
                throw new AggregateException(exceptionList);
            }
        }

        private double AdjustFrameRate(double frameRate, string frameRateMode, ICollection<VideoFormatException> exceptionList)
        {
            double adjustFrameRate = 0;
            try
            {
                adjustFrameRate = _adjustmentVideoMetadata.AdjustFrameRate(frameRate, frameRateMode);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustFrameRate;
        }

        private int AdjustVideoHeight(int videoHeight, ICollection<VideoFormatException> exceptionList)
        {
            int adjustVideoHeight = 0;
            try
            {
                adjustVideoHeight = _adjustmentVideoMetadata.AdjustVideoHeight(videoHeight);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustVideoHeight;
        }

        private int AdjustVideoWidth(int videoWidth, ICollection<VideoFormatException> exceptionList)
        {
            int adjustVideoWidth = 0;
            try
            {
                adjustVideoWidth = _adjustmentVideoMetadata.AdjustVideoWidth(videoWidth);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustVideoWidth;
        }

        private int AdjustVideoBitrate(int videoBitrate, int videoSize, ICollection<VideoFormatException> exceptionList)
        {
            int adjustVideoBitrate = 0;
            try
            {
                adjustVideoBitrate = _adjustmentVideoMetadata.AdjustVideoBitrate(videoSize, videoBitrate);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustVideoBitrate;
        }

        private string AdjustVideoCodec(string mediaContainer, string videoCodec, ICollection<VideoFormatException> exceptionList)
        {
            string adjustVideoCodec = null;
            try
            {
                adjustVideoCodec = _adjustmentVideoMetadata.AdjustVideoCodec(mediaContainer, videoCodec);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustVideoCodec;
        }

        private string AdjustMediaContainer(string currentCntainer, string newContainer, ICollection<VideoFormatException> exceptionList)
        {
            string adjustMediaContainer = null;
            try
            {
                adjustMediaContainer = _adjustmentVideoMetadata.AdjustMediaContainer(currentCntainer, newContainer);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustMediaContainer;
        }
    }
}