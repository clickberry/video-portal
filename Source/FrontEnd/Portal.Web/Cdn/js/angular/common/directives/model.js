// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.model', [])
    .directive('cbModelConfirm', function() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, elem, attrs, control) {
                var checker = function() {

                    // get the value of the first input
                    var e1 = scope.$eval(attrs.ngModel);

                    // get the value of the second input  
                    var e2 = scope.$eval(attrs.cbModelConfirm);

                    // identical values or both not specified (null or '')
                    return e1 == e2 || (!e1 && !e2);
                };
                scope.$watch(checker, function(value) {

                    // set the form control to valid if both 
                    // inputs are the same, else invalid
                    control.$setValidity("match", value);
                });
            }

        };
    })
    .directive('cbModelEin', function() {

        var regexp = /^(\d{2}-\d{7})?$/;

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function(viewValue) {
                    if (regexp.test(viewValue)) {
                        // it is valid
                        ctrl.$setValidity('ein', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('ein', false);
                        return undefined;
                    }
                });
            }
        };
    })
    .directive('cbModelPhone', function() {

        var regexp = /^.{0,64}$/;

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function(viewValue) {
                    if (regexp.test(viewValue)) {
                        // it is valid
                        ctrl.$setValidity('phone', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('phone', false);
                        return undefined;
                    }
                });
            }
        };
    })
    .directive('cbModelZipcode', function() {

        var regexp = /^[a-z0-9\s-]{5,10}$/i;

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function(viewValue) {
                    if (regexp.test(viewValue)) {
                        // it is valid
                        ctrl.$setValidity('zipcode', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('zipcode', false);
                        return undefined;
                    }
                });
            }
        };
    })
    .directive('cbModelHostName', function () {

        var regexp = /^(https?\:)\/\/(([^:\/?#]*)(?:\:([0-9]+))?)(\/[^?#]*)?(\?[^#]*|)(#.*|)$/;

        function getLocation(href) {

            var match = href.match(regexp);

            var res = match && {
                protocol: match[1],
                host: match[2],
                hostname: match[3],
                port: match[4],
                pathname: match[5],
                search: match[6],
                hash: match[7]
            }

            return res || {};
        }


        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {

                        var domainRegexp = /^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$/;
                        var uri;

                        if (domainRegexp.test(viewValue)) {
                            uri = { hostname: viewValue };
                        } else {
                            uri = getLocation(viewValue);
                        }

                        if (uri.hostname) {

                            ctrl.$setValidity('hostName', true);

                            if (viewValue !== uri.hostname) {
                                elm.context.value = uri.hostname;
                                ctrl.$setViewValue(uri.hostname);
                            }

                            return uri.hostname;
                        } else {
                            ctrl.$setValidity('hostName', false);
                            return undefined;
                        }


                });
            }
        };
    })
    .directive('cbModelPosInt', function () {

        var regexp = /^\d+$/;

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (regexp.test(viewValue) && parseInt(viewValue) > 0) {
                        // it is valid
                        ctrl.$setValidity('posInt', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('posInt', false);
                        return undefined;
                    }
                });
            }
        };
    })
    .directive('cbModelCardNumber', ['stripeService', function (stripeService) {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (stripeService.card.validateCardNumber(viewValue)) {
                        // it is valid
                        ctrl.$setValidity('cardNumber', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('cardNumber', false);
                        return undefined;
                    }
                });
            }
        };
    }])
    .directive('cbModelCardExp', ['stripeService', function (stripeService) {

        var regexp = /^(0[1-9]|1[0-2])\/(\d{2})$/;
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {

                    var match = viewValue.match(regexp);
                    var month = match && match[1] || null;
                    var year = match && match[2] || null;

                    if (stripeService.card.validateExpiry(month, '20' + year)) {
                        // it is valid
                        ctrl.$setValidity('cardExp', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('cardExp', false);
                        return undefined;
                    }
                });
            }
        };
    }])
    .directive('cbModelCardCvc', ['stripeService', function (stripeService) {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (stripeService.card.validateCVC(viewValue)) {
                        // it is valid
                        ctrl.$setValidity('cardCvc', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('cardCvc', false);
                        return undefined;
                    }
                });
            }
        };
    }])
    .directive('cbModelEmail', function () {

        // E-mail should contains a proper domain name
        var email = /^.*@.*\..*$/i;

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    return email.test(viewValue) && viewValue.indexOf("..") === -1 ? viewValue : undefined;
                });
            }
        };
    });