// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.subscriptions", [
        'ngClickberry.client.subscriptions.add',
        'ngClickberry.client.subscriptions.stats',
        'ui.router',
        'directives.loader',
        'directives.clients.subscriptions',
        'directives.clients.balance',
        'services.clients.subscriptions'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {

            $stateProvider
                .state('client.subscriptions', {
                    "abstract": true,
                    templateUrl: 'client.subscriptions.html',
                    controller: [
                        "$scope", "$state", "clientSubscriptionsService", function($scope, $state, clientSubscriptionsService) {


                            // PROPERTIES

                            $scope.subscription = null;


                            // METHODS

                            // function to load subscription info from child states
                            $scope.loadSubscription = function(id) {

                                // loading subscription by id
                                clientSubscriptionsService.get(id).then(function(subscription) {

                                        // caching subscription
                                        $scope.subscription = subscription;

                                    },
                                    function(reason) {
                                        throw new Error(reason);
                                    });

                            };

                        }
                    ]
                })
                .state('client.subscriptions.list', {
                    url: '/subscriptions',
                    data: {
                        pageTitle: 'My Subscriptions',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.subscriptions.list.html',
                    controller: 'ClientSubscriptionsListCtrl'
                });
        }
    ])

// Controllers
    .controller("ClientSubscriptionsListCtrl", [
        "$scope", "$interval", "clientSubscriptionsService", "$state",
        function($scope, $interval, clientSubscriptionsService, $state) {

            // PROPERTIES
            $scope.subscriptions = {
                free: [],
                basic: [],
                pro: [],
                custom: []
            };


            $scope.noSubscriptions = true;

            $scope.isLoading = false;


            $scope.currentTab = 1;


            // METHODS

            $scope.selectTab = function (tab) {
                $scope.currentTab = tab;
            };

            $scope.deleteSubscription = function(subscription) {

                clientSubscriptionsService.delete(subscription.id).then(function() {

                    // reloading subscriptions
                    loadSubscriptions();

                }, function(reason) {
                    throw new Error(reason);
                });
            };

            $scope.showSubscriptionStats = function(subscription) {

                // caching subscription
                $scope.subscription = subscription;

                // going to subscription statistics page
                $state.go("^.stats.list", subscription);

            };

            // PRIVATE METHODS

            function loadSubscriptions() {

                $scope.isLoading = true;

                clientSubscriptionsService.getAll()
                    .then(
                        function(data) {

                            $scope.isLoading = false;

                            if (!data.length) {
                                $scope.noSubscriptions = true;
                            } else {
                                $scope.noSubscriptions = false;
                            }

                            var free = [];
                            var basic = [];
                            var pro = [];
                            var custom = [];

                            for (var i = 0; i < data.length; i++) {
                                var s = data[i];
                                if (s.type == 0) {
                                    free.push(s);
                                } else if (s.type == 1) {
                                    basic.push(s);
                                } else if (s.type == 2) {
                                    pro.push(s);
                                } else if (s.type == 3) {
                                    custom.push(s);
                                }
                            }

                            $scope.subscriptions.free = free;
                            $scope.subscriptions.basic = basic;
                            $scope.subscriptions.pro = pro;
                            $scope.subscriptions.custom = custom;

                        },
                        function() {
                            $scope.isLoading = false;
                        });
            }


            // INIT

            // loading subscriptions
            loadSubscriptions();
        }
    ]);