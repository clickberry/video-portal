// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.Admin;

namespace Portal.Domain
{
    public class DataQueryOptions
    {
        public DataQueryOptions()
        {
            Filters = new List<DataFilterRule>();
        }

        public List<DataFilterRule> Filters { get; set; }

        public string OrderBy { get; set; }

        public OrderByDirections OrderByDirection { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public bool Count { get; set; }
    }
}