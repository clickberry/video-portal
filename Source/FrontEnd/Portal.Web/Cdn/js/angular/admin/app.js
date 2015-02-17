// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Main module
angular.module("ngClickberry", [
        'ngClickberry.admin',
        'ui.router',
        'services.exceptions'
    ])

// Third party libraries
    .constant("jQuery", window.$)
    .constant("toastr", window.toastr)
    .constant("moment", window.moment)

// Default route
    .config([
       "$urlRouterProvider", "$locationProvider", function ($urlRouterProvider, $locationProvider) {

            $urlRouterProvider.otherwise('/admin');
            $locationProvider.html5Mode(true);

        }
    ])

// Use the main applications run method to execute any code after services have been instantiated.
    .run(function() {
    })

// Main application controller
    .controller("AppCtrl", [
        "$scope", function($scope) {
            $scope.$on('$stateChangeSuccess', function(event, toState /*, toParams, fromState, fromParams*/) {
                if (angular.isDefined(toState.data) && angular.isDefined(toState.data.pageTitle)) {
                    $scope.pageTitle = toState.data.pageTitle;
                }
            });
        }
    ]);