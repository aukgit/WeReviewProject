using System;
using System.Web;
using System.Web.Optimization;

namespace WereViewApp {
    public static class BundleConfig {
        private enum BundleSelector {
            ShowEveryScript,
            EveryScriptThroughBundle,
            EveryScriptGzip
        }
        public static void RegisterBundles(BundleCollection bundles) {

            #region CDN Constants
            const string jQueryCdn = "http://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js";
            //const string jQueryCDN = "http://code.jquery.com/jquery-latest.min.js";
            const string mordernizerCdn = "http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js";
            //const string respondJsCDN = "http://cdnjs.cloudflare.com/ajax/libs/respond.js/1.4.2/respond.min.js"
            #endregion

            const BundleSelector bundleSelector = BundleSelector.ShowEveryScript;

            #region jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery", jQueryCdn)
                        .Include("~/Content/Scripts/jquery-2.1.3.min.js") //if no CDN
            );
            #endregion


            switch (bundleSelector) {
                case BundleSelector.ShowEveryScript:
                    #region JavaScripts Bundle

                    #region Validation Bundle & Form Inputs Processing
                    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    "~/Content/Scripts/jquery.validate.js",
                                    "~/Content/Scripts/jquery.validate.unobtrusive.js",

                                    "~/Content/Scripts/moment.js",
                                    "~/Content/Scripts/bootstrap-datetimepicker.js",
                                    "~/Content/Scripts/bootstrap-select.js",
                                    "~/Content/Scripts/bootstrap-table.js",
                                    "~/Content/Scripts/bootstrap-table-filter.js",
                                    "~/Content/Scripts/bootstrap-table-export.js",
                                    "~/Content/Scripts/jquery.elastic.source.js",
                                    "~/Content/Scripts/DevOrgPlugins/developers-organism.dynamicSelect.js",
                                    "~/Content/Scripts/DevOrgPlugins/developers-organism.country-phone.js",
                                    "~/Content/Scripts/DevOrgPlugins/dev-component-runner.js"
                                   ));
                    #endregion


                    #region Upload
                    bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                                        "~/Content/Scripts/Upload/jquery.ui.widget.js",
                                        "~/Content/Scripts/Upload/load-image.all.min.js",
                                        "~/Content/Scripts/Upload/canvas-to-blob.min.js",
                                        "~/Content/Scripts/Upload/jquery.iframe-transport.js",
                                        "~/Content/Scripts/Upload/jquery.fileupload.js",
                                        "~/Content/Scripts/Upload/jquery.fileupload-process.js",
                                        "~/Content/Scripts/Upload/jquery.fileupload-image.js",
                                        "~/Content/Scripts/Upload/jquery.fileupload-audio.js",
                                        "~/Content/Scripts/Upload/jquery.fileupload-video.js",
                                        "~/Content/Scripts/Upload/jquery.fileupload-validate.js",
                                        "~/Content/Scripts/DevOrgPlugins/developers-organism.upload.js"
                                   ));
                    #endregion

                    #region Bootstrap
                    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                  "~/Content/Scripts/DevOrgPlugins/byId.js", 
                                  "~/Content/Scripts/bootstrap.js", // 3.1.2
                                  "~/Content/Scripts/star-rating.js",
                                  "~/Content/Scripts/underscore.js",
                                  "~/Content/Scripts/DevOrgPlugins/every-page.js",
                                  "~/Content/Scripts/FrontEnd/wow.min.js",
                                  "~/Content/Scripts/FrontEnd/jquery.sticky.js",
                                  "~/Content/Scripts/FrontEnd/jquery.stellar.js",
                                  "~/Content/Scripts/FrontEnd/base-theme.js",
                                  "~/Content/Scripts/FrontEnd/jquery.isotope.min.js",
                                  "~/Content/Scripts/FrontEnd/owl.carousel.min.js",
                                  "~/Content/Scripts/FrontEnd/main.js",
                                  "~/Content/Scripts/FrontEnd/jquery.number.js",
                                  "~/Content/revolution-slider/js/jquery.themepunch.tools.min.js",
                                  "~/Content/revolution-slider/js/jquery.themepunch.revolution.min.js",
                                  "~/Content/Scripts/DevOrgPlugins/developers-organism.component.js",

                                  "~/Content/Scripts/FrontEnd/front-developer.js",
                                  "~/Content/Scripts/DevOrgPlugins/WeReviewApps.js"

                                  ));
                    #endregion

                    #endregion

                    #region CSS Bundles

                    bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                        "~/Content/css/bootstrap.css",
                                        "~/Content/css/less-imports.css",
                                        "~/Content/css/animate.min.css",
                                        "~/Content/css/font-awesome.min.css",
                                        "~/Content/css/animate-refresh.css",
                                        "~/Content/css/site.css",
                                        "~/Content/css/flags32.css",
                                        "~/Content/css/flags32-combo.css",
                                        "~/Content/css/Upload/jquery.fileupload.css",

                                        "~/Content/css/bootstrap-datetimepicker.css",
                                        "~/Content/css/bootstrap-table.css",
                                        "~/Content/css/bootstrap-select.css",

                                        "~/Content/css/color-fonts.css",
                                        "~/Content/css/star-rating.css",
                                        "~/Content/css/override-mvc.css",

                                        "~/Content/css/base-theme-styles.css",
                                        "~/Content/css/style.css",

                                        "~/Content/css/responsive.css",
                                        "~/Content/revolution-slider/css/settings.css",

                                        "~/Content/css/owl.carousel.css",
                                        "~/Content/css/owl.theme.css",
                                        "~/Content/css/owl.custom.finalize.css",

                                        "~/Content/css/seo-optimize.css",
                                        "~/Content/css/front-developer.css",
                                        "~/Content/css/footer-fixing.css",
                                        "~/Content/css/utilities.css"

                    ));

                    #endregion

                    break;
                case BundleSelector.EveryScriptThroughBundle:
                    #region Java Scripts Bundle

                    #region Validation Bundle & Form Inputs Processing
                    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    "~/Content/Scripts/Bundles/jqueryval.min.js"
                                   ));
                    #endregion


                    #region Upload
                    bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                                   "~/Content/Scripts/Bundles/UploadJs.min.js"
                                   ));
                    #endregion

                    #region Bootstrap
                    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                  "~/Content/Scripts/Bundles/FrontEndJs.min.js"
                                  ));
                    #endregion

                    #endregion

                    #region CSS Bundles

                    bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                  "~/Content/CompactCSS.min.css"

                    ));

                    #endregion
                    break;
                case BundleSelector.EveryScriptGzip:
                    #region Java Scripts Bundle

                    #region Validation Bundle & Form Inputs Processing
                    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    "~/Content/Scripts/Bundles/jqueryval.min.js.gz"
                                   ));
                    #endregion


                    #region Upload
                    bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                                   "~/Content/Scripts/Bundles/UploadJs.min.js.gz"
                                   ));
                    #endregion

                    #region Bootstrap
                    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                  "~/Content/Scripts/Bundles/FrontEndJs.min.js.gz"
                                  ));
                    #endregion

                    #endregion

                    #region CSS Bundles

                    bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                  "~/Content/CompactCSS.min.css.gz"

                    ));

                    #endregion
                    break;

            }




            #region Configs

            bundles.UseCdn = false;
            BundleTable.EnableOptimizations = false;

            #endregion

        }

    }
}
