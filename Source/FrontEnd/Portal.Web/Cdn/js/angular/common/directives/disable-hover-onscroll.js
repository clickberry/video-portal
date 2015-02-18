// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

// http://habrahabr.ru/post/204238/

angular.module('directives.disable-hover-onscroll', []).directive('disableHoverOnscroll', [
    '$timeout', '$window', function($timeout, $window) {

        var className = 'disable-hover-onscroll';

        function getAllSelectors() {
            var ret = [];
            for (var i = 0; i < document.styleSheets.length; i++) {

                var ss = document.styleSheets[i];
                var rules = ss.rules;              
                if (!rules) {

                    // http://stackoverflow.com/questions/21642277/security-error-the-operation-is-insecure-in-firefox-document-stylesheets

                    try {
                        // In Chrome, if stylesheet originates from a different domain,
                        // ss.cssRules simply won't exist. I believe the same is true for IE, but
                        // I haven't tested it.
                        //
                        // In Firefox, if stylesheet originates from a different domain, trying
                        // to access ss.cssRules will throw a SecurityError. Hence, we must use
                        // try/catch to detect this condition in Firefox.
                        if (!ss.cssRules)
                            return ret;
                    } catch (e) {
                        // Rethrow exception if it's not a SecurityError. Note that SecurityError
                        // exception is specific to Firefox.
                        if (e.name !== 'SecurityError')
                            throw e;
                        return ret;
                    }

                    rules = ss.cssRules;
                }

                for (var x in rules) {
                    if (typeof rules[x].selectorText == 'string') ret.push(rules[x].selectorText);
                }
            }
            return ret;
        }


        function selectorExists(selector) {
            var selectors = getAllSelectors();
            for (var i = 0; i < selectors.length; i++) {
                if (selectors[i] == selector) return true;
            }
            return false;
        }

        return {
            restrict: 'A',
            compile: function() {

                // creating style if required
                if (!selectorExists('.' + className)) {
                    var css = '.' + className + ', .' + className + ' * { pointer-events: none !important; }';
                    var head = document.head || document.getElementsByTagName('head')[0];
                    if (!head) {
                        return null;
                    }

                    var style = document.createElement('style');
                    style.type = 'text/css';

                    if (style.styleSheet) {
                        style.styleSheet.cssText = css;
                    } else {
                        style.appendChild(document.createTextNode(css));
                    }

                    head.appendChild(style);
                }

                // return link function
                return function(scope, element) {

                    var timer;

                    $window.addEventListener('scroll', function() {
                        $timeout.cancel(timer);

                        if (!element.hasClass(className)) {
                            element.addClass(className);
                        }

                        timer = $timeout(function() {
                            element.removeClass(className);
                        }, 500);

                    }, false);

                };
            }
        }
    }
]);