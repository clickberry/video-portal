// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.user.videos", [
        "ngClickberry.user.videos.video",
        'ui.router',
        'constants.common',
        'directives.masonry',
        'directives.loader',
        'directives.confirm-click',
        'services.watch',
        'services.projects',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('portal.user.videos', {
                    url: '/user/{userId}/videos',
                    data: {
                        pageTitle: 'User Videos on Clickberry Online Storage',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'user.videos.html',
                    controller: 'UserVideosCtrl'
                });
        }
    ])
// Controllers
    .controller("UserVideosCtrl", [
        "$scope", "$state", "$stateParams", "watchService", "projectsService", "userEvents",
        function ($scope, $state, $stateParams, watchService, projectsService, userEvents) {

            var userId = $stateParams.userId;

            if (!$scope.user || $scope.user.id != userId) {

                // loading user
                $scope.loadUser(userId);
            }



            // PROPERTIES

            $scope.watches = [];

            $scope.filter = {
                orderBy: 'Created',
                orderByAsc: false,
                userId: userId,
                name: null,
                skip: 0,
                top: 20
            };

            $scope.nameFilter = null;

            $scope.isAllLoaded = false;
            $scope.isLoading = false;


            // METHODS

            $scope.submitSearch = function(query) {

                $scope.filter.name = query;

                // reloading watches
                reloadWatches();
            }

            $scope.nextPage = function () {

                if ($scope.isLoading || $scope.isAllLoaded) {
                    return;
                }

                $scope.filter.skip += $scope.filter.top;

                // loading watches
                filterWatches();
            };

            $scope.deleteVideo = function (project) {
                projectsService.delete(project.id).then(function () {
                    reloadWatches();
                });
            };

            $scope.togglePublic = function (project) {
                project.access++;
                if (project.access > 2) {
                    project.access = 0;
                }
                projectsService.update(project);
            }


            // PRIVATE METHODS

            function filterWatches() {

                if (!$scope.filter.skip) {
                    $scope.watches = [];
                }

                $scope.isLoading = true;
                watchService.query($scope.filter).then(
                    function (data) {

                        if ($scope.filter.skip > 0) {
                            $scope.watches = $scope.watches.concat(data);
                        } else {
                            $scope.watches = data;
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

            function reloadWatches() {
                $scope.filter.skip = 0;
                filterWatches();
            }

            // EVENT HANDLERS

            $scope.$on(userEvents.videoBecomePublic, function (event, id) {

                for (var i = 0; i < $scope.watches.length; ++i) {
                    var watch = $scope.watches[i];
                    if (watch.id != id) {
                        continue;
                    }

                    watch.isPublic = true;
                    break;
                }

            });

            $scope.$on(userEvents.videoBecomePrivate, function (event, id) {

                for (var i = 0; i < $scope.watches.length; ++i) {
                    var watch = $scope.watches[i];
                    if (watch.id != id) {
                        continue;
                    }

                    watch.isPublic = false;
                    break;
                }

            });



            // INIT

            // loading watches
            filterWatches();

        }
    ]);