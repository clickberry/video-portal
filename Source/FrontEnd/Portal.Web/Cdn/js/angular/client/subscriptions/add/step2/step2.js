// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.subscriptions.add.step2", [
        'ui.router',
        'directives.model',
        'services.clients.subscriptions'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.subscriptions.add.step2', {
                    url: '/clients/subscriptions/add/step2',
                    data: {
                        pageTitle: 'New Subscription Step 2',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.subscriptions.add.step2.html',
                    controller: 'ClientSubscriptionsAddStep2Ctrl'
                });
        }
    ])

// Controllers
    .controller("ClientSubscriptionsAddStep2Ctrl", [
        "$scope", "$state", "clientSubscriptionsService",
        function ($scope, $state, clientSubscriptionsService) {
            
            if ($scope.model.type == null) {
                // if type not selected going to step1
                $state.go('^.step1');
            }

            // PROPERTIES

            $scope.agreedWithTerms = false;


            // METHODS

            $scope.cancel = function () {

                // going to subscriptions page
                $state.go('^.^.list');

            }

            $scope.submit = function() {

                $scope.isLoading = true;

                // create subscription
                clientSubscriptionsService.add($scope.model).then(function () {

                    $scope.isLoading = false;

                    // going to subscriptions page
                    $state.go('^.^.list');

                }, function (reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });

            };

        }
    ]);