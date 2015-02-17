angular.module('perfect_scrollbar', []).directive('perfectScrollbar', [
    '$parse', 'jQuery', '$window', function($parse, $, $window) {
        var psOptions = [
            'wheelSpeed', 'wheelPropagation', 'minScrollbarLength', 'useBothWheelAxes',
            'useKeyboard', 'suppressScrollX', 'suppressScrollY', 'scrollXMarginOffset',
            'scrollYMarginOffset', 'includePadding'
        ];

        return {
            restrict: 'E',
            transclude: true,
            template: '<div><div ng-transclude></div></div>',
            replace: true,
            link: function($scope, $elem, $attr) {
                var options = {};
                var enabled = true;

                for (var i = 0, l = psOptions.length; i < l; i++) {
                    var opt = psOptions[i];
                    if ($attr[opt] != undefined) {
                        options[opt] = $parse($attr[opt])();
                    }
                }

                $elem.perfectScrollbar(options);

                if ($attr.refreshOnChange) {
                    $scope.$watchCollection($attr.refreshOnChange, function() {
                        $scope.$evalAsync(function() {
                            $elem.perfectScrollbar('update');
                        });
                    });
                }

                $elem.bind('$destroy', function() {
                    $elem.perfectScrollbar('destroy');
                    enabled = false;
                });

                $($window).resize(function() {
                    if ($($window).width() >= 1240) {
                        if (!enabled) {
                            enabled = true;
                            $elem.perfectScrollbar(options);
                        }
                    } else {
                        if (enabled) {
                            enabled = false;
                            $elem.perfectScrollbar('destroy');
                        }
                    }
                });
            }
        };
    }
]);
