// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.memberships',
    ['ngResource'])
    .factory('membershipsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/accounts/profile/memberships/:provider', { provider: '@provider' });

            var service = {};

            service.delete = function (provider) {

                var deferred = $q.defer();
                resource.delete({ provider: provider }, function () {
                    deferred.resolve();
                }, function() {
                    deferred.reject("Failed to delete user membership");
                });

                return deferred.promise;

            };
            return service;
        }
    ]);