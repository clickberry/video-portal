// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.auth',
    [
        'ngResource',
        'services.cookies',
        'resources.session'
    ])
    .factory('authService', [
        '$http', '$q', 'directCookiesService', '$timeout', 'session', 'userRoles', function ($http, $q, directCookiesService, $timeout, session, userRoles) {

            var url = '/api/accounts/session';
            var service = {};

            var createSession = function() {

                var sessionId = directCookiesService.get('cbac');
                var rolesCookie = directCookiesService.get('cbroles');
                var roles = rolesCookie ? rolesCookie.split(',') : [];
                session.create(sessionId, roles);

            };


            service.login = function(loginModel) {

                return $http
                    .post(url, loginModel, { withCredentials: true }) // to allow cookies in response, details https://developer.mozilla.org/en-US/docs/Web/HTTP/Access_control_CORS (Requests with credentials)
                    .then(function() {

                        createSession();

                    });

            };

            service.logout = function() {

                return $http
                    .delete(url)
                    .then(function() {

                        session.destroy();

                    });

            };

            service.isAuthenticated = function() {

                // try to restore session
                if (!session.id) {
                    createSession();
                }

                return !!session.id;

            };

            service.isAuthorized = function(authorizedRoles) {

                if (!angular.isArray(authorizedRoles)) {
                    authorizedRoles = [authorizedRoles];
                }

                if (authorizedRoles.indexOf(userRoles.all) !== -1) {
                    // public access
                    return true;
                }

                if (!service.isAuthenticated()) {
                    return false;
                }


                for (var i = 0; i < session.userRoles.length; i++) {

                    if (authorizedRoles.indexOf(session.userRoles[i]) === -1) {
                        continue;
                    }

                    return true;

                }

                return false;

            };
            return service;
        }
    ]);