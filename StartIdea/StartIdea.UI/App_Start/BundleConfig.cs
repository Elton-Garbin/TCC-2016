using System.Collections.Generic;
using System.Web.Optimization;

namespace StartIdea.UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var jquery = new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js",
                                                                      "~/Scripts/jquery-ui-{version}.js");

            jquery.Orderer = new AsIsBundleOrderer();
            bundles.Add(jquery);

            var jqueryval = new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate-vsdoc.js",
                                                                            "~/Scripts/cldr.js",
                                                                            "~/Scripts/cldr/*.js",
                                                                            "~/Scripts/jquery.validate.js",
                                                                            "~/Scripts/jquery.validate.unobtrusive.js",
                                                                            "~/Scripts/globalize.js",
                                                                            "~/Scripts/jquery.validate.globalize.js",
                                                                            "~/Scripts/globalize/*.js");
            jqueryval.Orderer = new AsIsBundleOrderer();
            bundles.Add(jqueryval);

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            var bootstrap = new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js",
                                                                            "~/Scripts/respond.min.js",
                                                                            "~/Scripts/moment.min.js",
                                                                            "~/Scripts/moment-with-locales.min.js",
                                                                            "~/Scripts/bootstrap-datetimepicker.min.js");
            bootstrap.Orderer = new AsIsBundleOrderer();
            bundles.Add(bootstrap);

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                "~/Scripts/jquery.inputmask/inputmask.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                "~/Scripts/jquery.inputmask/inputmask.extensions.js",
                "~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/customizados").Include(
                "~/Scripts/masterScript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/jquery-ui.min.css",
                "~/Content/bootstrap.min.css",
                "~/Content/site.css",
                "~/Content/bootstrap-datetimepicker.min.css",
                "~/Content/PagedList.css"));
        }
    }

    public class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }

}
