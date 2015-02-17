// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.profile',
    ['ngResource'])
    .factory('profileService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/accounts/profile', null, {
                update: { method: 'PUT' }
            });


            var service = {};

            service.get = function() {

                var deferred = $q.defer();
                resource.get({}, function(profile) {

                    deferred.resolve(profile);

                }, function() {

                    deferred.reject("Could not get profile info");

                });

                return deferred.promise;

            };
            service.update = function(profileModel) {

                var deferred = $q.defer();
                resource.update(profileModel, function(profile) {

                    deferred.resolve(profile);

                }, function(response) {

                    deferred.reject(response);

                });

                return deferred.promise;

            };
            return service;
        }
    ]);