// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.Admin
{
    [Obsolete("Remove after migration to admin API")]
    public class EqualsFilterRules<T> where T : IEquatable<T>
    {
        public string MemberName { get; set; }
        public T MemberValue { get; set; }
    }
}