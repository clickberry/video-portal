// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.signin', [
        'services.acs',
        'services.auth',
        'directives.popup',
        'directives.loader',
        'resources.settings'
    ])
    .directive('cbSignIn', [
        '$rootScope', '$q', '$timeout', 'settings', 'authService', 'acsService', 'authEvents', 'ipCookie', '$location',
        function ($rootScope, $q, $timeout, settings, authService, acsService, authEvents, $cookie, $location) {

            return {
                restrict: 'EA',
                replace: true,
                templateUrl: 'portal.signin.html',
                scope: {},
                link: function (scope) {

                    var timeoutId;


                    // PROPERTIES

                    scope.signinVisible = false;

                    scope.model = {
                        email: null,
                        password: null
                    };

                    scope.socials = [];

                    scope.isLoading = false;

                    scope.loginFailed = false;



                    // METHODS

                    function loadSocials() {
                        acsService.getProviders().then(
                           function (providers) {

                               scope.socials = providers;

                           });
                    }

                    scope.onClose = function() {
                        hide();
                    }

                    scope.submit = function (credentials) {

                        scope.isLoading = true;
                        scope.loginFailed = false;

                        // authenticate user
                        timeoutId = $timeout(function() {
                            authService.login(credentials).then(function() {

                                scope.isLoading = false;

                                $rootScope.$broadcast(authEvents.loginSuccess);
                                hide();

                            }, function() {

                                scope.isLoading = false;
                                scope.loginFailed = true;

                                $rootScope.$broadcast(authEvents.loginFailed);

                            });
                        }, 500);
                    };

                    function show() {

                        scope.signinVisible = true;
                        $cookie('returnUrl', $location.url(), { path: '/', expires: 1 });

                    }

                    function hide() {
                        
                        scope.signinVisible = false;
                        reset();

                    }

                    function reset() {

                        scope.loginFailed = false;
                        scope.model = {
                            email: null,
                            password: null
                        };

                    }




                    // EVENT HANDLERS

                    $rootScope.$on('$stateChangeSuccess', function () {
                        // hide if state changed
                        hide();
                    });

                    $rootScope.$on(authEvents.authorize, function () {
                        // authorization requested
                        show();
                    });

                    $rootScope.$on(authEvents.notAuthenticated, function () {
                        show();
                    });

                    $rootScope.$on(authEvents.notAuthorized, function () {
                        if (!$rootScope.currentUser && !$rootScope.profileLoading) {
                            show();
                        }
                    });

                    $rootScope.$on(authEvents.sessionTimeout, function () {
                        show();
                    });

                    // destructor
                    scope.$on('$destroy', function () {
                        $timeout.cancel(timeoutId);
                    });



                    // INIT

                    loadSocials();

                }
            };
        }
    ])