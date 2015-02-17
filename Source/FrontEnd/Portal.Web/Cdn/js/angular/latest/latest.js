// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.latest", [
        "ngClickberry.latest.video",
        'ui.router',
        'directives.masonry',
        'directives.loader',
        'services.watch',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('portal.latest', {
                    url: '/latest',
                    data: {
                        pageTitle: 'Latest on Clickberry',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'latest.html',
                    controller: 'LatestCtrl'
                });
        }
    ])
// Controllers
    .controller("LatestCtrl", [
        "$scope", "watchService",
        function ($scope, watchService) {


            // PROPERTIES

            $scope.watches = [];

            $scope.filter = {
                orderBy: 'Created',
                orderByAsc: false,
                userId: '$all',
                state: 'Ready',
                minHitsCount: 3,
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

                // loading latest
                filterLatest();
            };


            // PRIVATE METHODS

            function filterLatest() {

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

                        // fixing trends version
                        if ($scope.watches.length > 0) {
                            $scope.filter.version = $scope.watches[0].version;
                        }

                        $scope.isLoading = false;

                    },
                    function () {
                        $scope.isLoading = false;
                    });
            };


            // INIT

            // loading latest
            filterLatest();

        }
    ]);