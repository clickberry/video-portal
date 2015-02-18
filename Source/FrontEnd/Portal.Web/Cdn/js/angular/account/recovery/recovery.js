// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.account.recovery", [
        'ngClickberry.account.recovery.email-sent',
        'ngClickberry.account.recovery.link-expired',
        'ngClickberry.account.recovery.set-password',
        'constants.common',
        'services.account.recovery',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.account.recovery', {
                    url: '/account/recovery',
                    data: {
                        pageTitle: 'Forgot your password on Clickberry?',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'account.recovery.html',
                    controller: 'AccountRecoveryCtrl'
                });
        }
    ])
// Controllers
    .controller("AccountRecoveryCtrl", [
        "$scope", "$state", "accountRecoveryService",
        function ($scope, $state, accountRecoveryService) {

            // PROPERTIES

            $scope.email = null;

            $scope.recoverEmail = null;

            $scope.isLoading = false;



            // METHODS

            $scope.submitEmail = function(email) {

                $scope.isLoading = true;

                // copy email
                $scope.recoverEmail = email;

                // send email message
                accountRecoveryService.sendEmail(email).then(function() {

                    $state.go('.email-sent');

                }, function() {

                    $scope.isLoading = false;

                    throw new Error('Faild to send password recovery email. Try again later.');

                });

            }

        }
    ]);