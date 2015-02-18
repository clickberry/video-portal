// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.admin.clients", [
        'services.admin.clients',
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
                .state('admin.clients', {
                    "abstract": true,
                    templateUrl: 'admin.clients.html'
                })
                .state('admin.clients.list', {
                    "abstract": true,
                    templateUrl: 'admin.clients.list.layout.html',
                    controller: [
                        "$scope", "settings", function($scope, settings) {

                            // shared with child views
                            var pageSize = settings.get('pageSize');
                            $scope.filter = {
                                name: null,
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
                .state('admin.clients.list.table', {
                    url: '/clients',
                    data: { pageTitle: 'Clients' },
                    views: {
                        'header': {
                            templateUrl: 'admin.clients.list.header.html'
                        },
                        'filters': {
                            controller: 'AdminClientsListFiltersCtrl',
                            templateUrl: 'admin.clients.list.filters.html'
                        },
                        'table': {
                            controller: 'AdminClientsListTableCtrl',
                            templateUrl: 'admin.clients.list.table.html'
                        }
                    }
                })
                .state('admin.clients.subscriptions', {
                    "abstract": true,
                    templateUrl: 'admin.clients.subscriptions.layout.html',
                    controller: [
                        "$scope", "settings", function($scope, settings) {

                            // shared with child views
                            var pageSize = settings.get('pageSize');
                            $scope.filter = {
                                orderBy: 'Created',
                                orderByAsc: false,
                                skip: 0,
                                top: pageSize,
                                inlinecount: 'allpages'
                            };
                        }
                    ]
                })
                .state('admin.clients.subscriptions.table', {
                    url: '/clients/{id}',
                    data: { pageTitle: 'Subscriptions' },

                    views: {
                        'header': {
                            controller: 'AdminClientSubscriptionsHeaderCtrl',
                            templateUrl: 'admin.clients.subscriptions.header.html'
                        },
                        'filters': {
                            controller: 'AdminClientSubscriptionsFiltersCtrl',
                            templateUrl: 'admin.clients.subscriptions.filters.html'
                        },
                        'table': {
                            controller: 'AdminClientSubscriptionsTableCtrl',
                            templateUrl: 'admin.clients.subscriptions.table.html'
                        }
                    }
                });
        }
    ])

    // Controllers
    .controller("AdminClientsListFiltersCtrl", [
        "$scope", "$window", "$interval", "$http", "$filter", "adminClientsService",
        function ($scope, $window, $interval, $http, $filter, adminClientsService) {

            var intervalId;


            // PROPERTIES

            $scope.filterTypes = { name: 0, email: 1, date: 2 };

            $scope.filterTypeOptions = [
                { name: "Name", value: $scope.filterTypes.name },
                { name: "Email", value: $scope.filterTypes.email },
                { name: "Date", value: $scope.filterTypes.date }
            ];

            $scope.formFilter = {
                name: null,
                email: null,
                dateFrom: null,
                dateTo: null
            };

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

            $scope.dateFromStr = null;
            $scope.$watch('dateFromStr', function (newValue) {

                var isoDate = null;
                if (newValue) {
                    var newDate = moment(newValue);
                    $scope.dateToOptions.minDate = newDate.format("DD-MM-YYYY");
                    isoDate = newDate.format("YYYY-MM-DD");
                }

                $scope.formFilter.dateFrom = isoDate;

            });

            $scope.dateToStr = null;
            $scope.$watch('dateToStr', function (newValue) {

                var isoDate = null;
                if (newValue) {
                    var newDate = moment(newValue);
                    $scope.dateFromOptions.maxDate = newDate.format("DD-MM-YYYY");
                    isoDate = newDate.add('days', 1).format("YYYY-MM-DD");
                }

                $scope.formFilter.dateTo = isoDate;

            });

            $scope.filterType = $scope.filterTypeOptions[0];
            $scope.$watch('filterType', function () {

                // clearing form filters
                for (var attr in $scope.formFilter) {
                    $scope.formFilter[attr] = null;
                }

            });


            // METHODS

            $scope.downloadCsv = function () {

                if ($scope.isCsvProcessing) {
                    return;
                }

                $scope.isCsvProcessing = true;

                adminClientsService.getCsvUrl($scope.filter).then(function (uri) {

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

            $scope.submitFilter = function () {

                // reseting page
                $scope.filter.skip = 0;

                for (var attr in $scope.formFilter) {
                    if ($scope.filter[attr] == $scope.formFilter[attr]) {
                        continue;
                    }
                    $scope.filter[attr] = $scope.formFilter[attr];
                }
            };

        }
    ])

    // Controllers
    .controller("AdminClientsListTableCtrl", [
        "$scope", "$state", "adminClientsService", "settings", "context",
        function($scope, $state, adminClientsService, settings, context) {

            // PROPERTIES

            $scope.clients = [];
            $scope.clientsCount = 0;

            $scope.$watch('filter', function() {

                // reloading if filter changed
                filterClients();

            }, true);

            $scope.resourceStates = settings.get('resourceStates');

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

            $scope.gotoClientSubscriptions = function(client) {

                context.client = client;
                $state.go('^.^.subscriptions.table', { id: client.id });

            };

            $scope.setState = function(client) {

                $scope.isLoading = true;

                client.$update(function() {
                    $scope.isLoading = false;
                }, function() {
                    $scope.isLoading = false;
                    throw new Error("Could not change client state " + client.id);
                });

            };

            $scope.getStateName = function(state) {
                return $scope.resourceStates.filter(function(item) {
                    return item.value == state;
                })[0].name;
            };


            // PRIVATE METHODS

            function filterClients() {

                $scope.isLoading = true;
                $scope.clients = [];

                adminClientsService.getAll($scope.filter).then(function(data) {

                    var clients = data.items;

                    if (!clients.length || clients.length < $scope.filter.top) {
                        $scope.isAllLoaded = true;
                    } else {
                        $scope.isAllLoaded = false;
                    }

                    $scope.clients = clients;
                    $scope.clientsCount = data.count;
                    $scope.isLoading = false;

                }, function(reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });
            };

        }
    ])
    .controller("AdminClientSubscriptionsHeaderCtrl", [
        "$scope", "$stateParams", "adminClientsService", "context",
        function($scope, $stateParams, adminClientsService, context) {

            // PROPERTIES

            $scope.client = context.client;
            if ($scope.client == null) {

                var clientId = $stateParams.id;
                $scope.client = { id: clientId };

                // loading current client
                adminClientsService.get(clientId).then(function(data) {

                    $scope.client = data;

                    // caching current client
                    context.client = data;

                }, function(reason) {
                    throw new Error(reason);
                });
            }

        }
    ])
    .controller("AdminClientSubscriptionsFiltersCtrl", [
        function() {}
    ])
    .controller("AdminClientSubscriptionsTableCtrl", [
        "$scope", "$stateParams", "adminClientsService", "context", "settings",
        function($scope, $stateParams, adminClientsService, context, settings) {

            // PROPERTIES

            $scope.subscriptions = [];

            $scope.resourceStates = settings.get('resourceStates');
            $scope.subscriptionTypes = {
                0: "Free",
                1: "Basic",
                2: "Pro",
                3: "Custom"
            };

            $scope.$watch('filter', function() {

                // reloading if filter changed
                filterSubscriptions();

            }, true);

            $scope.isLoading = false;

            // METHODS

            function filterSubscriptions() {

                $scope.isLoading = true;
                $scope.subscriptions = [];

                adminClientsService.getSubscriptions($stateParams.id).then(function(data) {

                    var subscriptions = data;

                    $scope.subscriptions = subscriptions;
                    $scope.isLoading = false;

                }, function(reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });
            };

            $scope.setState = function (subscription) {

                $scope.isLoading = true;

                subscription.$update({clientId: context.client.id}, function () {
                    $scope.isLoading = false;
                }, function () {
                    $scope.isLoading = false;
                    throw new Error("Could not change client subscription " + subscription.id);
                });

            };
        }
    ]);