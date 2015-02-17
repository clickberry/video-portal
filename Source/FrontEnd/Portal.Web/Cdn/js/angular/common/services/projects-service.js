// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.projects',
    [
        'ngResource'
    ])
    .factory('projectsService', [
        '$resource', '$q', function ($resource, $q) {

            var resource = $resource('/api/projects/:id', { id: '@id' }, {
                'delete': { method: 'DELETE' },
                'update': { method: 'PUT' }
            });

            function escape(value) {
                return value.replace(/'/g, "''").replace(/#/g, '%23');
            };

            var service = {};

            service.update = function (newData) {

                // making HTTP request

                var deferred = $q.defer();
                resource.update(newData, function (data) {

                    deferred.resolve(data);

                }, function () {

                    deferred.reject("Could not update project");

                });

                return deferred.promise;

            };


            service.delete = function (id) {

                // making HTTP request

                var deferred = $q.defer(id);
                resource.delete({ id: id }, function (data) {

                    deferred.resolve(data);

                }, function () {

                    deferred.reject("Could not delete project");

                });

                return deferred.promise;

            };


            return service;
        }
    ]);