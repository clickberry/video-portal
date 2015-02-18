// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.BLL.Concrete.Multimedia.AdjustmentParameters
{
    public interface IAdjustmentScreenshotMetadata
    {
        int AdjustScreenshotWidth(int width);
        int AdjustScreenshotHeight(int height);
        double AdjustScreenshotTimeOffset(double duration);
    }
}