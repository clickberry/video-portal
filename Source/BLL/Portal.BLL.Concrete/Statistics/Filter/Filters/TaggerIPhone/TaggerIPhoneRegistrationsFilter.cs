// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BLL.Statistics.Filter;
using Portal.Domain.StatisticContext;

namespace Portal.BLL.Concrete.Statistics.Filter.Filters.TaggerIPhone
{
    public class TaggerIPhoneRegistrationsFilter : StatFilterBase<IStatUserRegistrationFilter>, IStatUserRegistrationFilter
    {
        public void Call(DomainStatUserRegistration domainStatUserRegistration, DomainReport domainReport)
        {
            if (domainStatUserRegistration.ProductName == ProductName.TaggerIPhone)
            {
                domainReport.TaggerIPhoneRegistrations++;
            }
            if (Filter != null)
            {
                Filter.Call(domainStatUserRegistration, domainReport);
            }
        }
    }
}