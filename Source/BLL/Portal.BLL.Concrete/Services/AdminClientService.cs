// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Portal.BLL.Services;
using Portal.BLL.Subscriptions;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.DAL.User;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Services
{
    public sealed class AdminClientService : IAdminClientService
    {
        private readonly IBalanceService _balanceService;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;

        public AdminClientService(ICompanyRepository companyRepository, IUserRepository userRepository, IBalanceService balanceService)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _balanceService = balanceService;
        }

        public DataResult<Task<DomainClientForAdmin>> GetAsyncSequence(DataQueryOptions filter)
        {
            var query = new List<IMongoQuery>(filter.Filters.Count);

            // filtering
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainClientForAdmin>(x => x.Name),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by name
                    query.Add(Query.Text(f.Value.ToString().ToLowerInvariant()));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainUserForAdmin>(x => x.Email),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by email
                    string email = f.Value.ToString().ToLowerInvariant();
                    var expression = new BsonRegularExpression(string.Format("^{0}.*", Regex.Escape(email)));
                    query.Add(Query<CompanyEntity>.Matches(p => p.Email, expression));
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainClientForAdmin>(x => x.Created),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // by created date
                    var date = (DateTime)f.Value;
                    switch (f.Type)
                    {
                        case DataFilterTypes.Equal:
                            query.Add(Query<CompanyEntity>.EQ(p => p.Created, date));
                            break;
                        case DataFilterTypes.LessThan:
                            query.Add(Query<CompanyEntity>.LT(p => p.Created, date));
                            break;
                        case DataFilterTypes.LessThanOrEqual:
                            query.Add(Query<CompanyEntity>.LTE(p => p.Created, date));
                            break;
                        case DataFilterTypes.GreaterThan:
                            query.Add(Query<CompanyEntity>.GT(p => p.Created, date));
                            break;
                        case DataFilterTypes.GreaterThanOrEqual:
                            query.Add(Query<CompanyEntity>.GTE(p => p.Created, date));
                            break;
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            MongoCursor<CompanyEntity> cursor = _companyRepository.Collection.Find(query.Count > 0 ? Query.And(query) : Query.Null);
            IMongoSortBy sortOrder = null;

            // sorting
            if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainClientForAdmin>(x => x.Name),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by name
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<CompanyEntity>.Ascending(p => p.NameSort)
                    : SortBy<CompanyEntity>.Descending(p => p.NameSort);
            }
            else if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainClientForAdmin>(x => x.Created),
                StringComparison.OrdinalIgnoreCase) == 0)
            {
                // order by created
                sortOrder = filter.OrderByDirection == OrderByDirections.Asc
                    ? SortBy<CompanyEntity>.Ascending(p => p.Created)
                    : SortBy<CompanyEntity>.Descending(p => p.Created);
            }

            if (sortOrder != null)
            {
                cursor.SetSortOrder(sortOrder);
            }


            // paging
            if (filter.Skip.HasValue)
            {
                cursor.SetSkip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                cursor.SetLimit(filter.Take.Value);
            }

            // Count of results
            long? count = null;
            if (filter.Count)
            {
                count = cursor.Count();
            }

            // post-processing
            return new DataResult<Task<DomainClientForAdmin>>(cursor.Select(GetClientDataAsync), count);
        }

        public async Task<DomainClientForAdmin> GetAsync(string clientId)
        {
            CompanyEntity company = await _companyRepository.GetAsync(clientId);
            if (company == null)
            {
                throw new NotFoundException();
            }

            return await GetClientDataAsync(company);
        }

        public async Task<DomainClientForAdmin> SetStateAsync(string id, ResourceState state)
        {
            // Update company
            CompanyEntity company = await _companyRepository.GetAsync(id);
            if (company == null)
            {
                throw new NotFoundException();
            }

            company.State = (int)state;
            await _companyRepository.UpdateAsync(company);

            // Update users
            await Task.WhenAll(company.Users.Select(async p =>
            {
                UserEntity user = await _userRepository.GetAsync(p);
                user.State = (int)state;
                await _userRepository.UpdateAsync(user);
            }));

            return await GetClientDataAsync(company);
        }

        private async Task<DomainClientForAdmin> GetClientDataAsync(CompanyEntity company)
        {
            return new DomainClientForAdmin
            {
                Id = company.Id,
                Name = company.Name,
                Email = company.Email,
                Created = company.Created,
                Balance = await _balanceService.GetBalanceAsync(company.Id),
                State = (ResourceState)company.State
            };
        }
    }
}