// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

define("mappers", ["config", "knockout"], function(config, ko) {

    function getPoster(item) {
        // Returns a project video poster

        var poster = '';

        switch (item.state) {
        case 0:
            poster = config.constants.videoPosterNotUploadedYet;
            break;

        case 1:
            poster = config.constants.videoPosterEncoding;
            break;

        case 2:
            if (item.screenshotUrl) {
                poster = item.screenshotUrl;
            } else {
                if (item.screenshots.length > 0) {
                    poster = item.screenshots[0].uri;
                } else {
                    poster = config.constants.videoPosterFailed;
                }
            }
            break;
        }

        return poster;
    }

    return {
        mapProject: function(data, vm) {
            vm.name(data.name);
            vm.description(data.description);
            vm.isPublic(data.isPublic);
        },

        mapProjects: function(data, vm, replace, notifications) {
            if (data) {
                data.forEach(function(item) {
                    item.publicUrl = '/examples/' + item.id;
                    item.poster = ko.observable(getPoster(item));
                    item.notified = ko.observable(notifications.indexOf(item.id) >= 0);
                });
            }
            vm.videos(replace === true ? data || [] : vm.videos().concat(data));
        },

        mapWatch: function(data, vm) {
            vm.isEditable(data.isEditable);
            vm.created(data.created);
        },

        mapToPlayerSource: function(data) {

// Process loaded videos
            function processVideos(items) {
                videos = {};

                items.forEach(function(item) {
                    var width = item.width;
                    var name = width + "p";

                    videos[name] = videos[name] || { width: item.width, height: item.height };
                    videos[name].src = videos[name].src || {};
                    videos[name].src[item.contentType] = item.uri;
                });

                return videos;
            }

            return {
                avsx: data.avsx,
                videos: processVideos(data.videos),
                poster: data.screenshots.length > 0 ? data.screenshots[0].uri : "",
                publicUrl: data.publicUrl,
                isReady: data.isReady,
                external: data.external,
                screenshotUrl: data.screenshotUrl
            };
        },
    };
})