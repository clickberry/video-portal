// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.auth.interceptor', [])
    .factory('authInterceptor', [
        '$rootScope', '$q', 'authEvents', function($rootScope, $q, authEvents) {
            return {
                responseError: function(response) {
                    if (response.status === 401) {
                        $rootScope.$broadcast(authEvents.notAuthenticated, response);
                    }
                    if (response.status === 403) {
                        $rootScope.$broadcast(authEvents.notAuthorized, response);
                    }
                    if (response.status === 419 || response.status === 440) {
                        $rootScope.$broadcast(authEvents.sessionTimeout, response);
                    }
                    return $q.reject(response);
                }
            };
        }
    ])
    .config([
        "$httpProvider", function($httpProvider) {

            $httpProvider.interceptors.push('authInterceptor');

        }
    ])