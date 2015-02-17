// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Filter
{
    public interface IStatProjectUploadingFilter : IStatFilter<IStatProjectUploadingFilter>
    {
        void Call(DomainStatProjectState domainStatProjectState, DomainReport domainReport);
    }
}