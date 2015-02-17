// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.trends',
    [
        'ngResource'
    ])
    .factory('trendsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/watch/trends/week');

            var service = {};

            service.query = function(filter) {

                // build query params
                var params = {};

                // filter
                var filterExp = null;
                var filterParts = [];

                if (filter.version) {
                    filterParts.push("Version eq " + filter.version);
                }

                if (filter.productType) {
                    filterParts.push("Generator eq '" + filter.productType + "'");
                }

                if (filterParts.length > 0) {
                    filterExp = filterParts.join(" and ");
                }

                if (filterExp != null) {
                    params['$filter'] = filterExp;
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
                resource.query(params, function(data) {

                    deferred.resolve(data);

                }, function() {

                    deferred.reject("Could not query trends");

                });

                return deferred.promise;
            };


            return service;
        }
    ]);