using System.Web;
using System.Web.Optimization;

namespace AWIC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

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
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // add general css
            bundles.Add(new StyleBundle("~/bundles/generalcss").Include(
                      "~/Content/general.css"));
            // add flexslider js
            // add scripts.js
            bundles.Add(new ScriptBundle("~/bundles/sliderscripts").Include(
                      "~/Scripts/jquery.flexslider-min.js",
                      "~/Scripts/scripts.js"));
            bundles.Add(new StyleBundle("~/bundles/hbcalcss").Include(
                      "~/Content/jquery.event.calendar.css",
                      "~/Content/green.event.calendar.css"));
            bundles.Add(new ScriptBundle("~/bundles/hbcalscripts").Include(
                      "~/Scripts/jquery.event.calendar.js", 
                      "~/Scripts/jquery.event.calendar.en.js"));
            bundles.Add(new StyleBundle("~/bundles/photoswipecss").Include(
                "~/Content/photoswipe.css",
                "~/Content/default-skin.css"));
            bundles.Add(new ScriptBundle("~/bundles/photoswipescripts").Include(
                      "~/Scripts/photoswipe.js",
                      "~/Scripts/photoswipe-ui-default.js",
                      "~/Scripts/initPhotoSwipe.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
