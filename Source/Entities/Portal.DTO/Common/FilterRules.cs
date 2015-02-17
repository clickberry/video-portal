// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.Domain.Admin;

namespace Portal.DTO.Common
{
    [Obsolete("Remove after migration to admin API")]
    public class FilterRules
    {
        public virtual string Orderby { get; set; }
        public virtual OrderByDirections OrderDirection { get; set; }
        public virtual int? Skip { get; set; }
        public virtual int Take { get; set; }

        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual string Type { get; set; }
    }
}