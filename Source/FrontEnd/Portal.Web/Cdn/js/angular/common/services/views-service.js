// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.views',
    [
        'ngResource'
    ])
    .factory('viewsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/views/:id', { id: '@id' });

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