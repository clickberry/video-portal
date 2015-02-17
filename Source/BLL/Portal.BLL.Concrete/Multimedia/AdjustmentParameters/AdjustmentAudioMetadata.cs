// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BLL.Concrete.Multimedia.Constraints;
using Portal.Domain.BackendContext.Constant;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.AdjustmentParameters
{
    public class AdjustmentAudioMetadata : IAdjustmentAudioMetadata
    {
        private readonly AudioConstraints _constraints;

        public AdjustmentAudioMetadata(AudioConstraints constraints)
        {
            _constraints = constraints;
        }

        public string AdjustAudioCodec(string mediaContainer, string audioCodec)
        {
            if (String.IsNullOrEmpty(audioCodec))
            {
                throw new VideoFormatException(ParamType.AudioCodec);
            }

            if (mediaContainer == MetadataConstant.Mp4Container)
            {
                return MetadataConstant.AacCodec;
            }
            if (mediaContainer == MetadataConstant.WebmContainer)
            {
                return MetadataConstant.VorbisCodec;
            }

            return null;
        }

        public int AdjustAudioBitrate(int size, int audioChannel, int audioBitrate, int audioSampleRate)
        {
            if (audioBitrate <= 0)
            {
                throw new VideoFormatException(ParamType.AudioBitrate);
            }
            if (audioBitrate < audioChannel*audioSampleRate)
            {
                throw new VideoFormatException(ParamType.AudioBitrate);
            }

            if (size >= _constraints.Size720P)
            {
                if (audioChannel == 1)
                {
                    if (audioBitrate > _constraints.MaxAudioBitrate720POneChannel)
                    {
                        return _constraints.MaxAudioBitrate720POneChannel;
                    }
                    return audioBitrate;
                }
                if (audioChannel == 2)
                {
                    if (audioBitrate > _constraints.MaxAudioBitrate720PTwoChannel)
                    {
                        return _constraints.MaxAudioBitrate720PTwoChannel;
                    }
                    return audioBitrate;
                }
                if (audioChannel == 6)
                {
                    if (audioBitrate > _constraints.MaxAudioBitrate720PSixChannel)
                    {
                        return _constraints.MaxAudioBitrate720PSixChannel;
                    }
                    return audioBitrate;
                }
            }

            if (audioChannel == 1)
            {
                if (audioBitrate > _constraints.DefaultAudioBitrateOneChannel)
                {
                    return _constraints.DefaultAudioBitrateOneChannel;
                }
                return audioBitrate;
            }
            if (audioChannel == 2)
            {
                if (audioBitrate > _constraints.DefaultAudioBitrateTwoChannel)
                {
                    return _constraints.DefaultAudioBitrateTwoChannel;
                }
                return audioBitrate;
            }
            if (audioChannel == 6)
            {
                if (audioBitrate > _constraints.DefaultAudioBitrateSixChannel)
                {
                    return _constraints.DefaultAudioBitrateSixChannel;
                }
                return audioBitrate;
            }

            throw new VideoFormatException(ParamType.AudioChannel);
        }
    }
}