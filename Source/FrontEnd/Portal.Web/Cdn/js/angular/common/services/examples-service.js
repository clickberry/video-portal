// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.examples',
    [
        'ngResource'
    ])
    .factory('examplesService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/examples');

            var service = {};

            service.query = function(filter) {

                // build query params
                var params = {};

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

                    deferred.reject("Could not query examples");

                });

                return deferred.promise;
            };


            return service;
        }
    ]);