// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.admin.clients',
    ['ngResource'])
    .factory('adminClientsService', [
        '$resource', '$q', function($resource, $q) {

            var url = '/api/admin/clients';
            var subscriptionsUrl = '/api/admin/clients/:clientId/subscriptions';

            var clientsCount;
            var clientsResource = $resource(url + '/:id', { id: '@id' }, {
                query: {
                    method: 'GET',
                    isArray: true,
                    "transformResponse": function(data) {
                        var json = JSON.parse(data);
                        clientsCount = json.count;
                        return json.items;
                    }
                },
                update: { method: 'PUT' }
            });

            var clientSubscriptionsResource = $resource(subscriptionsUrl + '/:id', { clientId: '@clientId', id: '@id' }, {
                query: {
                    method: 'GET',
                    isArray: true
                },
                update: { method: 'PUT' }
            });

            var service = {};

            // retrieves all matching users
            service.getAll = function(filter) {

                var params = {};

                // filter
                var filterExp = null;

                var filterParts = [];
                if (filter.name != null) {
                    filterParts.push("Name eq '" + this.escape(filter.name) + "'");
                }

                if (filter.email != null) {
                    filterParts.push("Email eq '" + this.escape(filter.email) + "'");
                }

                if (filter.dateFrom != null) {
                    filterParts.push("Created ge datetime'" + filter.dateFrom + "'");
                }

                if (filter.dateTo != null) {
                    filterParts.push("Created lt datetime'" + filter.dateTo + "'");
                }

                if (filterParts.length > 0) {
                    filterExp = filterParts.join(" and ");
                }

                if (filterExp != null) {
                    params['$filter'] = filterExp;
                }


                // sort
                var sortExp = null;
                if (filter.orderBy != null) {
                    var orderByDirection = filter.orderByAsc ? 'asc' : 'desc';
                    sortExp = filter.orderBy + ' ' + orderByDirection;
                }

                // sort
                if (sortExp != null) {
                    params['$orderby'] = sortExp;
                }

                // count
                if (filter.inlinecount != null) {
                    params['$inlinecount'] = filter.inlinecount;
                }

                // paging
                if (filter.skip != null) {
                    params['$skip'] = filter.skip;
                }

                if (filter.top != null) {
                    params['$top'] = filter.top;
                }


                // making HTTP request

                var deferred = $q.defer();
                var clientsPage = clientsResource.query(params, function() {

                    var result = { count: clientsCount, items: clientsPage };
                    deferred.resolve(result);

                }, function() {

                    deferred.reject("Could not load clients");

                });

                return deferred.promise;

            };

            service.getCsvUrl = function (filter) {

                var params = {};

                // filter
                var filterExp = null;

                var filterParts = [];
                if (filter.name != null) {
                    filterParts.push("Name eq '" + this.escape(filter.name) + "'");
                }

                if (filter.email != null) {
                    filterParts.push("Email eq '" + this.escape(filter.email) + "'");
                }

                if (filter.dateFrom != null) {
                    filterParts.push("Created ge datetime'" + filter.dateFrom + "'");
                }

                if (filter.dateTo != null) {
                    filterParts.push("Created lt datetime'" + filter.dateTo + "'");
                }

                if (filterParts.length > 0) {
                    filterExp = filterParts.join(" and ");
                }

                if (filterExp != null) {
                    params['$filter'] = filterExp;
                }


                // sort
                var sortExp = null;
                if (filter.orderBy != null) {
                    var orderByDirection = filter.orderByAsc ? 'asc' : 'desc';
                    sortExp = filter.orderBy + ' ' + orderByDirection;
                }

                // sort
                if (sortExp != null) {
                    params['$orderby'] = sortExp;
                }

                // format
                params['$format'] = "csv";


                // making HTTP request

                var deferred = $q.defer();
                clientsResource.get(params, function (d, getResponseHeaders) {

                    var location = getResponseHeaders("Location");
                    deferred.resolve(location);

                }, function () {

                    deferred.reject("Faild to export clients to CSV");

                });

                return deferred.promise;

            };

            service.get = function(id) {

                // making HTTP request

                var deferred = $q.defer();
                var client = clientsResource.get({ id: id }, function() {

                    deferred.resolve(client);

                }, function() {

                    deferred.reject("Could not load client");

                });

                return deferred.promise;
            };

            service.getSubscriptions = function(clientId) {

                var params = { clientId: clientId };

                // making HTTP request

                var deferred = $q.defer();
                var subscriptionsPage = clientSubscriptionsResource.query(params, function() {

                    deferred.resolve(subscriptionsPage);

                }, function() {

                    deferred.reject("Could not load client subscriptions");

                });

                return deferred.promise;

            };

            service.escape = function(value) {
                return value.replace(/'/g, "''").replace(/#/g, '%23');
            };

            return service;
        }
    ]);