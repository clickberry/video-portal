// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.Admin
{
    public class DataFilterRule
    {
        public string Name { get; set; }

        public DataFilterTypes Type { get; set; }

        public object Value { get; set; }
    }
}