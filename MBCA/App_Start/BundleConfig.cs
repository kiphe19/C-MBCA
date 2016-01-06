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
                    "~/Scripts/moment.min.js",
                    "~/Scripts/_main.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/DataTables/datatables.min.css",
                    "~/content/editor.dataTables.min.css",
                    "~/content/metro-bootstrap.min.css"
                ));
            BundleTable.EnableOptimizations = true;
        }
    }
}