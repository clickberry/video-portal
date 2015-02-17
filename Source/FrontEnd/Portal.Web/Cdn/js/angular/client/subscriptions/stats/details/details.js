// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.client.subscriptions.stats.details", [
        'ui.router',
        'directives.loader',
        'directives.clients.chart.daily',
        'services.clients.subscriptions.stats.url',
        'services.clients.subscriptions.stats.date',
        'infinite-scroll',
        'filters'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.subscriptions.stats.details', {
                    url: '/subscriptions/{id}/stats/details?url',
                    data: {
                        pageTitle: 'Subscription Url Statistics',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.subscriptions.stats.details.html',
                    controller: 'ClientSubscriptionsStatsDetailsCtrl'
                });
        }
    ])

// Controllers
    .controller("ClientSubscriptionsStatsDetailsCtrl", [
        "$scope", "$stateParams", "$state", "clientSubscriptionUrlStatsService", "clientSubscriptionDateStatsService", "moment", '$filter',
        function ($scope, $stateParams, $state, clientSubscriptionUrlStatsService, clientSubscriptionDateStatsService, moment, $filter) {

            if (!$stateParams.id) {
                // going to subscriptions state
                $state.go("^.^.list");
            }

            var subscriptionId = $stateParams.id;


            if (!$stateParams.url) {
                // going to subscription page
                $state.go("^", { id: subscriptionId });
            }

            var subscriptionUrl = $stateParams.url;

            

            // PROPERTIES


            var dt = new Date();
            var df = new Date().setDate(dt.getDate() - 30);

            $scope.dateRange = { dateFrom: df, dateTo: dt };

            $scope.url = subscriptionUrl;

            $scope.dailyStats = [];

            $scope.stats = [];
            $scope.total = 0;

            $scope.filter = {
                orderBy: 'Date',
                orderByAsc: false,
                skip: 0,
                top: 20,
                inlinecount: 'allpages'
            };

            $scope.filter.dateFrom = $filter('date')(df, 'yyyy-MM-dd');
            $scope.filter.dateTo = $filter('date')(dt, 'yyyy-MM-dd');

            $scope.isAllLoaded = false;

            $scope.isLoading = false;




            // METHODS

            $scope.nextPage = function () {

                if ($scope.isLoading || $scope.isAllLoaded) {
                    return;
                }

                $scope.filter.skip += $scope.filter.top;

                // loading stats
                filterStats();

            };

            $scope.setOrder = function (order) {

                // reseting page
                $scope.filter.skip = 0;
                $scope.isAllLoaded = false;

                if ($scope.filter.orderBy == order) {
                    // just changing direction
                    $scope.filter.orderByAsc = !$scope.filter.orderByAsc;
                } else {
                    $scope.filter.orderBy = order;
                    $scope.filter.orderByAsc = true;
                }

                // reloading stats
                filterStats();
            };

            $scope.setDateRange = function (dateRange) {

                $scope.filter.dateFrom = $filter('date')(dateRange.dateFrom, 'yyyy-MM-dd');
                $scope.filter.dateTo = $filter('date')(dateRange.dateTo, 'yyyy-MM-dd');

                filterDailyStats();
                filterStats();
            }


            // PRIVATE METHODS

            function filterStats() {

                if (!$scope.filter.skip) {
                    $scope.stats = [];
                }

                $scope.isLoading = true;

                clientSubscriptionUrlStatsService.query(subscriptionId, subscriptionUrl, $scope.filter).then(
                    function(data) {

                        $scope.isLoading = false;

                        if ($scope.filter.skip > 0) {
                            $scope.stats = $scope.stats.concat(data.items);
                        } else {
                            $scope.stats = data.items;
                        }

                        $scope.total = data.count;

                        if ($scope.total == $scope.stats.length) {
                            $scope.isAllLoaded = true;
                        } else {
                            $scope.isAllLoaded = false;
                        }

                    },
                    function(reason) {
                        $scope.isLoading = false;
                        throw new Error(reason);
                    });
            };

            function filterDailyStats() {

                clientSubscriptionDateStatsService.query(subscriptionId, subscriptionUrl, $scope.filter, {}).then(
                    function (data) {

                        var points = [];
                        for (var i = 0; i < data.length; i++) {
                            var point = [moment.utc(data[i].date).valueOf(), data[i].count];
                            points.push(point);
                        }

                        $scope.dailyStats = points;

                    },
                    function (reason) {
                        throw new Error(reason);
                    });
            };



            // INIT

            // loading stats
            filterDailyStats();
            filterStats();
        }
    ]);