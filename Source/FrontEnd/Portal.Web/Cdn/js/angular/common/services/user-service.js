// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.user',
    ['ngResource'])
    .factory('userService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/users/:id', {id: '@id'});


            var service = {};

            service.get = function(id) {

                var deferred = $q.defer();
                resource.get({id: id}, function(user) {

                    user.id = user.id || id;
                    deferred.resolve(user);

                }, function() {

                    deferred.reject("Could not get user by id");

                });

                return deferred.promise;

            };
            
            return service;
        }
    ]);