// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DAL.Entities.QueryObject
{
    public class StatQueryObject
    {
        public bool IsStartInclude { get; set; }

        public bool IsEndInclude { get; set; }

        public string StartInterval { get; set; }

        public string EndInterval { get; set; }
    }
}