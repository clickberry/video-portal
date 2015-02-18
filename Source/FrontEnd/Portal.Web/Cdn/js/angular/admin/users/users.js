// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.admin.users", [
        'services.admin.users',
        'services.admin.projects',
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
                .state('admin.users', {
                    "abstract": true,
                    templateUrl: 'admin.users.html'
                })
                .state('admin.users.list', {
                    "abstract": true,
                    templateUrl: 'admin.users.list.layout.html',
                    controller: [
                        "$scope", "settings", function($scope, settings) {

                            // shared with child views
                            var pageSize = settings.get('pageSize');
                            $scope.filter = {
                                userName: null,
                                email: null,
                                productType: null,
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
                .state('admin.users.list.table', {
                    url: '/users?userName&email&productType&dateFrom&dateTo&orderBy&orderByAsc&skip&filterType',
                    data: { pageTitle: 'Users' },
                    views: {
                        'header': {
                            templateUrl: 'admin.users.list.header.html'
                        },
                        'filters': {
                            controller: 'AdminUsersListFiltersCtrl',
                            templateUrl: 'admin.users.list.filters.html'
                        },
                        'table': {
                            controller: 'AdminUsersListTableCtrl',
                            templateUrl: 'admin.users.list.table.html'
                        }
                    }
                })
                .state('admin.users.details', {
                    "abstract": true,
                    templateUrl: 'admin.users.details.layout.html',
                    controller: [
                        "$scope", "settings", function($scope, settings) {

                            // shared with child views
                            var pageSize = settings.get('pageSize');
                            $scope.filter = {
                                userName: null,
                                email: null,
                                productType: null,
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
                .state('admin.users.details.table', {
                    url: '/admin/users/{id}',
                    data: { pageTitle: 'User Videos' },

                    views: {
                        'header': {
                            controller: 'AdminUserDetailsHeaderCtrl',
                            templateUrl: 'admin.users.details.header.html'
                        },
                        'filters': {
                            controller: 'AdminUserDetailsFiltersCtrl',
                            templateUrl: 'admin.users.details.filters.html'
                        },
                        'table': {
                            controller: 'AdminUserDetailsTableCtrl',
                            templateUrl: 'admin.users.details.table.html'
                        }
                    }
                })
                .state('admin.users.changepass', {
                    url: '/users/{id}/changepass',
                    controller: 'AdminUserChangePasswordCtrl',
                    templateUrl: 'admin.user.change-password.html',
                    data: { pageTitle: 'Change User Password' }
                });
        }
    ])

    // Controllers
    .controller("AdminUsersListFiltersCtrl", [
        "$scope", "$window", "$interval", "$http", "$filter", "adminUsersService", "resources", "$stateParams", "$state",
        function ($scope, $window, $interval, $http, $filter, adminUsersService, resources, $stateParams, $state) {

            var intervalId;


            // PROPERTIES

            $scope.filterTypes = { name: 0, email: 1, product: 2, date: 3 };

            $scope.filterTypeOptions = [
                { name: "Name", value: $scope.filterTypes.name },
                { name: "Email", value: $scope.filterTypes.email },
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
            $scope.$watch('filterType', function(value) {

                if ($scope.filter.filterType == value.value) {
                    return;
                }

                // reseting form filters
                $scope.filter.userName = null;
                $scope.filter.email = null;
                $scope.filter.productType = null;
                $scope.filter.dateFrom = null;
                $scope.filter.dateTo = null;

                $scope.filter.filterType = value.value;

            });

            $scope.productTypeOptions = resources.products;

            $scope.isCsvProcessing = false;




            // METHODS

            $scope.downloadCsv = function () {

                if ($scope.isCsvProcessing) {
                    return;
                }

                $scope.isCsvProcessing = true;

                adminUsersService.getCsvUrl($scope.filter).then(function (uri) {

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

                        }, function () {

                            // not yet ready

                        });

                    }, 10000);

                }, function () {
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

                $scope.filter.userName = $stateParams.userName;
                $scope.filter.email = $stateParams.email;
                $scope.filter.productType = $stateParams.productType ? parseInt($stateParams.productType) : null;
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
    .controller("AdminUsersListTableCtrl", [
        "$scope", "$state", "adminUsersService", "settings", "context",
        function($scope, $state, adminUsersService, settings, context) {

            // PROPERTIES

            $scope.users = [];
            $scope.usersCount = 0;

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

            $scope.gotoUserVideos = function(user) {

                context.user = user;
                $state.go('^.^.details.table', { id: user.id });

            };

            $scope.delete = function(user) {

                $scope.isLoading = true;

                user.$delete(function() {
                    $scope.isLoading = false;
                    filterUsers();
                }, function() {
                    $scope.isLoading = false;
                    throw new Error("Could not delete user " + user.id);
                });

            };

            $scope.changePass = function(user) {

                context.user = user;
                $state.go('admin.users.changepass', { id: user.id });

            };


            // PRIVATE METHODS

            function filterUsers() {

                $scope.isLoading = true;
                $scope.users = [];

                adminUsersService.getAll($scope.filter).then(function(data) {

                    var users = data.items;

                    if (!users.length || users.length < $scope.filter.top) {
                        $scope.isAllLoaded = true;
                    } else {
                        $scope.isAllLoaded = false;
                    }

                    $scope.users = users;
                    $scope.usersCount = data.count;
                    $scope.isLoading = false;

                }, function(reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });
            };


            // INIT

            filterUsers();

        }
    ])
    .controller("AdminUserDetailsHeaderCtrl", [
        "$scope", "$stateParams", "adminUsersService", "context",
        function($scope, $stateParams, adminUsersService, context) {

            // PROPERTIES

            $scope.user = context.user;
            if ($scope.user == null) {

                var userId = $stateParams.id;
                $scope.user = { id: userId };

                // loading current user
                adminUsersService.get(userId).then(function(data) {

                    $scope.user = data;

                    // caching current user
                    context.user = data;

                }, function(reason) {
                    throw new Error(reason);
                });
            }

        }
    ])
    .controller("AdminUserDetailsFiltersCtrl", [
        "$scope",
        function($scope) {

            // PROPERTIES

            $scope.videoName = null;
            $scope.$watch('videoName', function() {

                // reseting page
                $scope.filter.skip = 0;
                $scope.isAllLoaded = false;

                $scope.filter.name = $scope.videoName;

            });

        }
    ])
    .controller("AdminUserDetailsTableCtrl", [
        "$scope", "$stateParams", "adminUsersService", "adminProjectsService", "settings",
        function($scope, $stateParams, adminUsersService, adminProjectsService, settings) {

            // PROPERTIES

            $scope.videos = [];
            $scope.videosCount = 0;

            $scope.$watch('filter', function() {

                // reloading if filter changed
                filterVideos();

            }, true);


            $scope.videoUrl = settings.get('videoUrl');

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
            };

            $scope.nextPage = function() {

                $scope.filter.skip += $scope.filter.top;

            };

            $scope.prevPage = function() {

                $scope.filter.skip -= $scope.filter.top;
                if ($scope.filter.skip < 0) {
                    $scope.filter.skip = 0;
                }

                $scope.isAllLoaded = false;
            };

            $scope.delete = function(project) {

                $scope.isLoading = true;

                adminProjectsService.delete(project).then(function() {
                    $scope.isLoading = false;
                    filterVideos();
                }, function() {
                    $scope.isLoading = false;
                    throw new Error("Could not delete project " + project.id);
                });
            };

            function filterVideos() {

                $scope.isLoading = true;
                $scope.videos = [];

                adminUsersService.getProjects($stateParams.id, $scope.filter).then(function(data) {

                    var videos = data.items;

                    if (!videos.length) {
                        $scope.isAllLoaded = true;
                    } else if (videos.length < $scope.filter.top) {
                        $scope.isAllLoaded = true;
                    }

                    $scope.videos = videos;
                    $scope.videosCount = data.count;
                    $scope.isLoading = false;

                }, function(reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });
            };

        }
    ])
    .controller("AdminUserChangePasswordCtrl",
    [
        "$scope", "$state", "$stateParams", "adminUsersService", "context",
        function($scope, $state, $stateParams, adminUsersService, context) {


            // PROPERTIES

            $scope.user = context.user;
            if ($scope.user == null) {

                var userId = $stateParams.id;
                $scope.user = { id: userId };

                // loading current user
                adminUsersService.get(userId).then(function(data) {

                    $scope.user = data;

                    // caching current user
                    context.user = data;

                }, function(reason) {
                    throw new Error(reason);
                });
            }


            // METHODS

            $scope.submitChangePass = function(form) {

                if (!form.$valid) {
                    return;
                }

                $scope.isLoading = true;

                var user = $scope.user;

                user.$update(function() {
                    $scope.isLoading = false;

                    // redirecting to user list
                    $state.go('^.list.table');

                }, function() {
                    $scope.isLoading = false;
                    throw new Error("Could not update user " + user.id);
                });

            };

        }
    ]);