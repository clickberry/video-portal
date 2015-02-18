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
    public class AudioAdjuster : IAudioAdjuster
    {
        private readonly IAdjustmentAudioMetadata _adjustmentAudioMetadata;

        public AudioAdjuster(IAdjustmentAudioMetadata adjustmentAudioMetadata)
        {
            _adjustmentAudioMetadata = adjustmentAudioMetadata;
        }

        public AudioParam AdjustAudioParam(AudioAdjusterParam audioAdjusterParam, string mediaContainer, IVideoSize videoSize)
        {
            var exceptionList = new List<VideoFormatException>();
            int size = videoSize.Square();
            string audioCodec = AdjustAudioCodec(mediaContainer, audioAdjusterParam.AudioCodec, audioAdjusterParam.IsExistAudioStream, exceptionList);
            int audioBitrate = AdjustAudioBitrate(size, audioAdjusterParam.AudioChannels, audioAdjusterParam.AudioBitrate, audioAdjusterParam.AudioSampleRate, audioAdjusterParam.IsExistAudioStream,
                exceptionList);
            CheckForException(exceptionList);

            AudioParam adjustaudioParam = CreateParam(audioCodec, audioBitrate);

            return adjustaudioParam;
        }

        private AudioParam CreateParam(string audioCodec, int audioBitrate)
        {
            return new AudioParam
            {
                AudioCodec = audioCodec,
                AudioBitrate = audioBitrate
            };
        }

        private void CheckForException(ICollection<VideoFormatException> exceptionList)
        {
            if (exceptionList.Count > 0)
            {
                throw new AggregateException(exceptionList);
            }
        }

        private string AdjustAudioCodec(string mediaContainer, string audioCodec, bool isExistAudioStream, ICollection<VideoFormatException> exceptionList)
        {
            string adjustVideoCodec = null;
            if (!isExistAudioStream)
            {
                return null;
            }

            try
            {
                adjustVideoCodec = _adjustmentAudioMetadata.AdjustAudioCodec(mediaContainer, audioCodec);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustVideoCodec;
        }

        private int AdjustAudioBitrate(int size, int audioChannel, int audioBitrate, int audioSamplerate, bool isExistAudioStream, ICollection<VideoFormatException> exceptionList)
        {
            int adjustVideoCodec = 0;
            if (!isExistAudioStream)
            {
                return 0;
            }

            try
            {
                adjustVideoCodec = _adjustmentAudioMetadata.AdjustAudioBitrate(size, audioChannel, audioBitrate, audioSamplerate);
            }
            catch (VideoFormatException ex)
            {
                exceptionList.Add(ex);
            }
            return adjustVideoCodec;
        }
    }
}