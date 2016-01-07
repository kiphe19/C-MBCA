using System.Web;
using System.Web.Optimization;

namespace chevron
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/main").Include(
                    "~/DataTables/datatables.min.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/dataTables.editor.min.js",
                    "~/Scripts/moment.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/DataTables/datatables.min.css",
                    "~/content/editor.dataTables.min.css",
                    "~/content/metro-bootstrap.min.css"
                ));

            bundles.Add(new ScriptBundle("~/Script/xdkJKAds").Include(
                    "~/Scripts/_main.js",
                    "~/Scripts/bootstrap-datetimepicker.min.js"
                ));
            bundles.Add(new ScriptBundle("~/Script/cfgxYSa").Include("~/Scripts/_config.js"));


            BundleTable.EnableOptimizations = true;
        }
    }
}