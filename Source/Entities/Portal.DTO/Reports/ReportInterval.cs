// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Portal.DTO.Reports
{
    [DataContract(Namespace = "")]
    public class ReportInterval
    {
        [DataMember(Order = 10)]
        public DateTime Start { get; set; }

        [DataMember(Order = 20)]
        public DateTime End { get; set; }
    }
}