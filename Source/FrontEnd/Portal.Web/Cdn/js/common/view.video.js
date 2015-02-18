// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

(function ($) {
    // Copy embed video text
    $("#copy_txt").on("click", function() {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", $("#fe_text").text());
    });

    // Select embed text on click
    $("#fe_text").on("click", function() {
        this.select();
    });

    $(".social-share").share();

    // Utilizes tag resharing functionality from player
    $(document).ready(function() {
        $(".banners .facebook a").on("click", function(e) {
            e.preventDefault();

            if ($(this).hasClass("disabled")) {
                return;
            }

            $(".video").cbplayer('resharetag', 1);
        });

        $(".banners .twitter a").on("click", function(e) {
            e.preventDefault();

            if ($(this).hasClass("disabled")) {
                return;
            }

            $(".video").cbplayer('resharetag', 3);
        });

        $(".video").on("resharetag", function(e) {
            $(".banners .twitter a, .banners .facebook a").toggleClass("disabled", e.resharing);
        });
    });
})(jQuery);