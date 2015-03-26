using System.Web;
using System.Web.Optimization;

namespace StankinQuestionnaire
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/InitGrid").Include(
                "~/Scripts/Admin/Grid.js",
                "~/Scripts/Admin/Init.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ladda").Include(
                "~/Scripts/Modules/spin.min.js",
                "~/Scripts/Modules/ladda.js"
                ));

            bundles.Add(new StyleBundle("~/Content/ladda").Include(
                "~/Content/Modules/ladda-themeless.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/multiselect").Include(
                "~/Content/Modules/multi-select.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/multiselect").Include(
                "~/Scripts/Modules/jquery.multi-select.js"
                ));

            // Присвойте EnableOptimizations значение false для отладки. Дополнительные сведения
            // см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
