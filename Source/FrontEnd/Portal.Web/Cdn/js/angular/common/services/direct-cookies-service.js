// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

// simple read-only service for direct access of document.cookie

angular.module('services.cookies',
    ['ngResource'])
    .factory('directCookiesService', [
        function() {

            var service = {
                get: function(key) {

                    if (key == null) {
                        return null;
                    }

                    var result = undefined;

                    var cookies = document.cookie ? document.cookie.split('; ') : [];

                    for (var i = 0, l = cookies.length; i < l; i++) {
                        var parts = cookies[i].split('=');
                        var name = parts[0];
                        var value = parts[1];

                        if (key === name) {
                            result = value;
                            break;
                        }


                    }

                    return result;
                }

            };


            return service;
        }
    ]);