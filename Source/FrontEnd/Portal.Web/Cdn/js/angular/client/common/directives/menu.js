// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.clients.menu', [
        'resources.context',
        'services.auth',
        'ui.router'
    ])
    .directive('cbClientMenu', [
        '$rootScope', 'authEvents', 'authService', function ($rootScope, authEvents, authService) {

            return {
                restrict: 'EA',
                replace: true,
                templateUrl: 'client.menu.html',
                link: function($scope) {


                    // METHODS

                    $scope.signin = function () {

                        $rootScope.$broadcast(authEvents.authorize);

                    }

                    $scope.signOut = function() {
                        authService.logout().then(function () {

                            $rootScope.$broadcast(authEvents.logoutSuccess);

                        });
                    }

                }
            };
        }
    ])