using System.Web;
using System.Web.Optimization;

namespace chevron
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/main").Include(
                    "~/DataTables/jQuery-2.1.4/jquery-2.1.4.min.js",
                    "~/DataTables/datatables.min.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/dataTables.editor.min.js",
                    "~/Scripts/jquery.unobtrusive-ajax.min.js",
                    "~/Scripts/moment.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/DataTables/datatables.min.css",
                    "~/content/editor.dataTables.min.css",
                    "~/content/metro-bootstrap.min.css"
                    //"~/content/bootstrap-flat.min.css",
                    //"~/content/bootstrap-flat-extras.min.css"
                ));

            bundles.Add(new ScriptBundle("~/Script/xdkJKAds").Include("~/Scripts/_main.js"));
            bundles.Add(new ScriptBundle("~/Script/cfgxYSa").Include("~/Scripts/_config.js"));


            BundleTable.EnableOptimizations = true;
        }
    }
}