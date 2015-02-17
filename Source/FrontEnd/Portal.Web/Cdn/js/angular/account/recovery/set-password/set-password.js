// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.account.recovery.set-password", [
        'constants.common',
        'services.auth',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.account.recovery.set-password', {
                    url: '/set-password/?i&e',
                    data: {
                        pageTitle: 'Choose your new password for Clickberry account',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'account.recovery.set-password.html',
                    controller: 'AccountRecoverySetPasswordCtrl'
                });
        }
    ])
// Controllers
    .controller("AccountRecoverySetPasswordCtrl", [
        "$scope", "$rootScope",  "$state", "$stateParams", "accountRecoveryService", "authService", "authEvents",
        function ($scope, $rootScope, $state, $stateParams, accountRecoveryService, authService, authEvents) {

            if (!$stateParams.i || !$stateParams.e) {

                $state.go('^');
                return;

            }



            // PROPERTIES

            $scope.linkValidated = false;

            $scope.newPassword = null;
            $scope.newPasswordConfirm = null;

            $scope.isLoading = false;


            // METHODS

            $scope.submitPassword = function(pass, passConfirm) {

                $scope.isLoading = true;

                var model = {
                    password: pass,
                    confirmation: passConfirm,
                    i: $stateParams.i,
                    e: $stateParams.e
                };

                // reset password
                accountRecoveryService.resetPassword(model).then(function() {
                    
                    // password reseted
                    $scope.isLoading = false;
                    $rootScope.$broadcast(authEvents.authorize);

                }, function () {

                    // link expired
                    $state.go('^.link-expired');
                    return;

                });

            };

            function validateLink(i, e) {


                $scope.isLoading = true;

                accountRecoveryService.validate({ i: i, e: e }).then(function() {

                    $scope.linkValidated = true;
                    $scope.isLoading = false;


                }, function() {

                    $state.go('^.link-expired');
                    return;

                });

            }



            // INIT

            validateLink($stateParams.i, $stateParams.e);

        }
    ]);