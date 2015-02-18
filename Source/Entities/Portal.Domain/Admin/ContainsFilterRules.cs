// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.Admin
{
    [Obsolete("Remove after migration to admin API")]
    public class ContainsFilterRules
    {
        public string MemberValue { get; set; }
        public string MemberName { get; set; }
    }
}