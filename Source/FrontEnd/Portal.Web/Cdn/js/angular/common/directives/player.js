// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.player', ['resources.settings']).directive('cbPlayer', [
    'jQuery', 'settings', function(jQuery, settings) {

        var $ = jQuery;

        if (!$.fn.cbplayer) {
            throw new Error("Clickberry player required");
        }

        return {
            restrict: 'EA',
            link: function($scope, element, attr) {

                $scope.$watch(attr.playerModel, function (value) {

                    if (value) {
                        initPlayer(value);
                    }

                });

                function initPlayer(model) {

                    var poster = model.screenshots && model.screenshots.length > 0 ? model.screenshots[0].uri : null;
                    var external = null;
                    if (model.external) {
                        external = {
                            productName: model.external.productName,
                            videoUrl: model.external.videoUri,
                            screenshotUrl: model.screenshotUrl,
                            acsNamespace: model.external.acsNamespace,
                            flashPlayerUrl: '/extension',
                            jwPlayerRoot: settings.get('jwFlashPlayerUrl'),
                            youtubeHtml5PlayerRoot: settings.get('youtubeHtml5PlayerUrl')
                        };
                    }

                    // building array of encoded videos
                    var videos = {};

                    // grouping by size
                    var grouppedVideos = {};
                    for (var i = 0; i < model.videos.length; i++) {
                        var video = model.videos[i];
                        if (grouppedVideos[video.width]) {
                            grouppedVideos[video.width].push(video);
                        } else {
                            grouppedVideos[video.width] = [video];
                        }
                    }

                    for (var group in grouppedVideos) {
                        if (!grouppedVideos.hasOwnProperty(group)) {
                            continue;
                        }

                        var groupVideos = grouppedVideos[group];
                        var videoFormats = {};
                        for (var j = 0; j < groupVideos.length; j++) {
                            videoFormats[groupVideos[j].contentType] = groupVideos[j].uri;
                        }

                        videos[groupVideos[0].height + 'p'] = {
                            widht: groupVideos[0].width,
                            height: groupVideos[0].height,
                            src: videoFormats
                        }
                    }

                    $(element).cbplayer({
                        avsx: model.avsx,
                        poster: poster,
                        root: settings.get('playerUrl'),
                        projectUrl: model.publicUrl,
                        external: external,
                        videos: videos
                    });
                    
                }

            }
        };
    }
])