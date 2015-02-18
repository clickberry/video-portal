// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('directives.watchcard', []).directive('cbWatchCard', [
    function() {
        return {
            restrict: 'EA',
            require: 'ngModel',
            replace: true,
            scope: {
                model: '=ngModel',
                isOwner: '=',
                onDelete: '=',
                onTogglePublic: '='
            },
            templateUrl: 'portal.watch-card.html'
        };
    }
]);