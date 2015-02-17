// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.client.subscriptions.add", [
        'ngClickberry.client.subscriptions.add.step1',
        'ngClickberry.client.subscriptions.add.step2',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('client.subscriptions.add', {
                    "abstract": true,
                    templateUrl: 'client.subscriptions.add.html',
                    controller: [
                        "$scope", function($scope) {

                            // shared with child views
                            $scope.model = {
                                type: null,
                                siteHostname: null,
                                siteName: null,
                                googleAnalyticsId: null
                            };

                            $scope.isLoading = false;
                        }
                    ]
                });

            $urlRouterProvider.when('/clients/subscriptions/add', '/clients/subscriptions/add/step1');
        }
    ]);