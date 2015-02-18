// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.BLL.Statistics.Filter
{
    public interface IStatFilter<in T>
    {
        void Set(T filter);
    }
}