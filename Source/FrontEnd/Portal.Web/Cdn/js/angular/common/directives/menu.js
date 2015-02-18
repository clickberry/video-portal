// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('directives.menu', [
        'directives.progressbar',
        'resources.context',
        'services.auth',
        'ui.router'
    ])
    .directive('cbMenu', [
        '$rootScope', '$state', '$stateParams', 'authEvents', 'authService', function ($rootScope, $state, $stateParams, authEvents, authService) {

            return {
                restrict: 'EA',
                replace: true,
                templateUrl: 'portal.menu.html',
                link: function($scope) {

                    $scope.state = $state;
                    $scope.stateParams = $stateParams;

                    // METHODS

                    $scope.signin = function() {
                        
                        $rootScope.$broadcast(authEvents.authorize);

                    }

                    $scope.signOut = function () {
                        authService.logout().then(function () {

                            $rootScope.$broadcast(authEvents.logoutSuccess);

                        });
                    }

                }
            };
        }
    ])