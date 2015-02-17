// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.clients',
    ['ngResource'])
    .factory('clientsService', [
        '$resource', '$q', function ($resource, $q) {

            var resource = $resource('/api/clients', null, {
                update: { method: 'PUT' }
            });



            var service = {};

            service.add = function(clientModel) {
                
                var deferred = $q.defer();
                resource.save(clientModel, function (client) {

                    deferred.resolve(client);

                }, function (response) {

                    var error = {
                        message: "Client registration failed",
                        page: "step2"
                    };

                    if (response.data && response.data.modelState) {
                        
                        var error_messages = new Array();

                        for (var key in response.data.modelState) {
                            for (var i = 0; i < response.data.modelState[key].length; i++) {
                                error_messages.push(response.data.modelState[key][i]);
                            }

                            if (key === "password" || key === "email") {
                                error.page = "step1";
                            }

                        }

                        error.message = error_messages.join("<br />");

                    }

                    if (response.status && response.status == "409") {
                        error.message = "Email already exist";
                        error.page = "step1";
                    }


                    deferred.reject(error);



                });

                return deferred.promise;

            }

            service.get = function () {

                var deferred = $q.defer();
                resource.get({}, function (client) {

                    deferred.resolve(client);

                }, function () {

                    deferred.reject("Could not get client info");

                });

                return deferred.promise;

            }

            service.update = function (clientModel) {

                var deferred = $q.defer();
                resource.update(clientModel, function (client) {

                    deferred.resolve(client);

                }, function () {

                    deferred.reject("Failed to update client info");

                });

                return deferred.promise;

            }

            return service;
        }
    ]);