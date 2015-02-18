// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.signup.step2", [
        'ui.router',
        'directives.model',
        'directives.select',
        'services.clients',
        'resources.common'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.signup.step2', {
                    url: '/signup/step2',
                    data: {
                        pageTitle: 'Sign Up Step 2',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'client.signup.step2.html',
                    controller: 'ClientSignupStep2Ctrl'
                });
        }
    ])

// Controllers
    .controller("ClientSignupStep2Ctrl", [
        "$scope", "$state", "clientsService", "resources",
        function ($scope, $state, clientsService, resources) {

            $scope.countries = resources.clientCountries;

            $scope.showEin = false;
            var showEinTrigger = function() {

                if ($scope.model.country == "us") {
                    return true;
                } else {
                    return false;
                }
            };
            $scope.$watch(showEinTrigger, function(value) {
                $scope.showEin = value;
            });


            $scope.submitStep2 = function(clientModel) {

                $scope.isLoading = true;

                // register client
                clientsService.add(clientModel).then(function (data) {

                    $scope.isLoading = false;

                    // caching client data
                    $scope.client = data;

                    $state.go('^.success');

                }, function (reason) {
                    $scope.isLoading = false;

                    if (reason.page === "step1") {
                        $state.go('^.step1');
                    }

                    throw new Error(reason.message);
                });

            };

        }
    ]);