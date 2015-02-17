// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.portal", [
        'ngClickberry.video',
        'ngClickberry.user',
        'ngClickberry.account',
        'ngClickberry.trends',
        'ngClickberry.latest',
        'ngClickberry.tag',
        'constants.common',
        'directives.menu',
        'directives.popup',
        'directives.signin',
        'directives.masonry',
        'directives.watchcard',
        'directives.examplecard',
        'directives.loader',
        "directives.connectSocialsNotification",
        'services.auth',
        'services.profile',
        'services.trends',
        'services.watch',
        'services.eventTracking',
        'ui.router',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal', {
                    "abstract": true,
                    templateUrl: 'portal.html',
                    controller: [
                        "$rootScope", "$state", "authEvents", "authService", "profileService",
                        function($rootScope, $state, authEvents, authService, profileService) {


                            // METHODS

                            function loadProfile() {

                                // loading profile info
                                profileService.get().then(function(user) {

                                    // caching user data
                                    $rootScope.currentUser = user;

                                });

                            }

                            function unloadProfile() {

                                $rootScope.currentUser = null;

                            }


// EVENT HANDLERS

                            $rootScope.$on(authEvents.loginSuccess, function() {

                                loadProfile();

                            });

                            $rootScope.$on(authEvents.logoutSuccess, function() {

                                unloadProfile();

                            });


                            // INIT

                            // checking whether we are authenticated and user info does not exist
                            if (!$rootScope.currentUser && authService.isAuthenticated()) {

                                // loading client info
                                loadProfile();
                            }

                        }
                    ]
                })
                .state('portal.home', {
                    url: '/',
                    data: {
                        pageTitle: 'Clickberry Online Storage',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'portal.home.html',
                    controller: 'HomeCtrl'
                });
        }
    ])
// Controllers
    .controller("HomeCtrl", [
        "$scope", "$state", "trendsService", "watchService", "eventTrackingService", "portalEvents",
        function ($scope, $state, trendsService, watchService, eventTrackingService, portalEvents) {


// PROPERTIES

            // trends
            $scope.trends = [];
            $scope.trendsFilter = {
                skip: 0,
                top: 20
            };
            $scope.isTrendsLoading = false;
            $scope.isTrendsAllLoaded = false;

            // latest
            $scope.latest = [];
            $scope.latestFilter = {
                orderBy: 'Created',
                orderByAsc: false,
                userId: '$all',
                state: 'Ready',
                minHitsCount: 3,
                skip: 0,
                top: 20
            };
            $scope.isLatestLoading = false;
            $scope.isLatestAllLoaded = false;

            $scope.currentTab = 0;


// METHODS

            $scope.trackBannerClick = function() {
                eventTrackingService.track(portalEvents.categories.portal, portalEvents.actions.bannerClick, portalEvents.labels.frontPage);
            }

            $scope.selectTab = function(tab) {
                $scope.currentTab = tab;
            }

            $scope.moreTrends = function() {
                $scope.nextTrendsPage();
            }

            $scope.moreLatest = function () {
                $scope.nextLatestPage();
            }

            $scope.nextTrendsPage = function () {

                if ($scope.isTrendsLoading || $scope.isTrendsAllLoaded) {
                    return;
                }

                $scope.trendsFilter.skip += $scope.trendsFilter.top;

                // loading
                filterTrends();
            };

            $scope.nextLatestPage = function () {

                if ($scope.isLatestLoading || $scope.isLatestAllLoaded) {
                    return;
                }

                $scope.latestFilter.skip += $scope.latestFilter.top;

                // loading
                filterLatest();
            };


// PRIVATE METHODS

            function filterTrends() {

                if (!$scope.trendsFilter.skip) {
                    $scope.trends = [];
                }

                $scope.isTrendsLoading = true;

                return trendsService.query($scope.trendsFilter).then(
                    function (data) {

                        $scope.trends = $scope.trends.concat(data);

                        if (data.length < $scope.trendsFilter.top) {
                            $scope.isTrendsAllLoaded = true;
                        } else {
                            $scope.isTrendsAllLoaded = false;
                        }

                        $scope.isTrendsLoading = false;
                    },
                    function() {
                        $scope.isTrendsLoading = false;
                    });
            };

            function filterLatest() {

                if (!$scope.latestFilter.skip) {
                    $scope.latest = [];
                }

                $scope.isLatestLoading = true;

                return watchService.query($scope.latestFilter).then(
                    function (data) {

                        $scope.latest = $scope.latest.concat(data);

                        if (data.length < $scope.latestFilter.top) {
                            $scope.isLatestAllLoaded = true;
                        } else {
                            $scope.isLatestAllLoaded = false;
                        }

                        $scope.isLatestLoading = false;
                    },
                    function () {
                        $scope.isLatestLoading = false;
                    });
            };


// INIT

            filterTrends().then(function() {
                return filterLatest();
            });

        }
    ]);