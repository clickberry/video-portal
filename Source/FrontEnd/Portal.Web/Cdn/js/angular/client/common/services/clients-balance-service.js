// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.clients.balance',
    ['ngResource'])
    .factory('clientBalanceService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/clients/balance');

            var service = {};

            service.get = function () {

                var deferred = $q.defer();
                resource.get({}, function (balance) {

                    deferred.resolve(balance.amount);

                }, function () {

                    deferred.reject("Could not get client balance");

                });

                return deferred.promise;

            }

            return service;
        }
    ]);