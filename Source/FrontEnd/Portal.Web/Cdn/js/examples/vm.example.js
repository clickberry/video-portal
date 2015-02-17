// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("vm.example", ["knockout", "config", "mappers", "presenter", "resources", "toastr", "tools"],
    function(ko, config, mappers, presenter, resources, toastr, tools) {

        var states = {
            loading: 'loading',
            error: 'error',
            uploading: 'uploading',
            encoding: 'encoding',
            ready: 'ready'
        };

        return function() {
            var self = this;

            var projectId;

            // ViewModel state
            self.errorText = ko.observable("");
            self.notified = ko.observable(false);
            self.state = ko.observable(states.loading);

            // Project
            self.project = {
                name: ko.observable(""),
                description: ko.observable(""),
                isPublic: ko.observable(false),
            };

            // Watch data
            self.watch = {
                isEditable: ko.observable(false),
                created: ko.observable(undefined),
                isExternal: ko.observable(false),
            };

            var timeoutId;

            // Page loaded
            self.activate = function(params) {
                document.title = resources.video + resources.on + resources.title;
                projectId = params.videoId;

                self.state(states.loading);

                config.dataServices.notifications.get(projectId)
                    .done(function(data) {
                        self.notified(data && data.length > 0);
                    })
                    .fail(function(args) {
                        var errors = tools.handleResponseErrors(args);
                        toastr.error(errors);
                    });

                config.dataServices.watch.get(projectId)
                    .done(function(data) {

                        mappers.mapProject(data, self.project);
                        mappers.mapWatch(data, self.watch);
                        self.errorText('');

                        switch (data.state) {
                        case 0:
                            self.state(states.uploading);
                            break;
                        case 1:
                            self.state(states.encoding);
                            break;
                        case 2:
                            presenter.showPlayer(mappers.mapToPlayerSource(data));
                            self.state(states.ready);
                            break;
                        default:
                            self.errorText(resources.videoInvalidData);
                            self.state(states.error);
                        }

                        // Wait while video is processing
                        if (data.state != 2) {
                            timeoutId = setTimeout(function() {
                                self.activate(params);
                            }, 10000);
                        }
                    })
                    .fail(function(args) {
                        var errors = tools.handleResponseErrors(args);
                        self.state(errors);
                    });
            };

            // Page loaded
            self.deactivate = function() {
                if (timeoutId) clearTimeout(timeoutId);
                presenter.stopPlayer();
            };

            // Send push notification
            self.sendNotification = function() {
                var title = prompt("Input push message title");
                if (!title) {
                    return;
                }

                config.dataServices.notifications.post({ projectId: projectId, title: title })
                    .done(function() {
                        toastr.success(resources.notificationsPostSucceeded);
                        self.notified(true);
                    })
                    .fail(function(args) {
                        var errors = tools.handleResponseErrors(args);
                        toastr.error(errors);
                    });
            };
        };
    });