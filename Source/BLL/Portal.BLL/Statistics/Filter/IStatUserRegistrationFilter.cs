// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Domain.StatisticContext;

namespace Portal.BLL.Statistics.Filter
{
    public interface IStatUserRegistrationFilter : IStatFilter<IStatUserRegistrationFilter>
    {
        void Call(DomainStatUserRegistration domainStatUserRegistration, DomainReport domainReport);
    }
}