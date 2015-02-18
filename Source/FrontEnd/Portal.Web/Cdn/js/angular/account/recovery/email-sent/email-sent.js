// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.account.recovery.email-sent", [
        'constants.common',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.account.recovery.email-sent', {
                    url: '/email-sent',
                    data: {
                        pageTitle: 'Email with instructions has been sent - Clickberry',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'account.recovery.email-sent.html',
                    controller: 'AccountRecoveryEmailSentCtrl'
                });
        }
    ])
// Controllers
    .controller("AccountRecoveryEmailSentCtrl", [
        "$scope", "$timeout", "$state", "accountRecoveryService",
        function ($scope, $timeout, $state, accountRecoveryService) {

            var timeoutId;


            // PROPERTIES

            $scope.resendActive = false;



            // METHODS

            $scope.resend = function() {

                if (!$scope.recoverEmail) {
                    // going back to enter email
                    $state.go('^');
                    return;
                }

                // send email message
                $scope.resendActive = false;
                accountRecoveryService.sendEmail($scope.recoverEmail).then(function () {

                    deactivateButtonForSomeTime();

                }, function() {
                    
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



            // INIT 

            deactivateButtonForSomeTime();
        }
    ]);