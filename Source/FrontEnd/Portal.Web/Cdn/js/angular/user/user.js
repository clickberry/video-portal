// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.user", [
        'ngClickberry.user.videos',
        'ngClickberry.user.profile',
        'ngClickberry.user.likes',
        'services.user',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", function($stateProvider) {
            $stateProvider
                .state('portal.user', {
                    "abstract": true,
                    template: '<div ui-view></div>',
                    controller: [
                        "$scope", "$state", "userService", function($scope, $state, userService) {


                            // PROPERTIES

                            $scope.user = null;


                            // METHODS

                            // function to load user info from child states
                            $scope.loadUser = function(id) {

                                // loading subscription by id
                                userService.get(id).then(function(user) {

                                        // caching user
                                        $scope.user = user;

                                    });

                            };

                        }
                    ]
                });
        }
    ]);