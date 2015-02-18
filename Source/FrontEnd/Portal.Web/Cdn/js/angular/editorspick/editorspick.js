// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.editorspick", [
        "ngClickberry.editorspick.video",
        'ui.router',
        'directives.masonry',
        'directives.loader',
        'services.examples',
        'infinite-scroll'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function ($stateProvider, userRoles) {
            $stateProvider
                .state('portal.editorspick', {
                    url: '/editorspick',
                    data: {
                        pageTitle: 'Clickberry Editor\'s Pick',
                        authorizedRoles: [userRoles.all]
                    },
                    templateUrl: 'editorspick.html',
                    controller: 'EditorsPickCtrl'
                });
        }
    ])
// Controllers
    .controller("EditorsPickCtrl", [
        "$scope", "examplesService",
        function ($scope, examplesService) {


            // PROPERTIES

            $scope.examples = [];

            $scope.filter = {
                skip: 0,
                top: 20
            };

            $scope.isAllLoaded = false;
            $scope.isLoading = false;


            // METHODS

            $scope.nextPage = function () {

                if ($scope.isLoading || $scope.isAllLoaded) {
                    return;
                }

                $scope.filter.skip += $scope.filter.top;

                // loading examples
                filterExamples();
            };


            // PRIVATE METHODS

            function filterExamples() {

                if (!$scope.filter.skip) {
                    $scope.examples = [];
                }

                $scope.isLoading = true;

                examplesService.query($scope.filter).then(
                    function (data) {

                        $scope.isLoading = false;

                        if ($scope.filter.skip > 0) {
                            $scope.examples = $scope.examples.concat(data);
                        } else {
                            $scope.examples = data;
                        }

                        if (data.length < $scope.filter.top) {
                            $scope.isAllLoaded = true;
                        } else {
                            $scope.isAllLoaded = false;
                        }

                    },
                    function () {
                        $scope.isLoading = false;
                    });
            };


            // INIT

            // loading examples
            filterExamples();

        }
    ]);