// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.clients.payments',
    ['ngResource'])
    .factory('clientPaymentsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/clients/payments');

            var service = {};

            service.pay = function (model) {

                var deferred = $q.defer();
                resource.save(model, function () {

                    deferred.resolve();

                }, function () {

                    deferred.reject("Payment fail, try again later");

                });

                return deferred.promise;

            }

            return service;
        }
    ]);