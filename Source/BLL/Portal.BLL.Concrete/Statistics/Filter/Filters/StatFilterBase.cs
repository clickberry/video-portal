// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Filter;

namespace Portal.BLL.Concrete.Statistics.Filter.Filters
{
    public class StatFilterBase<T> : IStatFilter<T>
    {
        protected T Filter { get; private set; }

        public void Set(T filter)
        {
            Filter = filter;
        }
    }
}