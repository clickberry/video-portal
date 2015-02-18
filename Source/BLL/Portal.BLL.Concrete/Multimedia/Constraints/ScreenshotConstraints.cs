// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.BLL.Concrete.Multimedia.Constraints
{
    public class ScreenshotConstraints
    {
        public ScreenshotConstraints()
        {
            MaxWidth = Int16.MaxValue - 1;
            MaxHeight = Int16.MaxValue - 1;
            MinWidth = 2;
            MinHeight = 2;

            RelatedTimeOffset = 0.3;
        }

        public int MaxWidth { get; private set; }

        public int MinWidth { get; private set; }

        public int MaxHeight { get; private set; }

        public int MinHeight { get; private set; }

        public double RelatedTimeOffset { get; private set; }
    }
}