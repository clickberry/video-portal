// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.client", [
        'ngClickberry.client.signup',
        'ngClickberry.client.profile',
        'ngClickberry.client.subscriptions',
        'ngClickberry.client.balance',
        'ngClickberry.client.pay',
        'directives.popup',
        'directives.clients.menu',
        'directives.signin',
        'services.clients',
        'services.auth',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "$urlRouterProvider", "userRoles", function ($stateProvider, $urlRouterProvider, userRoles) {
            $stateProvider
                .state('client', {
                    url: '/clients',
                    data: {
                        pageTitle: 'Clickberry Partner Program',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'client.html',
                    controller: [
                        "$rootScope", "$state", "$q", "authService", "clientsService", "authEvents", function ($rootScope, $state, $q, authService, clientsService, authEvents) {


                            // PROPERTIES

                            $rootScope.client = null;


                            // METHODS

                            function loadClient() {

                                var deferred = $q.defer();
                                clientsService.get().then(function(client) {
                                        // caching client
                                        $rootScope.client = client;

                                        deferred.resolve(client);
                                    },
                                    function() {
                                        // failed to get client info, maybe client's company is blocked or deleted
                                        $state.go('client.signup.step1');

                                        deferred.reject("Could not get client info");
                                    });

                                return deferred.promise;
                            }



                            // EVENT HANDLERS

                            $rootScope.$on(authEvents.loginSuccess, function () {

                                loadClient().then(function() {
                                    $state.go('client.subscriptions.list');
                                });

                            });



                            // INIT

                            // checking whether we are authenticated and client info does not exist
                            if (!$rootScope.client && authService.isAuthenticated()) {

                                // loading client info
                                loadClient();
                            }
                        }
                    ]
                });

            $urlRouterProvider.when('/', '/clients');
        }
    ]);