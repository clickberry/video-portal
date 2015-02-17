// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BLL.Concrete.Multimedia.Calculator;
using Portal.BLL.Concrete.Multimedia.Constraints;
using Portal.Domain.BackendContext.Constant;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.AdjustmentParameters
{
    public class AdjustmentVideoMetadata : IAdjustmentVideoMetadata
    {
        private readonly VideoConstraints _constraints;

        public AdjustmentVideoMetadata(VideoConstraints constraints)
        {
            _constraints = constraints;
        }

        public int AdjustVideoWidth(int width)
        {
            if (width < _constraints.MinWidth)
            {
                throw new VideoFormatException(ParamType.Width);
            }
            if (width > _constraints.MaxWidth)
            {
                throw new VideoFormatException(ParamType.Width);
            }
            int modulo = width%_constraints.Mul;
            return modulo == 0 ? width : width - modulo;
        }

        public int AdjustVideoHeight(int height)
        {
            if (height < _constraints.MinHeight)
            {
                throw new VideoFormatException(ParamType.Height);
            }
            if (height > _constraints.MaxHeight)
            {
                throw new VideoFormatException(ParamType.Height);
            }
            int modulo = height%_constraints.Mul;
            return modulo == 0 ? height : height - modulo;
        }

        public int AdjustVideoBitrate(int videoSize, int videoBitrate)
        {
            if (videoBitrate <= 0)
            {
                throw new VideoFormatException(ParamType.VideoBitRate);
            }

            if (videoSize >= _constraints.Size1080P)
            {
                if (videoBitrate > _constraints.MaxBitrate1080P)
                {
                    return _constraints.MaxBitrate1080P;
                }
                return videoBitrate;
            }
            if (videoSize >= _constraints.Size720P)
            {
                if (videoBitrate > _constraints.MaxBitrate720P)
                {
                    return _constraints.MaxBitrate720P;
                }
                return videoBitrate;
            }
            if (videoSize >= _constraints.Size480P)
            {
                if (videoBitrate > _constraints.MaxBitrate480P)
                {
                    return _constraints.MaxBitrate480P;
                }
                return videoBitrate;
            }
            if (videoBitrate > _constraints.DefaultBitrate)
            {
                return _constraints.DefaultBitrate;
            }

            return videoBitrate;
        }

        public double AdjustFrameRate(double frameRate, string frameRateMode)
        {
            if (frameRate <= 0 && frameRateMode == MetadataConstant.VariableFrameRate)
            {
                return _constraints.FrameRate;
            }

            if (frameRate > _constraints.MaxFrameRate)
            {
                return _constraints.MaxFrameRate;
            }
            if (frameRate < _constraints.MinFrameRate)
            {
                return _constraints.MinFrameRate;
            }

            return frameRate;
        }

        public string AdjustVideoProfile(string container, string profile)
        {
            if (container == MetadataConstant.Mp4Container)
            {
                if (profile == MetadataConstant.AvcBaselineProfile)
                {
                    return MetadataConstant.AvcBaselineProfile;
                }
                return MetadataConstant.AvcMainProfile;
            }
            return null;
        }

        public int AdjustKeyFrameRate(int keyFrameRate)
        {
            if (keyFrameRate < _constraints.MinKeyFrameRate || keyFrameRate > _constraints.MaxKeyFrameRate)
            {
                return _constraints.DefaultKeyFrameRate;
            }
            return keyFrameRate;
        }

        public string AdjustVideoCodec(string newMediaContainer, string codec)
        {
            if (String.IsNullOrEmpty(codec))
            {
                throw new VideoFormatException(ParamType.VideoCodec);
            }

            if (newMediaContainer == MetadataConstant.Mp4Container)
            {
                return MetadataConstant.AvcCodec;
            }
            if (newMediaContainer == MetadataConstant.WebmContainer)
            {
                return MetadataConstant.Vp8Codec;
            }

            return null;
        }

        public string AdjustMediaContainer(string currentContainer, string newContainer)
        {
            if (String.IsNullOrEmpty(currentContainer))
            {
                throw new VideoFormatException(ParamType.MediaContainer);
            }

            return newContainer;
        }

        public IVideoSize AdjustVideoRotateSize(int width, int height, double videoRotation)
        {
            switch ((int)videoRotation)
            {
                case 0:
                    return new VideoSize(width, height);

                case 90:
                    return new VideoSize(height, width);

                case 180:
                    return new VideoSize(width, height);

                case 270:
                    return new VideoSize(height, width);
            }

            return null;
        }
    }
}