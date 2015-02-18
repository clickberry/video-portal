// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.clients.subscriptions.stats.url',
    [])
    .factory('clientSubscriptionUrlStatsService', [
        '$http', '$q', '$window', function($http, $q, $window) {

            var service = {};

            service.query = function(id, url, filter) {


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

                if (filter.dateFrom && filter.dateTo) {
                    if (params['$filter']) {
                        params['$filter'] += " and ";
                    } else {
                        params['$filter'] = "";
                    }
                    params['$filter'] += "Date ge datetime'" + filter.dateFrom + "' and Date le datetime'" + filter.dateTo + "'";
                }


                // making HTTP request

                url = $window.encodeURIComponent(url);

                var deferred = $q.defer();

                $http({
                        url: '/api/clients/subscriptions/' + id + '/stats/url?url=' + url,
                        method: 'GET',
                        params: params
                    }).
                    success(function(data) {
                        deferred.resolve(data);
                    }).
                    error(function() {
                        deferred.reject("Could not get subscription statistics for url");
                    });

                return deferred.promise;

            };
            return service;
        }
    ]);