// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.signup.step1", [
        'ui.router',
        'directives.model'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.signup.step1', {
                    url: '/signup/step1',
                    data: {
                        pageTitle: 'Sign Up Step 1',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'client.signup.step1.html',
                    controller: 'ClientSignupStep1Ctrl'
                });
        }
    ])

// Controllers
    .controller("ClientSignupStep1Ctrl", [
        "$scope", "$state",
        function($scope, $state) {

            $scope.submitStep1 = function() {
                $state.go('^.step2');
            };

        }
    ]);