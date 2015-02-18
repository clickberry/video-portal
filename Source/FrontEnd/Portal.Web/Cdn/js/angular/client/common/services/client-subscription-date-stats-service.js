// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.clients.subscriptions.stats.date',
    [])
    .factory('clientSubscriptionDateStatsService', [
        '$http', '$q', '$window', function ($http, $q, $window) {

            var service = {};

            service.query = function (id, url, filter) {


                // build query params
                var params = {};

                // filter
                if (filter.dateFrom) {
                    params['$filter'] = "Date ge datetime'" + filter.dateFrom + "'";
                }
                if (filter.dateTo) {
                    if (params['$filter']) {
                        params['$filter'] += " and ";
                    } else {
                        params['$filter'] = "";
                    }
                    params['$filter'] += "Date le datetime'" + filter.dateTo + "'";
                }


                // making HTTP request

                var resourceUrl = '/api/clients/subscriptions/' + id + '/stats/group/date';
                if (url) {
                    resourceUrl += '?url=' + $window.encodeURIComponent(url);
                }

                var deferred = $q.defer();

                $http({
                    url: resourceUrl,
                    method: 'GET',
                    params: params
                }).
                    success(function (data) {
                        deferred.resolve(data);
                    }).
                    error(function () {
                        deferred.reject("Could not get subscription daily statistics");
                    });

                return deferred.promise;

            };
            return service;
        }
    ]);