// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.signup", [
        'ngClickberry.client.signup.step1',
        'ngClickberry.client.signup.step2',
        'ngClickberry.client.signup.success',
        'directives.loader',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('client.signup', {
                    "abstract": true,
                    templateUrl: 'client.signup.html',
                    controller: [
                        "$scope", function($scope) {

                            // shared with child views
                            $scope.model = {
                                email: null,
                                confirmEmail: null,
                                password: null,
                                confirmPassword: null,
                                country: null,
                                companyName: null,
                                contactPerson: null,
                                ein: null,
                                address: null,
                                zipCode: null,
                                phoneNumber: null
                            };

                            $scope.isLoading = false;
                        }
                    ]
                });

            $urlRouterProvider.when('/clients/signup', '/clients/signup/step1');
        }
    ]);