// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.masonry', []).directive('cbMasonry', [
    'masonry', function (masonry) {

        if (!masonry) {
            throw new Error("Masonry.js required");
        }

        return {
            restrict: 'EA',
            link: function ($scope, element, attr) {

                var options = {
                    columnWidth: attr.masonryColumnWidth ? parseInt(attr.masonryColumnWidth) : null,
                    itemSelector: attr.masonryItemSelector ? "." + attr.masonryItemSelector : null,
                    gutter: attr.masonryGutter ? parseInt(attr.masonryGutter) : null
                };

                var msnry;

                $scope.$watch(attr.masonryItems, function() {

                    // items changed
                    $scope.$evalAsync(function() {
                    
                        msnry = new Masonry(element[0], options);

                        imagesLoaded(element[0], function () {
                            msnry.layout();
                        });

                    });

                });

            }
        };
    }
])