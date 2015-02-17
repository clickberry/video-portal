// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Filter.Filters.Registrations
{
    public class OtherRegistrationsFilter : StatFilterBase<IStatUserRegistrationFilter>, IStatUserRegistrationFilter
    {
        public void Call(DomainStatUserRegistration domainStatUserRegistration, DomainReport domainReport)
        {
            if (domainStatUserRegistration.ProductName != ProductName.CicIPad &&
                domainStatUserRegistration.ProductName != ProductName.CicMac &&
                domainStatUserRegistration.ProductName != ProductName.CicPc &&
                domainStatUserRegistration.ProductName != ProductName.ImageShack &&
                domainStatUserRegistration.ProductName != ProductName.Mozilla &&
                domainStatUserRegistration.ProductName != ProductName.Player &&
                domainStatUserRegistration.ProductName != ProductName.Standalone &&
                domainStatUserRegistration.ProductName != ProductName.TaggerAndroid &&
                domainStatUserRegistration.ProductName != ProductName.TaggerIPhone &&
                domainStatUserRegistration.ProductName != ProductName.DailyMotion &&
                domainStatUserRegistration.ProductName != ProductName.JwPlayer)
            {
                domainReport.OtherRegistrations++;
            }
            if (Filter != null)
            {
                Filter.Call(domainStatUserRegistration, domainReport);
            }
        }
    }
}