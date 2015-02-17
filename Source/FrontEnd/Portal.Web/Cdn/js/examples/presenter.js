// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("presenter", ["jquery", "config"], function($, config) {

    return {
        changePage: function(element) {
            $(".b_menu a").removeClass("active").filter("a[href~='/" + element.menuId + "']").addClass("active");
            $("#pages > div").hide().filter("#" + element.id).show();
        },

        showPlayer: function(data) {

            // External video data
            var external = data.external ? {
                productName: data.external.productName,
                videoUrl: data.external.videoUri,
                screenshotUrl: data.screenshotUrl,
                flashPlayerUrl: "/extension",
                acsNamespace: data.external.acsNamespace,
                jwPlayerRoot: config.constants.jwPlayerRoot
            } : null;

            $(".video").cbplayer({
                avsx: data.avsx,
                poster: data.poster,
                root: config.constants.playersRoot,
                projectUrl: data.publicUrl,
                videos: data.videos,
                external: external
            });
        },

        stopPlayer: function() {
            if (window.html5 && window.html5.engine) {
                window.html5.engine.clearDemand();
            }
            $("video").each(function() {
                this.pause();
            });
            $(".video").empty();
        }
    };
});