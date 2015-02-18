// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IVideoEncodeStringFactory : IEncodeStringFactory
    {
        string GetVideoCodecLib(string videoCodec);
        string GetVideoCodecOption(string videoCodec, string videoProfile);
        string GetAudioCodecLib(string audioCodec);
        string GetContainerString(string container);
        string GetAudioString(string audioCodecLib, int audioBitrate, bool isAudioCopy);
        string GetVideoString(string videoCodecLib, string videoCodecOption, string videoFilter, int videoBitrate, double frameRate, int keyFrameRate, int width, int height, bool isVideoCopy);
    }
}