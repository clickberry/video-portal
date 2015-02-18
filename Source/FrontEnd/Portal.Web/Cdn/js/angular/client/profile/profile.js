// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.profile", [
        'ui.router',
        'constants.common',
        'directives.model',
        'resources.common',
        'services.clients',
        'directives.loader'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('client.profile', {
                    url: '/profile',
                    data: {
                        pageTitle: 'Client Profile',
                        authorizedRoles: [userRoles.client]
                    },
                    templateUrl: 'client.profile.html',
                    controller: 'ClientProfileCtrl'
                });
        }
    ])

// Controllers
    .controller("ClientProfileCtrl", [
        "$scope", "$rootScope", "$state", "$window", "resources", "clientsService", "toastr",
        function ($scope, $rootScope, $state, $window, resources, clientsService, toastr) {


            // PROPERTIES

            $scope.model = angular.copy($rootScope.client);
            $scope.$watch("client", function (value)
            {
                $scope.model = angular.copy(value);
            });

            $scope.countries = resources.clientCountries;

            $scope.showEin = false;
            var showEinTrigger = function () {

                if ($scope.model && $scope.model.country == "us") {
                    return true;
                } else {
                    return false;
                }
            };
            $scope.$watch(showEinTrigger, function (value) {
                $scope.showEin = value;
            });

            $scope.isLoading = false;



            // METHODS

            $scope.cancel = function () {

                $window.history.back();

            }

            $scope.submit = function (clientData) {

                $scope.isLoading = true;

                // update client
                clientsService.update(clientData).then(function (data) {

                    $scope.isLoading = false;

                    // caching client data
                    $rootScope.client = data;

                    toastr.success("Changes have been saved.");

                }, function (reason) {
                    $scope.isLoading = false;
                    throw new Error(reason);
                });

            };

        }
    ]);