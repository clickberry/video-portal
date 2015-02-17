// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

(function (window, angular) {
    "use strict";

    angular.module("directives.connectSocialsNotification", [
            "services.acs",
            "ngCookies"
        ])
        .directive("cbConnectSocialsNotification", [
            "acsService", function(acsService) {
                return {
                    restrict: "EA",
                    replace: true,
                    scope: {},
                    templateUrl: "portal.connect-socials-notification.html",
                    controller: [
                        "$scope", "$rootScope", "$cookieStore", function($scope, $rootScope, $cookieStore) {

                            var notShowNotificationsCookieName = "doNotShowConnectSocialAccountsNotification";
                            $scope.showConnectSocialAccountsNotification = !$cookieStore.get(notShowNotificationsCookieName);
                            $scope.allSocialAccountsConnected = true;

                            $scope.socials = [];

                            $scope.closeSocialAccountsNotification = function() {
                                $cookieStore.put(notShowNotificationsCookieName, true);
                                $scope.showConnectSocialAccountsNotification = false;
                            };

                            function initSocialButtons() {
                                if (!$rootScope.currentUser || $scope.socials.length === 0) {
                                    return;
                                }

                                var socialMap = {
                                    1: "fb",
                                    2: "google",
                                    3: "wind",
                                    4: "yahoo",
                                    5: "tw",
                                    6: "vk",
                                    7: "odn"
                                };

                                var connectedCounter = 0;
                                for (var i = 0; i < $rootScope.currentUser.memberships.length; i++) {
                                    for (var j = 0; j < $scope.socials.length; j++) {
                                        if (socialMap[$rootScope.currentUser.memberships[i]] === $scope.socials[j].className) {
                                            $scope.socials[j].disabled = true;
                                            ++connectedCounter;
                                        }
                                    }
                                }

                                if (connectedCounter < $scope.socials.length) {
                                    $scope.allSocialAccountsConnected = false;
                                }
                            }

                            function loadSocials() {
                                acsService.getProviders().then(
                                    function(providers) {
                                        $scope.socials = providers;
                                        initSocialButtons();
                                    });
                            }

                            $rootScope.$watch('currentUser', function (value) {
                                if (value) {
                                    initSocialButtons();
                                }
                            });

                            loadSocials();
                        }
                    ]
                };
            }
        ]);

})(window, angular);