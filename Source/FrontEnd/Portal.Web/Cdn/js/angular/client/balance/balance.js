// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.balance", [
        'ui.router',
        'directives.loader',
        'directives.clients.balance',
        'services.clients.balance.details',
        'resources.context'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.balance', {
                    url: '/balance',
                    data: {
                        pageTitle: 'Client Balance Details',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.balance.html',
                    controller: 'ClientBalanceCtrl'
            });
        }
    ])

// Controllers
    .controller("ClientBalanceCtrl", [
        "$scope", "$state", "context", "clientBalanceDetailsService",
        function ($scope, $state, context, clientBalanceDetailsService) {



            // PROPERTIES

            $scope.paymentSucceed = context.lastPaymentSucceed;
            context.lastPaymentSucceed = null;

            $scope.balanceHistory = [];

            $scope.filter = {
                orderBy: 'Date',
                orderByAsc: false,
                skip: 0,
                top: 20,
                inlinecount: 'allpages'
            };

            $scope.isAllLoaded = false;

            $scope.isLoading = false;


            // METHODS

            $scope.nextPage = function () {

                if ($scope.isLoading || $scope.isAllLoaded) {
                    return;
                }

                $scope.filter.skip += $scope.filter.top;

                // loading balance history
                filterBalanceHistory();

            };

            $scope.setOrder = function (order) {

                // reseting page
                $scope.filter.skip = 0;
                $scope.isAllLoaded = false;

                if ($scope.filter.orderBy == order) {
                    // just changing direction
                    $scope.filter.orderByAsc = !$scope.filter.orderByAsc;
                } else {
                    $scope.filter.orderBy = order;
                    $scope.filter.orderByAsc = true;
                }

                // reloading balance history
                filterBalanceHistory();
            };



            // PRIVATE METHODS

            function filterBalanceHistory() {

                if (!$scope.filter.skip) {
                    $scope.balanceHistory = [];
                }

                $scope.isLoading = true;

                clientBalanceDetailsService.query($scope.filter).then(
                    function (data) {

                        $scope.isLoading = false;

                        if ($scope.filter.skip > 0) {
                            $scope.balanceHistory = $scope.balanceHistory.concat(data);
                        } else {
                            $scope.balanceHistory = data;
                        }

                        if (data.length < $scope.filter.top) {
                            $scope.isAllLoaded = true;
                        } else {
                            $scope.isAllLoaded = false;
                        }

                    },
                    function (reason) {
                        $scope.isLoading = false;
                        throw new Error(reason);
                    });
            }



            // INIT

            // loading balance history
            filterBalanceHistory();
        }
    ]);