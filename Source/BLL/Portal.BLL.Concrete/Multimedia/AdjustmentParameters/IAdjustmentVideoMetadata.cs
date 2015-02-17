// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.Calculator;

namespace Portal.BLL.Concrete.Multimedia.AdjustmentParameters
{
    public interface IAdjustmentVideoMetadata
    {
        int AdjustVideoWidth(int width);
        int AdjustVideoHeight(int height);
        int AdjustVideoBitrate(int videoSize, int videoBitrate);
        double AdjustFrameRate(double frameRate, string frameRateMode);
        string AdjustVideoProfile(string container, string profile);
        int AdjustKeyFrameRate(int keyFrameRate);
        string AdjustVideoCodec(string newMediaContainer, string codec);
        string AdjustMediaContainer(string currentContainer, string newContainer);
        IVideoSize AdjustVideoRotateSize(int width, int height, double videoRotation);
    }
}