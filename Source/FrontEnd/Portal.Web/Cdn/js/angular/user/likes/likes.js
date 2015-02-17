// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.user.likes", [
        'ngClickberry.user.likes.video',
        'ui.router',
        'directives.masonry',
        'directives.loader',
        'services.likes',
        'services.watch',
        'constants.common',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('portal.user.likes', {
                    url: '/user/likes',
                    data: {
                        pageTitle: 'My Likes on Clickberry',
                        authorizedRoles: [userRoles.user]
                    },
                    templateUrl: 'user.likes.html',
                    controller: 'UserLikesCtrl'
                });
        }
    ])
// Controllers
    .controller("UserLikesCtrl", [
        "$scope", "likesService", "userEvents",
        function ($scope, likesService, userEvents) {


            // PROPERTIES

            $scope.likes = [];

            $scope.filter = {
                skip: 0,
                top: 20
            };

            $scope.isAllLoaded = false;
            $scope.isLoading = false;


            // METHODS

            $scope.nextPage = function () {

                if ($scope.isLoading || $scope.isAllLoaded) {
                    return;
                }

                $scope.filter.skip += $scope.filter.top;

                // loading likes
                filterLikes();
            };


            // PRIVATE METHODS

            function filterLikes() {

                if (!$scope.filter.skip) {
                    $scope.likes = [];
                }

                $scope.isLoading = true;
                likesService.queryLikes($scope.filter).then(
                    function (data) {

                        if ($scope.filter.skip > 0) {
                            $scope.likes = $scope.likes.concat(data);
                        } else {
                            $scope.likes = data;
                        }

                        if (data.length < $scope.filter.top) {
                            $scope.isAllLoaded = true;
                        } else {
                            $scope.isAllLoaded = false;
                        }

                        $scope.isLoading = false;

                    },
                    function () {
                        $scope.isLoading = false;
                    });
            };

            function reloadLikes() {
                $scope.filter.skip = 0;
                filterLikes();
            }


            // EVENT HANDLERS

            $scope.$on(userEvents.likeDeleted, function () {
                reloadLikes();
            });

            $scope.$on(userEvents.likeAdded, function () {
                reloadLikes();
            });



            // INIT

            filterLikes();

        }
    ]);