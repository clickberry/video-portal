// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.user.profile", [
        'ui.router',
        'constants.common',
        'directives.model',
        'directives.confirm-click',
        'services.profile',
        'services.password',
        'services.acs',
        'services.memberships',
        'directives.loader'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('portal.user.profile', {
                    url: '/profile',
                    data: {
                        pageTitle: 'Edit Profile on Clickberry',
                        authorizedRoles: [userRoles.user]
                    },
                    templateUrl: 'user.profile.html',
                    controller: 'UserProfileCtrl'
                });
        }
    ])

// Controllers
    .controller("UserProfileCtrl", [
        "$scope", "$rootScope", "$state", "$q", "$window", "profileService", "passwordService", "toastr", "acsService", "membershipsService",
        function ($scope, $rootScope, $state, $q, $window, profileService, passwordService, toastr, acsService, membershipsService) {

            if ($window.location.protocol === "http:") {
                $window.location.href = $window.location.href.toString().replace("http:", "https:");
                return;
            }



            // PROPERTIES

            $scope.isLoading = false;

            $scope.model = angular.copy($rootScope.currentUser);

            $scope.passwordModel = {
                oldPassword: null,
                newPassword: null,
                confirmPassword: null
            };

            $scope.socials = [];

            $scope.allowToDeleteSocialProfile = false;


            // METHODS

            function initSocialButtons() {

                if (!$rootScope.currentUser || $scope.socials.length === 0) {
                    return;
                }

                var socialMap = {
                    1: 'fb',
                    2: 'google',
                    3: 'wind',
                    4: 'yahoo',
                    5: 'tw',
                    6: 'vk',
                    7: 'odn'
                };

                var connectedCounter = 0;
                for (var i = 0; i < $rootScope.currentUser.memberships.length; i++) {
                    for (var j = 0; j < $scope.socials.length; j++) {
                        if (socialMap[$rootScope.currentUser.memberships[i]] === $scope.socials[j].className) {
                            $scope.socials[j].disabled = true;
                            ++connectedCounter;
                            $scope.socials[j].membership = $rootScope.currentUser.memberships[i];
                        }
                    }
                }

                if (connectedCounter < $scope.socials.length) {
                    $scope.allSocialAccountsConnected = false;
                }

                $scope.allowToDeleteSocialProfile = ($rootScope.currentUser.email && $rootScope.currentUser.memberships.length > 0)
                    || (!$rootScope.currentUser.email && $rootScope.currentUser.memberships.length > 1);
            }

            $rootScope.$watch('currentUser', function (value) {

                $scope.model = angular.copy(value);

                if (value) {
                    initSocialButtons();
                }

            });

            function loadSocials() {
                acsService.getProviders().then(
                    function (providers) {
                        $scope.socials = providers;
                        initSocialButtons();
                    });
            }

            $scope.cancel = function () {
                $window.history.back();
            }

            $scope.submit = function (profileData, passwordData) {

                $scope.isLoading = true;
                var tasks = [];
                var errors = [];

                // update profile
                var profileTask = profileService.update(profileData).then(function (data) {

                    // caching profile data
                    $rootScope.currentUser = data;

                }, function(response) {

                    var reason = response.data.message;
                    if (response.status === 409) {
                        reason = "Specified email is already in use. Please choose another one or sign in";
                    };

                    errors.push(reason);
                });

                tasks.push(profileTask);

                // change password
                if (passwordData.oldPassword) {

                    var passwordTask = passwordService.update(passwordData).then(function () {
                        
                        $scope.passwordModel = {
                            oldPassword: null,
                            newPassword: null,
                            confirmPassword: null
                        };

                    }, function (response) {
                        
                        var reason = response.data.message;
                        if (response.status === 400) {
                            reason = "Invalid old password";
                        };

                        errors.push(reason);
                    });
                    tasks.push(passwordTask);
                }


                $q.all(tasks).then(function () {

                    $scope.isLoading = false;
                    if (errors.length > 0) {
                        var message = "Failed to update profile";
                        if (errors.length > 0) {
                            message += ": ";
                            for (var i = 0; i < errors.length; ++i) {
                                message += errors[i];
                                if (i > 0) {
                                    message += ", ";
                                }
                            }
                        }
                        throw new Error(message);
                    } else {
                        toastr.success("Changes have been saved.");
                    }

                }, function () {
                    $scope.isLoading = false;
                    throw new Error("Failed to update profile");
                });

            };

            $scope.deleteSocialProfile = function(social) {
                membershipsService.delete(social.membership).then(
                    function () {
                        social.disabled = false;
                    });
            }


            // Init
            loadSocials();

        }
    ]);