// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.DTO.Subscriptions
{
    public class Balance
    {
        public virtual decimal Amount { get; set; }

        public virtual DateTime Date { get; set; }
    }
}