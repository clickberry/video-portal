// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.clients.balance.details',
    ['ngResource'])
    .factory('clientBalanceDetailsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/clients/balance/details');

            var service = {};

            service.query = function (filter) {

                // build query params
                var params = {};

                // sort
                var sortExp = null;
                if (filter.orderBy != null) {
                    var orderByDirection = filter.orderByAsc ? 'asc' : 'desc';
                    sortExp = filter.orderBy + ' ' + orderByDirection;
                }

                if (sortExp != null) {
                    params['$orderby'] = sortExp;
                }

                // count
                if (filter.inlinecount != null) {
                    params['$inlinecount'] = filter.inlinecount;
                }

                // paging
                if (filter.skip != null) {
                    params['$skip'] = filter.skip;
                }

                if (filter.top != null) {
                    params['$top'] = filter.top;
                }


                // making HTTP request

                var deferred = $q.defer();

                resource.query(params, function (data) {

                    deferred.resolve(data);

                }, function () {

                    deferred.reject("Could not get balance details");

                });

                return deferred.promise;

            }

            return service;
        }
    ]);