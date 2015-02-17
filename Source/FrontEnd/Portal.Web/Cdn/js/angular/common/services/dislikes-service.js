// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.dislikes',
    [
        'ngResource'
    ])
    .factory('dislikesService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/dislikes/:id', { id: '@id' });

            var service = {};

            service.add = function(id) {

                // making HTTP request

                var deferred = $q.defer();
                resource.save({id: id}, function () {

                    deferred.resolve();

                });

                return deferred.promise;

            };

            service.delete = function (id) {

                // making HTTP request

                var deferred = $q.defer();
                resource.delete({ id: id }, function () {

                    deferred.resolve();

                });

                return deferred.promise;

            };

            service.queryDislikes = function (filter) {

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
                resource.query(params, function (data) {

                    deferred.resolve(data);

                }, function () {

                    deferred.reject("Could not query user dislikes");

                });

                return deferred.promise;
            };

            service.queryDislikers = function (projectId, filter) {

                // build query params
                var params = { id: projectId };

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

                    deferred.reject("Could not query project dislikers");

                });

                return deferred.promise;
            };


            return service;
        }
    ]);