// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.BLL.Concrete.Multimedia.Constraints
{
    public class VideoConstraints
    {
        public VideoConstraints()
        {
            Mul = 2;
            MaxWidth = Int16.MaxValue - 1;
            MaxHeight = Int16.MaxValue - 1;
            MinWidth = Mul;
            MinHeight = Mul;

            MaxBitrate1080P = 8000000;
            MaxBitrate720P = 5000000;
            MaxBitrate480P = 2500000;
            DefaultBitrate = 1000000;

            Size1080P = 1920*1080;
            Size720P = 1280*720;
            Size480P = 854*480;

            FrameRate = 25;
            MaxFrameRate = 60;
            MinFrameRate = 10;

            MaxKeyFrameRate = 60;
            MinKeyFrameRate = 1;
            DefaultKeyFrameRate = 10;
        }

        public int Mul { get; private set; }

        public int MaxWidth { get; private set; }

        public int MinWidth { get; private set; }

        public int MaxHeight { get; private set; }

        public int MinHeight { get; private set; }

        public int MaxBitrate1080P { get; private set; }

        public int MaxBitrate720P { get; private set; }

        public int MaxBitrate480P { get; private set; }

        public int DefaultBitrate { get; private set; }

        public int Size1080P { get; private set; }

        public int Size720P { get; private set; }

        public int Size480P { get; private set; }

        public double MaxFrameRate { get; private set; }

        public double MinFrameRate { get; private set; }

        public int MinKeyFrameRate { get; private set; }

        public int MaxKeyFrameRate { get; private set; }

        public int DefaultKeyFrameRate { get; private set; }

        public double FrameRate { get; set; }
    }
}