// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.client.pay", [
        'ui.router',
        'ui.mask',
        'services.clients.payments',
        'services.stripe',
        'directives.loader',
        'directives.model',
        'resources.context'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('client.pay', {
                    url: '/pay',
                    data: {
                        pageTitle: 'Add Payment',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.pay.html',
                    controller: 'ClientPayCtrl'
                });
        }
    ])

// Controllers
    .controller("ClientPayCtrl", [
        "$scope", "$state", "$timeout", "$window", "context", "stripeService", "clientPaymentsService",
        function($scope, $state, $timeout, $window, context, stripeService, clientPaymentsService) {


            var timeoutId;


            // PROPERTIES

            $scope.model = {
                amount: null,
                cardNumber: null,
                cardExp: null,
                cardCVC: null
            };

            $scope.isLoading = false;


            // METHODS

            $scope.cancel = function() {

                $window.history.back();

            };
            $scope.submit = function() {

                $scope.isLoading = true;
                var amountInCents = parseInt($scope.model.amount) * 100;

                var expRegexp = /^(0[1-9]|1[0-2])\/(\d{2})$/;
                var match = $scope.model.cardExp.match(expRegexp);
                var month = match[1];
                var year = match[2];

                // create card token
                stripeService.card.createToken({
                        number: $scope.model.cardNumber,
                        cvc: $scope.model.cardCVC,
                        exp_month: month,
                        exp_year: year
                    },
                    function(status, response) {

                        if (response.error) {

                            $scope.$apply(function() {
                                $scope.isLoading = false;
                                throw new Error(response.error.message);
                            });


                        } else {


                            var tokenId = response['id'];

                            // make payment
                            $scope.$apply(function() {

                                clientPaymentsService.pay({ amountInCents: amountInCents, tokenId: tokenId }).then(function() {

                                        $scope.isLoading = false;

                                        // going to balance state
                                        context.lastPaymentSucceed = true;
                                        $state.go('^.balance');

                                    },
                                    function() {

                                        $scope.isLoading = false;

                                        // going to balance state
                                        context.lastPaymentSucceed = false;
                                        $state.go('^.balance');
                                    });

                            });
                        }
                    });


                // destructor
                $scope.$on('$destroy', function() {
                    if (timeoutId) {
                        $timeout.cancel(timeoutId);
                    }
                });

            };
        }
    ]);