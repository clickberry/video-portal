// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web.Optimization;

namespace Portal.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Styles

            // Master page style for design 2.0
            bundles.Add(new StyleBundle("~/cdn/css/clickberry.css")
                .Include("~/cdn/css/widgets.css",
                    "~/cdn/css/all.css",
                    "~/cdn/css/common.css",
                    "~/content/toastr.css",
                    "~/cdn/css/selectize.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/cdn/css/perfect-scrollbar.css",
                    "~/cdn/css/date-range.css",
                    "~/cdn/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/cdn/styles/css.css")
                .Include("~/content/jquery-ui/jquery.progressbar.css",
                    "~/content/jquery-ui/jquery.ui.selectmenu.css",
                    "~/cdn/css/jquery.checkbox.css",
                    "~/cdn/css/style.css",
                    "~/content/toastr.css",
                    "~/cdn/css/jquery.socialshare.css"));

            bundles.Add(new StyleBundle("~/cdn/css/ui.css")
                .Include("~/cdn/css/jquery-ui.css",
                    "~/cdn/css/jquery.ui.theme.css",
                    "~/cdn/css/angular-ui.css"));

            bundles.Add(new StyleBundle("~/cdn/styles/roles.css")
                .Include("~/cdn/css/role.css",
                    "~/cdn/css/choosen.css"));


            // Scripts

            // JQuery
            bundles.Add(new ScriptBundle("~/cdn/scripts/jquery.js")
                .Include("~/scripts/jquery-{version}.js",
                    "~/scripts/plugins/jquery.placeholder.js",
                    "~/cdn/js/common/resources.js"));

            // AngularJs
            bundles.Add(new ScriptBundle("~/cdn/scripts/clickberry.js")
                .Include("~/scripts/jquery-{version}.js",
                    "~/scripts/jquery-ui-{version}.js",
                    "~/scripts/jquery.checkbox.js",
                    "~/scripts/jquery-migrate-{version}.js",
                    "~/scripts/jquery.placeholder.js",
                    "~/scripts/moment.js",
                    "~/scripts/toastr.js",
                    "~/scripts/plugins/jquery.socialshare.js",
                    "~/scripts/angular.js",
                    "~/scripts/angular-resource.js",
                    "~/scripts/angular-ui-router.js",
                    "~/scripts/ui-utils-mask.js",
                    "~/scripts/angular-animate.js",
                    "~/scripts/angular-cookies.js",
                    "~/scripts/ng-infinite-scroll.js",
                    "~/scripts/jquery.mousewheel.js",
                    "~/scripts/perfect-scrollbar.js",
                    "~/scripts/angular-perfect-scrollbar.js",
                    "~/scripts/angular-cookie.js",
                    "~/scripts/angular-elastic.js",
                    "~/cdn/js/angular/common/constants/*.js",
                    "~/cdn/js/angular/common/directives/*.js",
                    "~/cdn/js/angular/common/resources/*.js",
                    "~/cdn/js/angular/common/services/*.js",
                    "~/cdn/js/angular/common/filters/*.js"));

            // Home page
            bundles.Add(new ScriptBundle("~/cdn/scripts/home.js")
                .Include("~/cdn/js/home/view.home.js"));

            // External scripts
            bundles.Add(new ScriptBundle("~/cdn/scripts/external.js")
                .Include("~/scripts/jquery-migrate-{version}.js",
                    "~/cdn/js/common/external.js"));

            // Portal SPA bundle
            bundles.Add(new ScriptBundle("~/cdn/scripts/portal.js")
                .Include("~/scripts/masonry.pkgd.js",
                    "~/scripts/imagesloaded.pkgd.min.js",
                    "~/cdn/js/angular/*.js",
                    "~/cdn/js/angular/video/*.js",
                    "~/cdn/js/angular/user/*.js",
                    "~/cdn/js/angular/user/profile/*.js",
                    "~/cdn/js/angular/user/videos/*.js",
                    "~/cdn/js/angular/user/videos/video/*.js",
                    "~/cdn/js/angular/user/likes/*.js",
                    "~/cdn/js/angular/user/likes/video/*.js",
                    "~/cdn/js/angular/account/*.js",
                    "~/cdn/js/angular/account/recovery/*.js",
                    "~/cdn/js/angular/account/recovery/email-sent/*.js",
                    "~/cdn/js/angular/account/recovery/set-password/*.js",
                    "~/cdn/js/angular/account/recovery/link-expired/*.js",
                    "~/cdn/js/angular/trends/*.js",
                    "~/cdn/js/angular/trends/video/*.js",
                    "~/cdn/js/angular/latest/*.js",
                    "~/cdn/js/angular/latest/video/*.js",
                    "~/cdn/js/angular/tag/*.js",
                    "~/cdn/js/angular/tag/video/*.js"));

            // Admin SPA bundle
            bundles.Add(new ScriptBundle("~/cdn/scripts/admin.js")
                .Include("~/scripts/jquery-ui-{version}.js",
                    "~/scripts/angular-ui.js",
                    "~/cdn/js/angular/admin/*.js",
                    "~/cdn/js/angular/admin/common/services/*.js",
                    "~/cdn/js/angular/admin/projects/*.js",
                    "~/cdn/js/angular/admin/users/*.js",
                    "~/cdn/js/angular/admin/clients/*.js"));

            // Client SPA bundle
            bundles.Add(new ScriptBundle("~/cdn/scripts/client.js")
                .Include("~/scripts/highcharts.js",
                    "~/scripts/selectize.min.js",
                    "~/cdn/js/angular/client/*.js",
                    "~/cdn/js/angular/client/common/services/*.js",
                    "~/cdn/js/angular/client/common/directives/*.js",
                    "~/cdn/js/angular/client/signup/step1/*.js",
                    "~/cdn/js/angular/client/signup/step2/*.js",
                    "~/cdn/js/angular/client/signup/success/*.js",
                    "~/cdn/js/angular/client/signup/*.js",
                    "~/cdn/js/angular/client/profile/*.js",
                    "~/cdn/js/angular/client/subscriptions/common/directives/*.js",
                    "~/cdn/js/angular/client/subscriptions/add/step1/*.js",
                    "~/cdn/js/angular/client/subscriptions/add/step2/*.js",
                    "~/cdn/js/angular/client/subscriptions/add/*.js",
                    "~/cdn/js/angular/client/subscriptions/stats/details/*.js",
                    "~/cdn/js/angular/client/subscriptions/stats/*.js",
                    "~/cdn/js/angular/client/subscriptions/*.js",
                    "~/cdn/js/angular/client/balance/*.js",
                    "~/cdn/js/angular/client/pay/*.js"));

            // Video Standalone SPA bundle
            bundles.Add(new ScriptBundle("~/cdn/scripts/video-standalone.js")
                .Include("~/cdn/js/angular/video-standalone/*.js"));

            // Video Pages
            bundles.Add(new ScriptBundle("~/cdn/scripts/video.js")
                .Include("~/scripts/jquery-ui-{version}.js",
                    "~/scripts/plugins/jquery.socialshare.js",
                    "~/cdn/js/common/view.video.js"));

            // External libraries
            bundles.Add(new ScriptBundle("~/cdn/scripts/libs.js")
                .Include("~/scripts/sammy-{version}.js",
                    "~/scripts/moment.js",
                    "~/scripts/knockout-{version}.js",
                    "~/scripts/toastr.js",
                    "~/scripts/plugins/jquery.ui.selectmenu.js",
                    "~/scripts/require.js"));

            // Examples SPA
            bundles.Add(new ScriptBundle("~/cdn/scripts/examples.js")
                .Include("~/cdn/js/common/resources.js",
                    "~/cdn/js/common/tools.js",
                    "~/cdn/js/spa/*.js",
                    "~/cdn/js/examples/*.js"));

            // Password recovery
            bundles.Add(new ScriptBundle("~/cdn/scripts/recovery.js")
                .Include("~/scripts/jquery.validate.js",
                    "~/scripts/jquery.validate.unobtrusive.js",
                    "~/scripts/customFormValidation.js"));


            // Roles SPA
            bundles.Add(new ScriptBundle("~/cdn/scripts/roles.js")
                .Include("~/scripts/jquery-ui-{version}.js",
                    "~/scripts/choosen.js",
                    "~/scripts/knockout-{version}.js",
                    "~/scripts/toastr.js",
                    "~/scripts/require.js",
                    "~/cdn/js/common/resources.js",
                    "~/cdn/js/common/tools.js",
                    "~/cdn/js/spa/datacontext.js",
                    "~/cdn/js/spa/infrastructure.js",
                    "~/cdn/js/roles/*.js"));
        }
    }
}