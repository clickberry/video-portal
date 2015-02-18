// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

// Main module
angular.module("ngClickberry", [
        'ngClickberry.video-standalone',
        'ui.router',
        'constants.common',
        'services.exceptions',
        'services.auth',
        'services.auth.interceptor',
        'services.profile',
        'directives.config',
        'directives.form-auto-fill-fix',
        'directives.disable-hover-onscroll',
        'ipCookie',
        'perfect_scrollbar',
        'directives.confirm-click'
    ])

// Third party libraries
    .constant("jQuery", window.$)
    .constant("toastr", window.toastr)
    .constant("moment", window.moment)

// Config
    .config([
        "$urlRouterProvider", "$locationProvider", function ($urlRouterProvider, $locationProvider) {

            $urlRouterProvider.otherwise('/');
            $locationProvider.html5Mode(true);

        }
    ])

// Use the main applications run method to execute any code after services have been instantiated.
    .run(function () {
    })

// Main application controller
    .controller("AppCtrl", [
        "$rootScope", "$state", "$window", "userRoles", "authService", "authEvents", "profileService",
        function ($rootScope, $state, $window, userRoles, authService, authEvents, profileService) {

            $rootScope.currentUser = null;
            $rootScope.profileLoading = true;
            $rootScope.userRoles = userRoles;
            $rootScope.isAuthorized = authService.isAuthorized;


            // METHODS

            function loadProfile() {

                // loading profile info
                profileService.get().then(function (user) {

                    // caching user data
                    $rootScope.currentUser = user;
                    $rootScope.profileLoading = false;

                });

            }

            function unloadProfile() {
                
                $rootScope.currentUser = null;

            }



            // EVENT HANDLERS

            $rootScope.$on(authEvents.loginSuccess, function () {

                loadProfile();

            });

            $rootScope.$on(authEvents.logoutSuccess, function () {

                unloadProfile();

                $window.location = '/';

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



            // INIT

            // checking whether we are authenticated and user info does not exist
            if (!$rootScope.currentUser && authService.isAuthenticated()) {

                // loading client info
                loadProfile();
            }

        }
    ]);