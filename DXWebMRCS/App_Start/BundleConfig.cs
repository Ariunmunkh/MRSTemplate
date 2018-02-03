﻿using System.Web;
using System.Web.Optimization;

namespace DXWebMRCS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-2.2.4.min.js",
                        "~/Content/js/jquery-ui.min.js",
                        "~/Content/js/bootstrap.min.js",
                        "~/Content/js/jquery-plugin-collection.js",
                        "~/Content/js/revolution-slider/js/jquery.themepunch.tools.min.js",
                        "~/Content/js/revolution-slider/js/jquery.themepunch.revolution.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Language").Include(
                        "~/Scripts/es-es/jquery.validate.extension.js",
                        "~/Scripts/fr-fr/jquery.validate.extension.js",
                        "~/Scripts/_references.js",
                        "~/Scripts/multiLanguajeDemo.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/Site.css",
                        "~/Content/bootstrap.css",
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/jquery-ui.min.css",
                        "~/Content/css/animate.css",
                        "~/Content/css/css-plugin-collections.css",
                        "~/Content/css/menuzord-skins/menuzord-rounded-boxed.css",
                        "~/Content/css/style-main.css",
                        "~/Content/css/preloader.css",
                        "~/Content/css/custom-bootstrap-margin-padding.css",
                        "~/Content/css/responsive.css",
                        "~/Content/js/revolution-slider/css/settings.css",
                        "~/Content/js/revolution-slider/css/layers.css",
                        "~/Content/js/revolution-slider/css/navigation.css",
                        "~/Content/css/colors/theme-skin-color-set-1.css"));

            bundles.Add(new ScriptBundle("~/bundles/revolutionslider").Include(
                        "~/Content/js/custom.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.actions.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.carousel.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.kenburn.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.layeranimation.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.migration.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.navigation.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.parallax.min.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension.slideanims.min.js"));

        }
    }
}
