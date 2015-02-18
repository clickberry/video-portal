// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.admin.users',
    ['ngResource'])
    .factory('adminUsersService', [
        '$resource', '$q', function($resource, $q) {

            var url = '/api/admin/users';
            var projectsUrl = '/api/admin/users/:id/projects';

            var usersCount;
            var projectsCount;
            var usersResource = $resource(url + '/:id', { id: '@id' }, {
                query: {
                    method: 'GET',
                    isArray: true,
                    "transformResponse": function(data) {
                        var json = JSON.parse(data);
                        usersCount = json.count;
                        return json.items;
                    }
                },
                update: { method: 'PUT' }
            });

            var userProjectsResource = $resource(projectsUrl, { id: '@id' }, {
                query: {
                    method: 'GET',
                    isArray: true,
                    "transformResponse": function(data) {
                        var json = JSON.parse(data);
                        projectsCount = json.count;
                        return json.items;
                    }
                }
            });

            var service = {};

            // retrieves all matching users
            service.getAll = function(filter) {

                var params = {};

                // filter
                var filterExp = null;

                var filterParts = [];
                if (filter.userName != null) {
                    filterParts.push("UserName eq '" + this.escape(filter.userName) + "'");
                }
                if (filter.email != null) {
                    filterParts.push("Email eq '" + filter.email + "'");
                }
                if (filter.productType != null) {
                    filterParts.push("ProductType eq '" + filter.productType + "'");
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
                var usersPage = usersResource.query(params, function() {

                    var result = { count: usersCount, items: usersPage };
                    deferred.resolve(result);

                }, function() {

                    deferred.reject("Could not load users");

                });

                return deferred.promise;

            };

            service.getCsvUrl = function(filter) {

                var params = {};

                // filter
                var filterExp = null;

                var filterParts = [];
                if (filter.userName != null) {
                    filterParts.push("UserName eq '" + this.escape(filter.userName) + "'");
                }
                if (filter.email != null) {
                    filterParts.push("Email eq '" + filter.email + "'");
                }
                if (filter.productType != null) {
                    filterParts.push("ProductType eq '" + filter.productType + "'");
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
                usersResource.get(params, function (d, getResponseHeaders) {

                    var location = getResponseHeaders("Location");
                    deferred.resolve(location);

                }, function () {

                    deferred.reject("Faild to export users to CSV");

                });

                return deferred.promise;
            };

            service.get = function(id) {

                // making HTTP request

                var deferred = $q.defer();
                var user = usersResource.get({ id: id }, function() {

                    deferred.resolve(user);

                }, function() {

                    deferred.reject("Could not load user");

                });

                return deferred.promise;
            };

            service.getProjects = function(id, filter) {

                var params = { id: id };

                // filter
                var filterExp = null;

                if (filter.name != null) {
                    filterExp = "Name eq '" + this.escape(filter.name) + "'";
                }

                if (filter.productType != null) {
                    filterExp = "ProductType eq '" + filter.productType + "'";
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
                var projectsPage = userProjectsResource.query(params, function() {

                    var result = { count: projectsCount, items: projectsPage };
                    deferred.resolve(result);

                }, function() {

                    deferred.reject("Could not load user projects");

                });

                return deferred.promise;

            };

            service.escape = function(value) {
                return value.replace(/'/g, "''").replace(/#/g, '%23');
            };

            return service;
        }
    ]);