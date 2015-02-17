// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.StatisticContext
{
    public class DomainStatProjectState
    {
        public string ProjectId { get; set; }

        public bool IsSuccessfulUpload { get; set; }

        public DateTime DateTime { get; set; }

        public string Producer { get; set; }

        public string Version { get; set; }
    }
}