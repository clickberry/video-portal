// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Concrete.Multimedia.Constraints;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.AdjustmentParameters
{
    public class AdjustmentScreenshotMetadata : IAdjustmentScreenshotMetadata
    {
        private readonly ScreenshotConstraints _constraints;

        public AdjustmentScreenshotMetadata(ScreenshotConstraints constraints)
        {
            _constraints = constraints;
        }

        public int AdjustScreenshotWidth(int width)
        {
            if (width < _constraints.MinWidth)
            {
                throw new VideoFormatException(ParamType.Width);
            }
            if (width > _constraints.MaxWidth)
            {
                throw new VideoFormatException(ParamType.Width);
            }

            return width;
        }

        public int AdjustScreenshotHeight(int height)
        {
            if (height < _constraints.MinHeight)
            {
                throw new VideoFormatException(ParamType.Height);
            }
            if (height > _constraints.MaxHeight)
            {
                throw new VideoFormatException(ParamType.Height);
            }

            return height;
        }

        public double AdjustScreenshotTimeOffset(double duration)
        {
            return (duration*_constraints.RelatedTimeOffset);
        }
    }
}