// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Main module
angular.module("ngClickberry", [
        'ui.router',
        'ngAnimate',
        'perfect_scrollbar',
        'constants.common',
        'ngClickberry.client',
        'services.exceptions',
        'services.auth',
        'services.auth.interceptor',
        'directives.config',
        'directives.form-auto-fill-fix',
        'directives.disable-hover-onscroll',
        'ipCookie'
    ])

// Third party libraries
    .constant("jQuery", window.$)
    .constant("toastr", window.toastr)
    .constant("stripe", window.Stripe)
    .constant("moment", window.moment)
    .constant("highcharts", window.Highcharts)

// Config
    .config([
        "$urlRouterProvider", "$locationProvider", function ($urlRouterProvider, $locationProvider) {

            $urlRouterProvider.otherwise('/clients');
            $locationProvider.html5Mode(true);

        }
    ])

// Use the main applications run method to execute any code after services have been instantiated.
    .run(function () {

        if (location.protocol === "http:") {
            location.href = location.href.toString().replace("http:","https:");
        }

    })

// Main application controller
    .controller("AppCtrl", [
        "$rootScope", "$state", "$window", "userRoles", "authService", "authEvents", function ($rootScope, $state, $window, userRoles, authService, authEvents) {

            $rootScope.currentUser = null;
            $rootScope.userRoles = userRoles;
            $rootScope.isAuthorized = authService.isAuthorized;


            // CUSTOM EVENTS

            $rootScope.$on(authEvents.loginSuccess, function () {

                if (!authService.isAuthorized(userRoles.client)) {
                    // go home
                    $window.location.href = '/';
                    return;
                }

                $state.go('client.subscriptions.list');

            });

            $rootScope.$on(authEvents.logoutSuccess, function () {

                // go home
                $window.location.href = '/';

            });


            // SYSTEM EVENTS

            $rootScope.$on('$stateChangeStart', function (event, next) {

                var authorizedRoles = next.data.authorizedRoles;
                if (!authService.isAuthorized(authorizedRoles)) {

                    event.preventDefault();

                    if (authService.isAuthenticated()) {
                        // user is not allowed
                        $rootScope.$broadcast(authEvents.notAuthorized);
                    } else {
                        // user is not logged in
                        $rootScope.$broadcast(authEvents.notAuthenticated);
                    }
                }
            });

            $rootScope.$on('$stateChangeSuccess', function (event, toState /*, toParams, fromState, fromParams*/) {
                if (angular.isDefined(toState.data) && angular.isDefined(toState.data.pageTitle)) {
                    $rootScope.pageTitle = toState.data.pageTitle;
                }
            });
        }
    ]);