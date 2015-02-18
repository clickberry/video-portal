// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.account.recovery',
    ['ngResource'])
    .factory('accountRecoveryService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/accounts/recovery', null, {
                update: { method: 'PUT' }
            });

            var resourceValidate = $resource('/api/accounts/recovery/validate');


            var service = {};

            service.sendEmail = function(email) {

                var deferred = $q.defer();
                resource.save({ email: email }, function() {

                    deferred.resolve();

                }, function() {

                    deferred.reject("Faild to send password recovery email");

                });

                return deferred.promise;

            };

            service.validate = function(validationModel) {

                var deferred = $q.defer();
                resourceValidate.save(validationModel, function () {

                    deferred.resolve();

                }, function() {

                    deferred.reject("Password recovery link validation failed");

                });

                return deferred.promise;

            };

            service.resetPassword = function(resetPasswordModel) {

                var deferred = $q.defer();
                resource.update(resetPasswordModel, function() {

                    deferred.resolve();

                }, function() {

                    deferred.reject("Failed to reset password");

                });

                return deferred.promise;

            };

            return service;
        }
    ]);