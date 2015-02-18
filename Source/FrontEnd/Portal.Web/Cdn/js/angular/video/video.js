// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.video", [
        'constants.common',
        'directives.popup',
        'directives.player',
        'directives.socialshare',
        'directives.loader',
        'directives.copy',
        'directives.confirm-click',
        'services.watch',
        'services.video.comments',
        'services.views',
        'services.likes',
        'services.dislikes',
        'services.abuse',
        'services.projects',
        'services.eventTracking',
        'filters',
        'ui.router',
        'perfect_scrollbar'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.home.video', {
                    url: 'video/{id}',
                    data: {
                        pageTitle: 'Video on Clickberry Online Storage',
                        authorizedRoles: [userRoles.all]
                    },
                    views: {
                        'video': {
                            controller: 'VideoCtrl',
                            templateUrl: 'portal.video.html'
                        }
                    }
                });
        }
    ])
// Controllers
    .controller("VideoCtrl", [
        "$scope", "$rootScope", "$state", "$stateParams", "watchService", "projectsService", "videoCommentsService", "viewsService", "likesService", "dislikesService", "userEvents", "abuseService", "eventTrackingService", "portalEvents", "authEvents", "toastr",
        function ($scope, $rootScope, $state, $stateParams, watchService, projectsService, videoCommentsService, viewsService, likesService, dislikesService, userEvents, abuseService, eventTrackingService, portalEvents, authEvents, toastr) {

            if (!$stateParams.id) {
                $state.go('^');
                return;
            }


// PROPERTIES

            $scope.watch = null;

            $scope.screenshotUrl = null;
            $scope.$watch("watch", function (value) {
                if (value) {
                    $scope.screenshotUrl = value.screenshotUrl;
                    if ($scope.screenshotUrl == null && value.screenshots.length > 0) {
                        $scope.screenshotUrl = value.screenshots[0].uri;
                    }
                } else {
                    $scope.screenshotUrl = null;
                }
            });

            $scope.isLoading = false;
            $scope.showWatch = true;

            $scope.comments = [];
            $scope.comment = {
                body: null
            };

            $scope.commentsLoading = false;
            $scope.commentPosting = false;

            $scope.selectedTab = 0; // comments

            $scope.isTitleEditing = false;
            $scope.isDescriptionEditing = false;


// METHODS

            $scope.editTitle = function() {
                $scope.isTitleEditing = true;
            }

            $scope.saveTitle = function (project) {
                $scope.isTitleEditing = false;
                projectsService.update(project);
            }

            $scope.editDescription = function () {
                $scope.isDescriptionEditing = true;
            }

            $scope.saveDescription = function (project) {
                $scope.isDescriptionEditing = false;
                projectsService.update(project);
            }

            $scope.trackBannerClick = function () {
                eventTrackingService.track(portalEvents.categories.portal, portalEvents.actions.bannerClick, portalEvents.labels.videoPage);
            }

            $scope.showComments = function (event) {
                event.preventDefault();

                loadComments();
                $scope.selectedTab = 0;
            };

            $scope.showAbuse = function (event) {
                event.preventDefault();

                $scope.selectedTab = 1;
            };

            $scope.like = function (project, event) {
                event.preventDefault();

                if (!checkAuthorization()) {
                    return;
                }

                if (project.$isLiking) {
                    return;
                }

                project.$isLiking = true;

                if (!project.isLiked) {
                    likesService.add(project.id).then(function() {

                        project.isLiked = true;
                        project.likesCount++;

                        $rootScope.$broadcast(userEvents.likeAdded, project.id);

                        project.$isLiking = false;
                    });
                } else {
                    likesService.delete(project.id).then(function () {

                        project.isLiked = false;
                        project.likesCount = project.likesCount > 0 ? project.likesCount - 1 : 0;

                        $rootScope.$broadcast(userEvents.likeDeleted, project.id);

                        project.$isLiking = false;
                    });
                }
                
            }

            $scope.dislike = function (project, event) {
                event.preventDefault();

                if (!checkAuthorization()) {
                    return;
                }

                if (project.$isDisliking) {
                    return;
                }

                project.$isDisliking = true;

                if (!project.isDisliked) {
                    dislikesService.add(project.id).then(function () {

                        project.isDisliked = true;
                        project.dislikesCount++;

                        $rootScope.$broadcast(userEvents.dislikeAdded, project.id);

                        project.$isDisliking = false;
                    });
                } else {
                    dislikesService.delete(project.id).then(function () {

                        project.isDisliked = false;
                        project.dislikesCount = project.dislikesCount > 0 ? project.dislikesCount - 1 : 0;

                        $rootScope.$broadcast(userEvents.dislikeDeleted, project.id);

                        project.$isDisliking = false;
                    });
                }

            }

            $scope.postComment = function (comment, event) {

                event.preventDefault();

                $scope.commentPosting = true;

                videoCommentsService.add($stateParams.id, comment).then(function(data) {
                        $scope.commentPosting = false;
                        $scope.comment.body = null;
                        $scope.comments.unshift(data);
                    },
                    function(reason) {
                        $scope.commentPosting = false;
                        throw new Error(reason);
                    });

            };

            $scope.deleteComment = function (comment) {

                var idx = $scope.comments.indexOf(comment);
                $scope.comments.splice(idx, 1);

                videoCommentsService.delete($stateParams.id, comment.id).then(function() {
                    },
                    function (reason) {
                        $scope.comments.splice(idx, 0, comment); // restore element if error occured
                        throw new Error(reason);
                    });
            }

            $scope.reportAbuse = function (project, event) {
                
                event.preventDefault();

                if (!checkAuthorization()) {
                    return;
                }

                if (project.$isAbuseReporting) {
                    return;
                }

                project.$isAbuseReporting = true;

                abuseService.add(project.id).then(function() {
                    project.$isAbuseReporting = false;
                }, function() {
                    project.$isAbuseReporting = false;
                });
            }

            $scope.onWatchClose = function () {
                $state.go("^");
            };


            $scope.deleteVideo = function (project) {
                projectsService.delete(project.id).then(function () {
                    $state.go('portal.user.videos', { userId: $rootScope.currentUser.id }, { reload: true });
                });
            };

            $scope.togglePublic = function(project) {
                project.access++;
                if (project.access > 2) {
                    project.access = 0;
                }
                projectsService.update(project).then(function () {
                    switch (project.access) {
                    case 0:
                        $rootScope.$broadcast(userEvents.videoBecomePublic, project.id);
                        break;
                    case 1:
                        $rootScope.$broadcast(userEvents.videoBecomePrivate, project.id);
                        break;
                    case 2:
                        $rootScope.$broadcast(userEvents.videoBecomePrivate, project.id);
                        break;
                    }
                });
            }

            function checkAuthorization() {
                if (!$rootScope.isAuthorized($rootScope.userRoles.user)) {
                    $rootScope.$broadcast(authEvents.authorize);
                    return false;
                } else {
                    return true;
                }
            }

            function loadWatch() {

                $scope.isLoading = true;
                watchService.get($stateParams.id).then(function (data) {

                        $scope.watch = data;

                        // counting view
                        viewsService.add(data.id);

                        // loading comments
                        loadComments();

                        $scope.isLoading = false;

                        // invalidating watch cache to get fresh aggregated values
                        return watchService.get($stateParams.id, {invalidateCache: true}).then(function(data) {
                            $scope.watch.isLiked = data.isLiked;
                            $scope.watch.isDisliked = data.isDisliked;
                        });
                    },
                    function (response) {

                        $scope.isLoading = false;

                        if (response.status == 403 || response.status == 404) {

                            $state.go("^");

                            // Show toast notification
                            toastr.error(response.data.message, null, {
                                closeButton: true,
                                timeOut: 20000
                            });

                        } else {
                            throw new Error(response.data.message);
                        }
                    });
            }

            function loadComments() {

                $scope.commentsLoading = true;

                videoCommentsService.getAll($stateParams.id).then(function(data) {
                        $scope.commentsLoading = false;
                        $scope.comments = data;
                    },
                    function(reason) {
                        $scope.commentsLoading = false;
                        throw new Error(reason);
                    });
            }

            // INIT

            loadWatch();

            // EVENT HANDLERS

            $rootScope.$on('$stateChangeSuccess', function () {

                // hide video popup
                $scope.showWatch = false;

            });

        }
    ]);