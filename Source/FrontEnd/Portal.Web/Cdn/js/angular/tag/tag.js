// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.tag", [
        "ngClickberry.tag.video",
        'ui.router',
        'constants.common',
        'directives.masonry',
        'directives.loader',
        'directives.confirm-click',
        'services.watch',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.tag', {
                    url: '/tag/{tag}',
                    data: {
                        pageTitle: 'Videos on Clickberry Online Storage',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'tag.html',
                    controller: 'TagCtrl'
                });
        }
    ])
// Controllers
    .controller("TagCtrl", [
        "$scope", "$state", "$stateParams", "watchService",
        function($scope, $state, $stateParams, watchService) {


// PROPERTIES

            $scope.tagName = $stateParams.tag;

            $scope.watches = [];

            $scope.filter = {
                orderBy: 'Created',
                orderByAsc: false,
                userId: '$all',
                state: 'Ready',
                name: "\"#" + $scope.tagName + "\"",
                skip: 0,
                top: 20
            };

            $scope.isAllLoaded = false;
            $scope.isLoading = false;


            // METHODS

            $scope.nextPage = function() {

                if ($scope.isLoading || $scope.isAllLoaded) {
                    return;
                }

                $scope.filter.skip += $scope.filter.top;

                // loading watches
                filterWatches();
            };


// PRIVATE METHODS

            function filterWatches() {

                if (!$scope.filter.skip) {
                    $scope.watches = [];
                }

                $scope.isLoading = true;
                watchService.query($scope.filter).then(
                    function(data) {

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
                    function() {
                        $scope.isLoading = false;
                    });
            };


            // INIT

            // loading watches
            filterWatches();

        }
    ]);