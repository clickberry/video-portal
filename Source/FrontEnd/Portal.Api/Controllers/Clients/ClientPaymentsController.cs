// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Filters;
using Portal.Api.Models;
using Portal.BLL.Subscriptions;
using Portal.Domain.PortalContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;
using Portal.Mappers;

namespace Portal.Api.Controllers.Clients
{
    /// <summary>
    ///     Manages client company payments.
    /// </summary>
    [ValidationHttp]
    [AuthorizeHttp(Roles = DomainRoles.Client)]
    [Route("clients/payments")]
    public class ClientPaymentsController : ApiControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public ClientPaymentsController(
            ICompanyService companyService,
            IMapper mapper,
            IBalanceService balanceService)
        {
            _companyService = companyService;
            _mapper = mapper;
            _balanceService = balanceService;
        }

        //
        // GET: /api/clients/payments

        /// <summary>
        ///     Gets payment history.
        /// </summary>
        /// <returns></returns>
        [CustomQueryable]
        [ODataValidationExceptionFilter]
        public async Task<IEnumerable<BalanceHistory>> Get()
        {
            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            IEnumerable<DomainBalanceHistory> balanceHistory = await _balanceService.QueryPaymentsHistoryAsync(company.Id);

            return _mapper.Map<IEnumerable<DomainBalanceHistory>, IEnumerable<BalanceHistory>>(balanceHistory);
        }


        //
        // POST: /api/clients/payments

        /// <summary>
        ///     Makes a payment for client company by charging card.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CheckAccessHttp]
        public async Task<HttpResponseMessage> Post(ClientPaymentModel model)
        {
            // Get company for user
            DomainCompany company = await _companyService.FindByUserAsync(UserId);

            var chargeCreateOptions = new CompanyChargeOptions
            {
                Id = company.Id,
                AmountInCents = model.AmountInCents,
                Currency = DomainCurrencies.Default,
                Description = model.Description,
                TokenId = model.TokenId
            };

            // Charging

            // balance will be updated after callback (webhook) from billing system
            await _companyService.ChargeAsync(chargeCreateOptions);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}