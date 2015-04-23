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
            bundles.Add(new ScriptBundle("~/bundles/stripejs").Include(
                      "~/Scripts/stripe.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
