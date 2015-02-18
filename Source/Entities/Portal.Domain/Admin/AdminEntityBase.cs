﻿// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.Domain.Admin
{
    public class AdminEntityBase<T>
    {
        public List<T> EntityList { get; set; }

        /// <summary>
        ///     How much entities in persistence. May differ from EntityList.Count
        /// </summary>
        public int TotalEntities { get; set; }
    }
}