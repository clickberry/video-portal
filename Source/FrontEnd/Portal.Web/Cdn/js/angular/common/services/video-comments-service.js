// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.video.comments',
    ['ngResource'])
    .factory('videoCommentsService', [
        '$resource', '$q', function($resource, $q) {

            var resource = $resource('/api/projects/:projectId/comments/:commentId', { projectId: '@projectId', commentId: '@commentId' }, {
                update: { method: 'PUT' }
            });


            var service = {};

            service.add = function(projectId, commentModel) {

                var deferred = $q.defer();
                resource.save({ projectId: projectId }, commentModel, function(data) {

                    deferred.resolve(data);

                }, function() {

                    deferred.reject("Could not create comment");

                });

                return deferred.promise;

            };

            service.getAll = function(projectId) {

                var deferred = $q.defer();
                resource.query({ projectId: projectId }, function(comments) {

                    deferred.resolve(comments);

                }, function() {

                    deferred.reject("Could not get project comments");

                });

                return deferred.promise;

            };
            service.update = function(projectId, commentModel) {

                var deferred = $q.defer();
                resource.update({ projectId: projectId, commentId: commentModel.id, comment: commentModel }, function(client) {

                    deferred.resolve(client);

                }, function() {

                    deferred.reject("Failed to update project comment");

                });

                return deferred.promise;

            };
            service.delete = function(projectId, commentId) {

                var deferred = $q.defer();
                resource.delete({ projectId: projectId, commentId: commentId }, function() {

                    deferred.resolve();

                }, function() {

                    deferred.reject("Failed to delete project comment");

                });

                return deferred.promise;

            };
            return service;
        }
    ]);