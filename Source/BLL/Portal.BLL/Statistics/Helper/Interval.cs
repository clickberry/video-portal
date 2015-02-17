// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.BLL.Statistics.Helper
{
    public class Interval
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public int Days
        {
            get { return (Finish - Start).Days; }
        }
    }
}