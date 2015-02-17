// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using Portal.BLL.Subscriptions;
using Portal.Domain.PortalContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;
using Portal.Mappers;

namespace Portal.Api.Controllers.Clients
{
    /// <summary>
    ///     Gets detailed client company balance.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [Route("clients/balance/details")]
    public class ClientBalanceDetailsController : ApiControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public ClientBalanceDetailsController(
            ICompanyService companyService,
            IBalanceService balanceService,
            IMapper mapper)
        {
            _companyService = companyService;
            _balanceService = balanceService;
            _mapper = mapper;
        }

        //
        // GET: /api/clients/balance/details

        /// <summary>
        ///     Gets detailed client company balance.
        /// </summary>
        /// <returns></returns>
        [CustomQueryable]
        [ODataValidationExceptionFilter]
        public async Task<IEnumerable<BalanceHistory>> Get()
        {
            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            IEnumerable<DomainBalanceHistory> balanceHistory = await _balanceService.QueryHistoryAsync(company.Id);

            return _mapper.Map<IEnumerable<DomainBalanceHistory>, IEnumerable<BalanceHistory>>(balanceHistory);
        }
    }
}