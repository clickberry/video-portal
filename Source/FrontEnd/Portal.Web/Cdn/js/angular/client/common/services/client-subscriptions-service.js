// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.clients.subscriptions',
    ['ngResource'])
    .factory('clientSubscriptionsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/clients/subscriptions/:id', { id: '@id' }, {
                update: { method: 'PUT' }
            });



            var service = {};

            service.add = function(subscriptionModel) {
                
                var deferred = $q.defer();
                resource.save(subscriptionModel, function (subscription) {

                    deferred.resolve(subscription);

                }, function () {

                    deferred.reject("Could not create client subscription");

                });

                return deferred.promise;

            }

            service.get = function (id) {

                var deferred = $q.defer();
                resource.get({id: id}, function (subscription) {

                    deferred.resolve(subscription);

                }, function () {

                    deferred.reject("Could not get client subscription by id");

                });

                return deferred.promise;

            }

            service.getAll = function () {

                var deferred = $q.defer();
                resource.query({'$orderby': 'State asc'}, function (subscriptions) {

                    deferred.resolve(subscriptions);

                }, function () {

                    deferred.reject("Could not get client subscriptions");

                });

                return deferred.promise;

            }

            service.update = function (subscriptionModel) {

                var deferred = $q.defer();
                resource.update(subscriptionModel, function (client) {

                    deferred.resolve(client);

                }, function () {

                    deferred.reject("Failed to update client subscription");

                });

                return deferred.promise;

            }

            service.delete = function (id) {

                var deferred = $q.defer();
                resource.delete({id: id}, function () {

                    deferred.resolve();

                }, function () {

                    deferred.reject("Failed to delete client subscription");

                });

                return deferred.promise;

            }

            return service;
        }
    ]);