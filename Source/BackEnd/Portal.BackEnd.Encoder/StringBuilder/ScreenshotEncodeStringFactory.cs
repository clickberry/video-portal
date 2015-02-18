// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Portal.BackEnd.Encoder.Interface;

namespace Portal.BackEnd.Encoder.StringBuilder
{
    public class ScreenshotEncodeStringFactory : EncodeStringFactoryBase, IScreenshotEncodeStringFactory
    {
        public string GetImageOption(double timeOffset)
        {
            return String.Format("-ss {0}", timeOffset.ToString(CultureInfo.InvariantCulture));
        }
    }
}