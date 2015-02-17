// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.BLL.Concrete.Multimedia.Calculator
{
    public class VideoSize : IVideoSize
    {
        public VideoSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Square()
        {
            return Width*Height;
        }
    }
}