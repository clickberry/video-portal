// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.watch',
    [
        'ngResource'
    ])
    .factory('watchService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/watch/:id', { id: '@id' });

            function escape(value) {
                return value.replace(/'/g, "''").replace(/#/g, '%23');
            };

            var service = {};
            var cache = {};

            service.get = function(id, options) {

                options = options || {};

                var deferred = $q.defer();

                if (!options.invalidateCache && cache[id]) {

                    deferred.resolve(cache[id]);
                    return deferred.promise;
                }

                // making HTTP request

                resource.get({id: id}, function (data) {

                    // Cache loaded data
                    cache[id] = data;

                    deferred.resolve(data);

                }, function (response) {

                    deferred.reject(response);

                });

                return deferred.promise;

            };

            service.query = function(filter) {

                // build query params
                var params = {};

                // filter
                var filterExp = null;
                var filterParts = [];

                if (filter.userId != null && filter.userId != '') {
                    filterParts.push("UserId eq '" + filter.userId + "'");
                }

                if (filter.name != null && filter.name != '') {
                    var name = filter.name;
                    if (name && name.substr(0, 2) != '"#') {
                        // escaping name
                        name = escape(name);
                    }
                    filterParts.push("Name eq '" + name + "'");
                }

                if (filter.minHitsCount) {
                    filterParts.push("HitsCount ge " + filter.minHitsCount);
                }

                if (filter.state) {
                    filterParts.push("State eq '" + filter.state + "'");
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

                if (sortExp != null) {
                    params['$orderby'] = sortExp;
                }

                // paging
                if (filter.skip != null) {
                    params['$skip'] = filter.skip;
                }

                if (filter.top != null) {
                    params['$top'] = filter.top;
                }

                // clear cache
                cache = {};

                // making HTTP request

                var deferred = $q.defer();
                resource.query(params, function(data) {

                    // Cache loaded data
                    data.forEach(function(elem) {
                        cache[elem.id] = elem;
                    });

                    deferred.resolve(data);

                }, function() {

                    deferred.reject("Could not query watch videos");

                });

                return deferred.promise;

            };



            return service;
        }
    ]);