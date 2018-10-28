using System.Web;
using System.Web.Optimization;

namespace Sub
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


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/revolutionslider").Include(
                        "~/Content/js/custom.js",
                        "~/Content/js/revolution-slider/js/extensions/revolution.extension*"));

            bundles.Add(new StyleBundle("~/bundles/Styles").Include("~/Content/Site.css", "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryel").Include(
                       "~/Scripts/all.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/app.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new StyleBundle("~/Content/cssel").Include(
                      "~/Content/all.css",
                      "~/Content/app.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
