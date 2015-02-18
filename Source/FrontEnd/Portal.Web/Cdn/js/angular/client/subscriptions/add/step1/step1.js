// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.subscriptions.add.step1", [
        'ui.router',
        'directives.model'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.subscriptions.add.step1', {
                    url: '/clients/subscriptions/add/step1',
                    data: {
                        pageTitle: 'New Subscription Step 1',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.subscriptions.add.step1.html',
                    controller: 'ClientSubscriptionsAddStep1Ctrl'
                });
        }
    ])

// Controllers
    .controller("ClientSubscriptionsAddStep1Ctrl", [
        "$scope", "$state",
        function($scope, $state) {

            $scope.selectType = function (type) {

                $scope.model.type = type;

                $state.go('^.step2');

            };

        }
    ]);