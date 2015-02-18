// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

// overrides the behavior of unhandled exceptions processing
angular.module('services.exceptions', [])
    .factory('$exceptionHandler', [
        'toastr', "$injector", function (toastr, $injector) {

            return function (exception) {

                // Old Opera versions fails when accessing document.domain value
                if (exception instanceof ReferenceError &&
                    exception.message.indexOf("Security error:") === 0) {
                    return;
                }

                // Show toast notification
                toastr.error(exception.message, null, {
                    closeButton: true,
                    timeOut: 20000
                });

                // Log in console
                console.log(exception);

                // Send issue to jira collector
                var settings = $injector.get('resources.settings');
                if (!settings) return;

                var jiraIssueCollector = settings.get('jiraIssueCollector');
                if (!jiraIssueCollector) return;

                var $location = $injector.get('$location');
                var $http = $injector.get('$http');
                var user = $injector.get('$rootScope').currentUser;

                $http({
                    method: 'POST',
                    url: jiraIssueCollector,
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    transformRequest: function(obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    },
                    data: {
                        summary: "Uncaught JavaScript Exception",
                        environment: $location.host(),
                        description: "URL: " + $location.absUrl() +
                            "\nUser: " + (user ? user.id : "anonymous") +
                            "\nBrowser: " + $injector.get('$window').navigator.userAgent +
                            "\nException:\n{code}\n" + exception +
                            "\n{code}\nStack Trace:\n{code}\n" + exception.stack + "\n{code}",
                    }
                });
            };

        }
    ]);