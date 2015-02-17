// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Portal.DTO.Reports
{
    [DataContract(Namespace = "")]
    public class StandardReport
    {
        [DataMember(Order = 1)]
        public DateTime DateTime { get; set; }

        [DataMember(Order = 10)]
        public Report LastDay { get; set; }

        [DataMember(Order = 20)]
        public Report LastWeek { get; set; }

        [DataMember(Order = 30)]
        public Report LastMonth { get; set; }

        [DataMember(Order = 40)]
        public Report AllDays { get; set; }
    }
}