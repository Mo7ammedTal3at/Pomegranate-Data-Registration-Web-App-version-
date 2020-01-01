using System.Web;
using System.Web.Optimization;

namespace NoorEl7abeebCompanyWebApp
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
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                
                "~/Content/AdminLTE/bower_components/jquery/dist/jquery.min.js",
                "~/Content/AdminLTE/bower_components/bootstrap/dist/js/bootstrap.min.js",
                "~/Content/AdminLTE/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Content/AdminLTE/bower_components/fastclick/lib/fastclick.js",
                "~/Content/AdminLTE/dist/js/adminlte.min.js",
                "~/Content/AdminLTE/dist/js/adminlte.min.js",
                "~/Scripts/sweetalert2.js"
                
                ));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/AdminLTE/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/AdminLTE/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/Content/AdminLTE/bower_components/Ionicons/css/ionicons.min.css",
                      "~/Content/AdminLTE/dist/css/AdminLTE.min.css",
                      "~/Content/AdminLTE/dist/css/skins/_all-skins.min.css",
                      "~/Content/sweetalert2.css"
                      ));
        }
    }
}
