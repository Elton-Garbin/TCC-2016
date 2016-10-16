using System.Web.Optimization;

namespace StartIdea.UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", 
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/globalize.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate-vsdoc.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobstrusive.js",
                        "~/Scripts/jquery.validate.globalize.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/customizados").Include(
                        "~/Scripts/masterScript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-datetimepicker.min.css"));
        }
    }
}
