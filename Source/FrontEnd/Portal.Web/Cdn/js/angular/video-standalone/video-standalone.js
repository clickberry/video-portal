// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.video-standalone", [
        'constants.common',
        'filters',
        'resources.settings',
        'directives.signin',
        'directives.menu',
        'directives.socialshare',
        'directives.loader',
        'directives.copy',
        'services.video.comments',
        'services.views',
        'services.likes',
        'services.dislikes',
        'services.abuse',
        'services.projects',
        'services.watch',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('video', {
                    url: '/video/{id}',
                    data: {
                        pageTitle: 'Video on Clickberry Online Storage',
                        authorizedRoles: [userRoles.all]
                    },
                    controller: 'VideoStandaloneCtrl'
                });
        }
    ])
// Controllers
    .controller("VideoStandaloneCtrl", [
        "$scope", "$rootScope", "$stateParams", "$window", "videoCommentsService", "viewsService", "likesService", "dislikesService", "authEvents", "abuseService", "projectsService", "watchService",
        function ($scope, $rootScope, $stateParams, $window, videoCommentsService, viewsService, likesService, dislikesService, authEvents, abuseService, projectsService, watchService) {


// PROPERTIES

            $scope.projectId = $stateParams.id;

            $scope.comments = [];
            $scope.comment = {
                body: null
            };

            $scope.commentsLoading = false;

            $scope.selectedTab = 0; // comments

            $scope.userId = null;
            $scope.isLiked = false;
            $scope.isDisliked = false;
            $scope.access = 0;

            $scope.likesCount = 0;
            $scope.dislikesCount = 0;


// METHODS

            $scope.showComments = function (event) {
                event.preventDefault();

                loadComments();
                $scope.selectedTab = 0;
            };

            $scope.showAbuse = function (event) {
                event.preventDefault();

                $scope.selectedTab = 1;
            };

            $scope.like = function () {

                if (!$scope.isLiked) {
                    likesService.add($scope.projectId).then(function () {
                        $scope.isLiked = true;
                        $scope.likesCount++;
                    });
                } else {
                    likesService.delete($scope.projectId).then(function () {
                        $scope.isLiked = false;
                        $scope.likesCount--;
                    });
                }
            }

            $scope.dislike = function () {

                if (!$scope.isDisliked) {
                    dislikesService.add($scope.projectId).then(function () {
                        $scope.isDisliked = true;
                        $scope.dislikesCount++;
                    });
                } else {
                    dislikesService.delete($scope.projectId).then(function () {
                        $scope.isDisliked = false;
                        $scope.dislikesCount--;
                    });
                }
            }

            $scope.postComment = function (comment, event) {

                event.preventDefault();

                $scope.commentsLoading = true;

                videoCommentsService.add($scope.projectId, comment).then(function () {
                        $scope.commentsLoading = false;
                        $scope.comment.body = null;
                        loadComments();
                    },
                    function(reason) {
                        $scope.commentsLoading = false;
                        throw new Error(reason);
                    });

            };

            $scope.deleteComment = function (comment) {

                var idx = $scope.comments.indexOf(comment);
                $scope.comments.splice(idx, 1);

                videoCommentsService.delete($stateParams.id, comment.id).then(function () {
                },
                    function (reason) {
                        $scope.comments.splice(idx, 0, comment); // restore element if error occured
                        throw new Error(reason);
                    });
            }

            $scope.reportAbuse = function (event) {

                event.preventDefault();

                if ($scope.isAbuseReporting) {
                    return;
                }

                $scope.isAbuseReporting = true;

                abuseService.add($scope.projectId).then(function () {
                    $scope.isAbuseReporting = false;
                }, function () {
                    $scope.isAbuseReporting = null;
                });
            }

            $scope.onWatchClose = function() {
                goHome();
            }

            $scope.deleteVideo = function () {
                projectsService.delete($scope.projectId).then(function () {
                    goHome();
                });
            };

            $scope.togglePublic = function () {
                $scope.access++;
                if ($scope.access > 2) {
                    $scope.access = 0;
                }

                watchService.get($scope.projectId).then(function (data) {
                    data.access = $scope.access;
                    projectsService.update(data);
                });
            }



            // PRIVATE METHODS

            function loadComments() {

                $scope.commentsLoading = true;

                videoCommentsService.getAll($scope.projectId).then(function (data) {
                        $scope.commentsLoading = false;
                        $scope.comments = data;
                    },
                    function(reason) {
                        $scope.commentsLoading = false;
                        throw new Error(reason);
                    });
            }

            function countView() {

                viewsService.add($scope.projectId);

            }

            function goHome() {
                $window.location.href = $scope.userId ? '/user/' + $scope.userId + '/videos' : '/';
            }


// INIT

            loadComments();


// EVENT HANDLERS

            $rootScope.$on(authEvents.loginSuccess, function () {
                // reloading page
                $window.location.reload();
            });
            
        }
    ]);