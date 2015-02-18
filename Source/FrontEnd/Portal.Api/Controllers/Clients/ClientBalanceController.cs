// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Portal.BLL.Subscriptions;
using Portal.Domain.PortalContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;

namespace Portal.Api.Controllers.Clients
{
    /// <summary>
    ///     Gets accumulative client company balance.
    /// </summary>
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [Route("clients/balance")]
    public class ClientBalanceController : ApiControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ICompanyService _companyService;

        public ClientBalanceController(
            ICompanyService companyService,
            IBalanceService balanceService)
        {
            _companyService = companyService;
            _balanceService = balanceService;
        }

        //
        // GET: /api/clients/balance

        /// <summary>
        ///     Gets accumulative client company balance.
        /// </summary>
        /// <returns></returns>
        public async Task<Balance> Get()
        {
            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            decimal balance = await _balanceService.GetBalanceAsync(company.Id);

            return new Balance { Amount = balance, Date = DateTime.UtcNow };
        }
    }
}