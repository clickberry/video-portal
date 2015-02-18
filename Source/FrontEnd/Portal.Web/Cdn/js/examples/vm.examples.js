// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

define("vm.examples", ["knockout", "config", "mappers", "resources", "toastr"],
    function(ko, config, mappers, resources, toastr) {

        return function() {
            var self = this;

            self.availableFilters = ko.observableArray(config.constants.videoFilters);
            self.selectedFilter = ko.observable(self.availableFilters()[0]);

            self.videoPage = 0;
            self.videos = ko.observableArray([]);
            self.showMoreVideos = ko.observable(false);
            self.loading = ko.observable(false);
            self.searchText = ko.observable("");

            var sentNotificatios = [];

            // Page loaded
            self.activate = function() {
                document.title = resources.video + resources.on + resources.title;
                self.reloadVideos();

                config.dataServices.notifications.get()
                    .done(function(data) {
                        sentNotificatios = data.reduce(function(previousValue, currentValue) {
                            if (previousValue.indexOf(currentValue.projectId) < 0) {
                                previousValue.push(currentValue.projectId);
                            }

                            return previousValue;
                        }, []);

                        self.videos().forEach(function(video) {
                            video.notified(sentNotificatios.indexOf(video.id) >= 0);
                        });
                    })
                    .fail(function() {
                        toastr.error(resources.notificationsLoadFailed);
                    });
            };

            // Reload videos
            self.reloadVideos = function() {
                self.videoPage = 0;
                self.loadVideos(true);
            };

            // Load videos
            self.loadVideos = function (replace) {

                var searchText = self.searchText();
                if (searchText.length > 0) {
                    searchText = searchText.toLowerCase().replace(/'/g, "''").replace(/#/g, '%23');
                }

                var url = "?$top={0}{1}&$orderby={2}{3}".format(
                    config.constants.videosPageFetchCount,
                    self.videoPage == 0 ? "" : "&$skip={0}".format(self.videoPage * config.constants.videosPageFetchCount),
                    self.selectedFilter().key.toLowerCase() == 'created' ? "{0} desc".format(self.selectedFilter().key) : self.selectedFilter().key,
                    searchText.length == 0 ? "" : "&$filter=Name eq '{0}'".format(searchText)
                );

                self.loading(true);

                config.dataServices.watch.get(url)
                    .done(function(data) {
                        mappers.mapProjects(data, self, replace, sentNotificatios);
                        self.showMoreVideos(data && data.length == config.constants.videosPageFetchCount);
                        self.videoPage++;
                    })
                    .fail(function() {
                        self.showMoreVideos(false);
                        toastr.error(resources.videosLoadFailed);
                    })
                    .always(function() {
                        self.loading(false);
                    });
            };

            // Changed filter
            self.selectedFilterChanged = function(data) {
                if (data == self.selectedFilter()) {
                    return;
                }
                self.selectedFilter(data);
                self.reloadVideos();
            };

            // Setup Google Chrome voice search
            self.searchTextChanged = function(e) {
                searchText(e.target.value);
                reloadVideos();
            };
        };
    });