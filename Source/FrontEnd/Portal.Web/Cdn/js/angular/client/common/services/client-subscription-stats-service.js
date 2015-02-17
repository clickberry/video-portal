// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.clients.subscriptions.stats',
    [])
    .factory('clientSubscriptionStatsService', [
        '$http', '$q', function ($http, $q) {

            var service = {};

            service.query = function (id, filter) {


                // build query params
                var params = {};

                // filter
                if (filter.redirectUrl) {
                    params['$filter'] = "RedirectUrl eq '" + filter.redirectUrl + "'";
                }

                // filter
                if (filter.dateFrom && filter.dateTo) {
                    if (params['$filter']) {
                        params['$filter'] += " and ";
                    } else {
                        params['$filter'] = "";
                    }
                    params['$filter'] += "DateFrom eq datetime'" + filter.dateFrom + "' and DateTo eq datetime'" + filter.dateTo + "'";
                }

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

                $http({
                        url: '/api/clients/subscriptions/' + id + '/stats',
                        method: 'GET',
                        params: params
                    })
                    .success(function(data) {
                        deferred.resolve(data);
                    })
                    .error(function() {
                        deferred.reject("Could not get subscription statistics");
                    });

                return deferred.promise;

            };
            return service;
        }
    ]);