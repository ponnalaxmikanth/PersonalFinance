using System.Web;
using System.Web.Optimization;

namespace PersonlaFinance
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Scripts/bootstrap.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //                "~/Content/bootstrap.css",
            //                "~/Content/PersonalFinance.css",
            //                "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            //"~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/datepicker.css",
            //            "~/Content/themes/base/datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css",
            //            "~/Content/themes/base/jquery.multiselect.css",
            //            "~/Content/DataTables-1.10.11/media/css/dataTables.bootstrap.css"
            //            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                            "~/Content/bootstrap.css",
                            "~/Content/PersonalFinance.css",
                            "~/Content/themes/base/datepicker.css",
                            "~/Content/themes/base/jquery.multiselect.css",
                            "~/Content/dataTables.bootstrap.css",
                            "~/Content/jquery.dataTables.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css",
                        "~/Content/themes/base/jquery.multiselect.css",
                        "~/Content/jquery.dataTables.css",
                        "~/Content/DataTables-1.10.11/extensions/Buttons/css/buttons.dataTables.css",
                        "~/Content/DataTables-1.10.11/extensions/Select/css/select.dataTables.css"
                //"~/Content/DataTables-1.10.11/extensions/Editor/editor.dataTables.min.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Common").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/Widgets/jquery.multiselect.js",
                        "~/Scripts/DataTables-1.10.11/media/js/jquery.dataTables.js",
                        "~/Scripts/DataTables-1.10.11/extensions/Buttons/js/dataTables.buttons.js",
                        "~/Scripts/DataTables-1.10.11/extensions/Select/js/dataTables.select.js",
                //"~/Scripts/DataTables-1.10.11/extensions/dataTables.editor.min.js",
                //"~/Scripts/DataTables-1.10.11/extensions/Editor/dataTables.editor.js",
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/MutualFunds").Include(
                        "~/Scripts/MutualFunds/MutualFunds.js"));

            #region Mutual Funds

            bundles.Add(new StyleBundle("~/Content/MutualFunds/css").Include(
                           "~/Content/bootstrap.css",
                           "~/Content/themes/base/datepicker.css",
                           "~/Content/dataTables.bootstrap.css",
                           "~/Content/MutualFunds/Site.css",
                           "~/Content/dataTables.bootstrap.css",
                            "~/Content/jquery.dataTables.css",
                            "~/Content/DataTables-1.10.11/extensions/Buttons/css/buttons.dataTables.css",
                            "~/Content/DataTables-1.10.11/extensions/Select/css/select.dataTables.css"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/MutualFunds/Common").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/Dashboard").Include(
                        "~/Scripts/MutualFunds/Dashboard.js",
                        "~/amcharts/amcharts.js",
                        "~/amcharts/pie.js",
                        "~/amcharts/serial.js",
                        "~/amcharts/themes/light.js",
                        "~/Scripts/DataTables-1.10.11/media/js/jquery.dataTables.js",
                        "~/Scripts/DataTables-1.10.11/extensions/Buttons/js/dataTables.buttons.js",
                        "~/Scripts/DataTables-1.10.11/extensions/Select/js/dataTables.select.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/BenchmarkHistory").Include(
                        "~/Scripts/MutualFunds/BenchmarkHistory.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/History").Include(
                        "~/Scripts/MutualFunds/History.js",
                        "~/amcharts/amcharts.js",
                        "~/amcharts/pie.js",
                        "~/amcharts/serial.js",
                        "~/amcharts/themes/light.js"
                        ));



            #endregion
        }
    }
}