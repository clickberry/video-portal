// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Uses http://momentjs.com/ to format date (date format is differ from default angular 'date' filter) and ignores timezone.

angular.module('filters', [])
    .filter('apiDate', [
            'moment', function(moment) {

                return function(text, format) {
                    var date = moment.utc(text); // ignore timezone
                    return date.format(format);
                };

            }
        ]
    )
    .filter('apiDateFromNow', [
            'moment', function(moment) {

                return function(text) {
                    var date = moment.utc(text); // ignore timezone
                    return date.fromNow();
                };

            }
        ]
    )
    .filter('schemeAgnostic', [
            function() {

                return function(text) {

                    var pattern = /^(http:)|(https:)/i;
                    if (!text || !pattern.test(text)) {
                        return text;
                    }

                    var match = text.match(pattern);
                    var scheme = match[0];

                    text = text.substr(scheme.length);

                    return text;

                };

            }
        ]
    )
    .filter('watchScreenshotUrl', [
            function() {

                return function(watch) {

                    if (!watch.screenshotUrl && watch.screenshots && watch.screenshots.length > 0) {
                        watch.screenshotUrl = watch.screenshots[0].uri;
                    }

                    return watch.screenshotUrl;

                };

            }
        ]
    )
    .filter('hashTags', [
            '$sce', function($sce) {

                return function (str, linkPrefix) {
                    if (linkPrefix == undefined) {
                        linkPrefix = '#';
                    }
                    return $sce.trustAsHtml(str.replace(/#([^#\s]+)/g, '<a href="' + linkPrefix + '$1">#$1</a>'));
                };
            }
        ]
    );