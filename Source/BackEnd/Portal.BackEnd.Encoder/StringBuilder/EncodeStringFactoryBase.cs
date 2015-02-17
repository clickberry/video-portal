// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.BackEnd.Encoder.Interface;

namespace Portal.BackEnd.Encoder.StringBuilder
{
    public abstract class EncodeStringFactoryBase : IEncodeStringFactory
    {
        public string GetVideoFilter(int videoRotation)
        {
            switch (videoRotation)
            {
                case 0:
                    return String.Empty;

                case 90:
                    return "-vf transpose=1 ";

                case 270:
                    return "-vf transpose=2 ";

                case 180:
                    return "-vf vflip,hflip ";
            }
            return null;
        }
    }
}