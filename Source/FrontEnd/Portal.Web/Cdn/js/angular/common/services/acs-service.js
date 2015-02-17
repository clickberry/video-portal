// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.acs',
    ['ngResource'])
    .factory('acsService', [
        '$resource', '$q', function ($resource, $q) {

            var resource = $resource('/api/accounts/networks');
            var service = {};

            service.getProviders = function() {

                var deferred = $q.defer();

                resource.query({}, function (data) {

                    data = data.sort(function (a, b) {
                        return a.name > b.name ? 1 : -1;
                    });

                    var p = [];
                    for (var i = 0; i < data.length; i++) {
                        var name = data[i].name;
                        var url = data[i].loginUrl;
                        var className = null;

                        if (name.toLowerCase().indexOf('facebook') > -1) {
                            className = 'fb';
                        }
                        else if (name.toLowerCase().indexOf('twitter') > -1) {
                            className = 'tw';
                        }
                        else if (name.toLowerCase().indexOf('google') > -1) {
                            className = 'google';
                        }
                        else if (name.toLowerCase().indexOf('vk') > -1) {
                            className = 'vk';
                        }
                        else if (name.toLowerCase().indexOf('odnoklassniki') > -1) {
                            className = 'odn';
                        }
                        else if (name.toLowerCase().indexOf('windows') > -1) {
                            className = 'wind';
                        }
                        else if (name.toLowerCase().indexOf('yahoo') > -1) {
                            className = 'yahoo';
                        }

                        if (className != null) {
                            p.push({ className: className, url: url });
                        }
                    }

                    deferred.resolve(p);

                }, function () {
                    deferred.reject("Failed to get ACS providers");
                });

                return deferred.promise;

            };

            return service;
        }
    ]);