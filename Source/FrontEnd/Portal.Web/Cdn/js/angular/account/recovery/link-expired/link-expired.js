// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.account.recovery.link-expired", [
        'constants.common',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.account.recovery.link-expired', {
                    url: '/link-expired',
                    data: {
                        pageTitle: 'Password recovery link has been expired - Clickberry',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'account.recovery.link-expired.html',
                    controller: 'AccountRecoveryLinkExpiredCtrl'
                });
        }
    ])
// Controllers
    .controller("AccountRecoveryLinkExpiredCtrl", [
        "$scope", "$state", "$timeout", "accountRecoveryService",
        function ($scope, $state, $timeout, accountRecoveryService) {


            var timeoutId;


            // PROPERTIES

            $scope.resendActive = true;



            // METHODS

            $scope.resend = function () {

                if (!$scope.recoverEmail) {
                    // going back to enter email
                    $state.go('^');
                    return;
                }

                // send email message
                $scope.resendActive = false;
                accountRecoveryService.sendEmail($scope.recoverEmail).then(function () {

                    deactivateButtonForSomeTime();

                }, function () {

                    $scope.resendActive = true;

                });

            };

            function deactivateButtonForSomeTime() {

                $scope.resendActive = false;

                timeoutId = $timeout(function () {

                    // make resend active after one minute
                    $scope.resendActive = true;

                }, 60000);

            }



            // EVENT HANDLERS

            // destructor
            $scope.$on('$destroy', function () {
                $timeout.cancel(timeoutId);
            });

        }
    ]);