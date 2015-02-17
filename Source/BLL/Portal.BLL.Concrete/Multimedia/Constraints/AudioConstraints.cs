// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.BLL.Concrete.Multimedia.Constraints
{
    public class AudioConstraints
    {
        public AudioConstraints()
        {
            Size720P = 720*1280;

            MaxAudioBitrate720POneChannel = 128000;
            MaxAudioBitrate720PTwoChannel = 384000;
            MaxAudioBitrate720PSixChannel = 512000;

            DefaultAudioBitrateOneChannel = 64000;
            DefaultAudioBitrateTwoChannel = 128000;
            DefaultAudioBitrateSixChannel = 196000;
        }

        public int Size720P { get; private set; }

        public int MaxAudioBitrate720POneChannel { get; private set; }

        public int MaxAudioBitrate720PTwoChannel { get; private set; }

        public int MaxAudioBitrate720PSixChannel { get; private set; }

        public int DefaultAudioBitrateOneChannel { get; private set; }

        public int DefaultAudioBitrateTwoChannel { get; private set; }

        public int DefaultAudioBitrateSixChannel { get; private set; }
    }
}