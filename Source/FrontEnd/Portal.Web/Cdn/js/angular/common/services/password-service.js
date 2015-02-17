// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.password',
    ['ngResource'])
    .factory('passwordService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/accounts/emails', null, {
                update: { method: 'PUT' }
            });


            var service = {};

            service.update = function(passwordModel) {

                var deferred = $q.defer();
                resource.update(passwordModel, function () {

                    deferred.resolve();

                }, function(reponse) {

                    deferred.reject(reponse);

                });

                return deferred.promise;

            };
            return service;
        }
    ]);