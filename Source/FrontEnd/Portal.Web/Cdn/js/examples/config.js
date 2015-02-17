// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("config", ["datacontext", "resources"], function(datacontext, resources) {

    var videoFilters = [
        { name: resources.date, key: "Created" },
        { name: resources.name, key: "Name" }
    ];

    var apiAddresses = {
        projects: "/api/projects",
        watch: "/api/watch",
        notifications: "/api/notifications"
    };

    return {
        dataServices: {
            projects: new datacontext(apiAddresses.projects),
            watch: new datacontext(apiAddresses.watch),
            notifications: new datacontext(apiAddresses.notifications),

            create: function(address) {
                return new datacontext(address);
            }
        },

        constants: {
            apis: apiAddresses,
            videoFilters: videoFilters,
            videosPageFetchCount: 8,
            videoPosterEncoding: "/cdn/images/encoding.png",
            videoPosterFailed: "/cdn/images/encoding-failed.png",
            videoPosterNotUploadedYet: "/cdn/images/cloud.png",
            videoPageAddress: "/video",
            embedPageAddress: "/embed",
            videoSideSize: 640,
            playersRoot: window.cb.playerRoot,
            jwPlayerRoot: window.cb.jwPlayerRoot,
            embedVideoCode: "<iframe src='{0}' width='{1}' height='{2}' frameborder='0' webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>"
        },
    };
});