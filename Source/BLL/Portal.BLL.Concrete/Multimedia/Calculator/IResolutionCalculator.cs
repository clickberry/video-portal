// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.BLL.Concrete.Multimedia.Calculator
{
    public interface IResolutionCalculator
    {
        List<IVideoSize> Calculate(int videoWidth, int videoHeight);
    }
}