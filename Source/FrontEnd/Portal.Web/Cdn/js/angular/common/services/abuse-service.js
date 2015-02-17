// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.abuse',
    [
        'ngResource'
    ])
    .factory('abuseService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/abuse/:id', { id: '@id' });

            var service = {};

            service.add = function(id) {

                // making HTTP request

                var deferred = $q.defer();
                resource.save({id: id}, function () {
                    deferred.resolve();
                }, function() {
                    deferred.reject();
                });

                return deferred.promise;

            };


            return service;
        }
    ]);