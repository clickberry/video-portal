((function($) {

    var networks = {
        "facebook": {
            name: "Facebook",
            getUrl: function(page) {
                return "https://www.facebook.com/sharer.php?u=" + page.url + "&t=" + page.title;
            }
        },
        "google-plus": {
            name: "Google+",
            getUrl: function(page) {
                return "https://plus.google.com/share?url=" + page.url;
            }
        },
        "twitter": {
            name: "Twitter",
            getUrl: function(page) {
                return "https://twitter.com/intent/tweet?url=" + page.url + "&text=" + encodeURIComponent(decodeURIComponent(page.title).substring(0, 140));
            }
        },
        "livejournal": {
            name: "LiveJournal",
            getUrl: function(page) {
                return "http://www.livejournal.com/update.bml?subject=" + page.title + "&event=" + page.url;
            }
        },
        "blogger": {
            name: "Blogger",
            getUrl: function(page) {
                return "http://blogger.com/blog-this.g?t=" + page.description + "&n=" + page.title + "&u=" + page.url;
            }
        },
        "pinterest": {
            name: "Pinterest",
            getUrl: function(page) {
                return "http://pinterest.com/pin/create/button/?url=" + page.url + "&media=" + page.media + "&description=" + page.description;
            }
        },
        "linkedin": {
            name: "LinkedIn",
            getUrl: function(page) {
                return "http://www.linkedin.com/shareArticle?mini=true&url=" + page.url + "&title=" + page.title + "&summary=" + page.description + "&source=clickberry.tv";
            }
        },
        "vk": {
            name: "VK",
            getUrl: function(page) {
                return "http://vk.com/share.php?url=" + page.url + "&description=" + page.description + "&image=" + page.media;
            }
        },
        "tumblr": {
            name: "Tumblr",
            getUrl: function (page) {
                return "http://www.tumblr.com/share/photo?source=" + page.media + "&caption=" + page.title + "&clickthru=" + page.url;
            }
        },
        "reddit": {
            name: "Reddit",
            getUrl: function(page) {
                return "http://reddit.com/submit?url=" + page.url;
            }
        },
        "email": {
            name: "E-mail",
            getUrl: function(page) {
                return "mailto:?to=&subject=" + page.title + "&body=" + page.url;
            }
        }
    };

    var share = "Share to ";

    function getTitle() {
        return $(document.head).find("meta[property='og:title']:first").attr("content") ||
            document.title || "";
    }

    function getDescription() {
        return $(document.head).find("meta[name='description']:first").attr("content") ||
            $(document.head).find("meta[property='og:description']:first").attr("content") || "";
    }

    function getMedia() {
        return $(document.head).find("meta[property='og:image']:first").attr("content") ||
            $(document.head).find("meta[property='og:image:secure_url']:first").attr("content") ||
            $(document.head).find("meta[property='og:video']:first").attr("content") ||
            $(document.body).find("video:first").attr("poster") ||
            $(document.body).find("img:first").attr("src") || "";
    }

    $.fn.socialshare = function (settings) {

        settings = settings || {};
        settings.title = encodeURIComponent(settings.title || getTitle());
        settings.description = encodeURIComponent(settings.description || getDescription());
        settings.media = encodeURIComponent(settings.media || getMedia());
        settings.url = encodeURIComponent(settings.url || document.location.href);
        settings.share = settings.share || share;

        $(this).find("li").each(function() {
            var $this = $(this);
            var networkId = $this.attr("class");
            var network = networks[networkId];

            if (!network) {
                console.log("Invalid social network id: " + network);
                return;
            }

            $this.html($("<a></a>").attr({
                href: network.getUrl(settings),
                title: settings.share + network.name,
                target: "_blank"
            }));
        });

        return this;
    };
})(jQuery));