// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.clients.balance', [
        'services.clients.balance',
        'services.auth'
    ])
    .directive('cbClientBalanceWidget', [
        '$interval', 'authService', 'clientBalanceService',
            function ($interval, authService, clientBalanceService) {

            return {
                restrict: 'E',
                scope: {},
                templateUrl: 'client.balance.widget.html',
                controller: ['$scope', function($scope) {

                    if (!authService.isAuthenticated()) {
                        // going to sign in page
                        return;
                    }



                    // PROPERTIES

                    $scope.balance = null;


                    // METHODS

                    function loadBalance() {
                        clientBalanceService.get().then(function(data) {

                                // rounding to cents
                                var modulo = Math.ceil(Math.abs(data));
                                var balance = data >= 0 ? modulo : -modulo;
                                $scope.balance = balance;
                            });
                    }



                    // INIT

                    // loading balance
                    loadBalance();

                    // reloading balance periodically
                    var timeoutId = $interval(function() {
                        loadBalance();
                    }, 60000);



                    // EVENT HANDLERS

                    // destructor
                    $scope.$on('$destroy', function() {
                        $interval.cancel(timeoutId);
                    });

                }]
            };
        }
    ])