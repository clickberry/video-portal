// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.trends", [
        "ngClickberry.trends.video",
        'ui.router',
        'directives.masonry',
        'directives.loader',
        'services.trends',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('portal.trends', {
                    url: '/trends',
                    data: {
                        pageTitle: 'Clickberry Trends',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'trends.html',
                    controller: 'TrendsCtrl'
                });
        }
    ])
// Controllers
    .controller("TrendsCtrl", [
        "$scope", "trendsService",
        function ($scope, trendsService) {


            // PROPERTIES

            $scope.watches = [];

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

                // loading trends
                filterTrends();
            };


            // PRIVATE METHODS

            function filterTrends() {

                if (!$scope.filter.skip) {
                    $scope.watches = [];
                }

                $scope.isLoading = true;

                trendsService.query($scope.filter).then(
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

            // loading trends
            filterTrends();

        }
    ]);