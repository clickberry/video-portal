// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.admin.projects", [
        'services.admin.projects',
        'services.admin.users',
        'resources.settings',
        'resources.common',
        'resources.context',
        'filters',
        'ui.router'
    ])

    // Routes
    .config([
        "$stateProvider", function($stateProvider) {
            $stateProvider
                .state('admin.videos', {
                    "abstract": true,
                    templateUrl: 'admin.videos.html'
                })
                .state('admin.videos.list', {
                    "abstract": true,
                    templateUrl: 'admin.videos.list.layout.html',
                    controller: [
                        "$scope", "settings", function($scope, settings) {

                            // shared with child views
                            var pageSize = settings.get('pageSize');
                            $scope.filter = {
                                name: null,
                                productType: null,
                                userName: null,
                                dateFrom: null,
                                dateTo: null,
                                orderBy: 'Created',
                                orderByAsc: false,
                                skip: 0,
                                top: pageSize,
                                inlinecount: 'allpages'
                            };
                        }
                    ]
                })
                .state('admin.videos.list.table', {
                    url: '/videos?name&userName&productType&dateFrom&dateTo&orderBy&orderByAsc&skip&filterType',
                    data: { pageTitle: 'Videos' },
                    views: {
                        'header': {
                            templateUrl: 'admin.videos.list.header.html'
                        },
                        'filters': {
                            controller: 'AdminVideosListFiltersCtrl',
                            templateUrl: 'admin.videos.list.filters.html'
                        },
                        'table': {
                            controller: 'AdminVideosListTableCtrl',
                            templateUrl: 'admin.videos.list.table.html'
                        }
                    }
                });
        }
    ])

    // Controllers
    .controller("AdminVideosListFiltersCtrl", [
        "$scope", "$window", "$interval", "$http", "$filter", "adminProjectsService", "resources", "$stateParams", "$state",
        function ($scope, $window, $interval, $http, $filter, adminProjectsService, resources, $stateParams, $state) {

            var intervalId;



            // PROPERTIES

            $scope.filterTypes = { title: 0, author: 1, product: 2, date: 3 };

            $scope.filterTypeOptions = [
                { name: "Title", value: $scope.filterTypes.title },
                { name: "Author", value: $scope.filterTypes.author },
                { name: "Product", value: $scope.filterTypes.product },
                { name: "Date", value: $scope.filterTypes.date }
            ];

            $scope.dateFromOptions = {
                minDate: null,
                maxDate: null,
                dateFormat: "dd-mm-yy"
            };

            $scope.dateToOptions = {
                minDate: null,
                maxDate: null,
                dateFormat: "dd-mm-yy"
            };

            $scope.dateFromStr = $stateParams.filterType == $scope.filterTypes.date && $stateParams.dateFrom ? moment($stateParams.dateFrom, "YYYY-MM-DD").toDate() : null;
            $scope.$watch('dateFromStr', function(newValue) {

                var isoDate = null;
                if (newValue) {
                    var newDate = moment(new Date(newValue.getFullYear(), newValue.getMonth(), newValue.getDate(), 0, 0, 0));
                    $scope.dateToOptions.minDate = newDate.format("DD-MM-YYYY");
                    isoDate = newDate.format("YYYY-MM-DD");
                }

                $scope.filter.dateFrom = isoDate;

            });

            // we add -1 day because our query string contains max date value for strict inequality 'date lt datetime', so we need normalize date for date picker
            $scope.dateToStr = $stateParams.filterType == $scope.filterTypes.date && $stateParams.dateTo ? moment($stateParams.dateTo, "YYYY-MM-DD").add('days', -1).toDate() : null;
            $scope.$watch('dateToStr', function(newValue) {

                var isoDate = null;
                if (newValue) {
                    var newDate = moment(new Date(newValue.getFullYear(), newValue.getMonth(), newValue.getDate(), 0, 0, 0));
                    $scope.dateFromOptions.maxDate = newDate.format("DD-MM-YYYY");
                    isoDate = newDate.add('days', 1).format("YYYY-MM-DD"); // we add +1 day because we use 'date lt datetime' for max date value
                }

                $scope.filter.dateTo = isoDate;

            });

            $scope.filterType = $stateParams.filterType ? $scope.filterTypeOptions[$stateParams.filterType] : $scope.filterTypeOptions[0];
            $scope.$watch('filterType', function (value) {

                if ($scope.filter.filterType == value.value) {
                    return;
                }

                // reseting form filters
                $scope.filter.name = null;
                $scope.filter.productType = null;
                $scope.filter.userName = null;
                $scope.filter.dateFrom = null;
                $scope.filter.dateTo = null;

                $scope.filter.filterType = value.value;

            });

            $scope.productTypeOptions = resources.products;

            $scope.isCsvProcessing = false;




            // METHODS

            $scope.downloadCsv = function() {

                if ($scope.isCsvProcessing) {
                    return;
                }

                $scope.isCsvProcessing = true;

                adminProjectsService.getCsvUrl($scope.filter).then(function(uri) {
                
                    // making uri scheme-agnostic
                    var schemalessUri = $filter('schemeAgnostic')(uri);

                    // checking csv periodically to complete
                    intervalId = $interval(function () {

                        $http.get(schemalessUri).then(function () {

                            // ready
                            $scope.isCsvProcessing = false;
                            $interval.cancel(intervalId);

                            // navigating to download
                            $window.location.href = schemalessUri;

                        }, function() {

                            // not yet ready

                        });

                    }, 10000);

                }, function() {
                    $scope.isCsvProcessing = false;
                    throw new Error("Failed to generate CSV.");
                });

            };

            $scope.submitFilter = function() {

                // reseting page
                $scope.filter.skip = 0;

                // reloading page with new params
                $state.go($state.current, $scope.filter, { reload: true });
            };

            function initFilter() {

                $scope.filter.name = $stateParams.name;
                $scope.filter.productType = $stateParams.productType ? parseInt($stateParams.productType) : null;
                $scope.filter.userName = $stateParams.userName;
                $scope.filter.dateFrom = $stateParams.dateFrom;
                $scope.filter.dateTo = $stateParams.dateTo;

                $scope.filter.orderBy = $stateParams.orderBy ? $stateParams.orderBy : $scope.filter.orderBy;
                $scope.filter.orderByAsc = $stateParams.orderByAsc ? $stateParams.orderByAsc.toLowerCase() == "true" : $scope.filter.orderByAsc;
                $scope.filter.skip = $stateParams.skip ? parseInt($stateParams.skip) : 0;
                $scope.filter.filterType = $stateParams.filterType;

            };



            // EVENT HANDLERS

            // destructor
            $scope.$on('$destroy', function () {
                $interval.cancel(intervalId);
            });



            // INIT

            initFilter();

        }
    ])
    .controller("AdminVideosListTableCtrl", [
        "$scope", "$state", "adminProjectsService", "context", "settings",
        function ($scope, $state, adminProjectsService, context, settings) {


            // PROPERTIES

            $scope.videos = [];
            $scope.videosCount = 0;

            $scope.videoUrl = settings.get('videoUrl');

            $scope.userVideosUrl = settings.get('userVideosUrl');

            $scope.isLoading = false;

            $scope.isAllLoaded = false;



            // METHODS

            $scope.setOrder = function(order) {

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

                // reload state
                $state.go($state.current, $scope.filter, { reload: true });
            };

            $scope.nextPage = function() {
                $scope.filter.skip += $scope.filter.top;

                // reload state
                $state.go($state.current, $scope.filter, { reload: true });
            };

            $scope.prevPage = function() {
                $scope.filter.skip -= $scope.filter.top;
                if ($scope.filter.skip < 0) {
                    $scope.filter.skip = 0;
                }

                // reload state
                $state.go($state.current, $scope.filter, { reload: true });
            };


            $scope.gotoUserVideos = function(userId, userName) {

                var user = { id: userId, userName: userName };
                context.user = user;

                $state.go('admin.users.details.table', { id: user.id });

            };

            $scope.delete = function(project) {

                $scope.isLoading = true;

                project.$delete(function() {
                    $scope.isLoading = false;
                    filterVideos();
                }, function() {
                    $scope.isLoading = false;
                    throw new Error("Could not delete project " + project.id);
                });
            };

            

            // PRIVATE METHODS

            function filterVideos() {

                $scope.isLoading = true;
                $scope.videos = [];

                adminProjectsService.getAll($scope.filter).then(function(data) {

                    var videos = data.items;

                    if (!videos.length || videos.length < $scope.filter.top) {
                        $scope.isAllLoaded = true;
                    } else {
                        $scope.isAllLoaded = false;
                    }

                    $scope.videos = videos;
                    $scope.videosCount = data.count;
                    $scope.isLoading = false;

                }, function(reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });
            };



            // INIT

            filterVideos();

        }
    ]);