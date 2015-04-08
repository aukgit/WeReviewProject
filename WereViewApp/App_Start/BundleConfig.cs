using System.Web;
using System.Web.Optimization;

namespace WereViewApp {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {

            #region CDN Constants
            //const string jQueryCDN = "http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js";
            const string jQueryCDN = "http://code.jquery.com/jquery-latest.min.js";
            const string mordernizerCDN = "http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js";
            //const string respondJsCDN = "http://cdnjs.cloudflare.com/ajax/libs/respond.js/1.4.2/respond.min.js"
            #endregion

            #region Java Scripts Bundle

            #region jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery", jQueryCDN)
                        .Include("~/Scripts/jquery-2.1.1.min.js") //if no CDN
                );
            #endregion

            #region Validation Bundle & Form Inputs Processing
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                           "~/Content/Scripts/jquery.validate*",
                            "~/Scripts/moment.js",
                            "~/Scripts/bootstrap-datetimepicker.js",
                            "~/Scripts/bootstrap-select.js",
                            "~/Content/Scripts/bootstrap-table.js",
                            "~/Content/Scripts/bootstrap-table-filter.js",
                            "~/Content/Scripts/bootstrap-table-export.js",
                            "~/Content/Scripts/jquery.elastic.source.js",
                            "~/Scripts/CustomScripts/DevOrgComponent.js",
                            "~/Scripts/CustomScripts/CustomJS.js"
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
                                "~/Content/Scripts/Upload/bootstrap-progressbar.js",
                                "~/Content/Scripts/Upload/devOrgUploadConfig.js"
                           ));
            #endregion

            #region Mordernizer
            bundles.Add(new ScriptBundle("~/bundles/modernizr", mordernizerCDN)
                    .Include("~/Content/Scripts/modernizr-*") //if no CDN
            );
            #endregion

            #region Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Content/Scripts/bootstrap.js", // 3.1.2
                          "~/Content/Scripts/respond.js",
                          "~/Content/Scripts/star-rating.js",
                           "~/Scripts/underscore.js",
                          "~/Scripts/CustomScripts/CommonJsEveryPage.js"

                          ));
            #endregion

            #endregion

            #region CSS Bundles
            bundles.Add(new StyleBundle("~/Content/css/bootstrap_red").Include(
                                "~/Content/css/bootstrap.theme.unitied.css"

            ));
            bundles.Add(new StyleBundle("~/Content/css/FrontEndDeveloperCSS/bootstrap").Include(
                                "~/Content/css/frontEnd-bootstrap.min.css",
                                "~/Content/css/FrontEndDeveloperCSS/main.css",
                                "~/Content/css/FrontEndDeveloperCSS/menu.css",
                                "~/Content/css/FrontEndDeveloperCSS/media.css",
                                "~/Content/css/FrontEndDeveloperCSS/nivo-lightbox.css",
                                "~/Content/css/FrontEndDeveloperCSS/nivo-lightbox-extra.css"

            ));

            //bundles.Add(new StyleBundle("~/Content/css/FrontEndDeveloperCSS/bootstrap").Include(
            //                    "~/Content/css/FrontEndDeveloperCSS/FrontEndMinifiedCSS.css"

            //));
            bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                "~/Content/css/site.css",
                                "~/Content/css/flags32.css",
                                "~/Content/css/Upload/jquery.fileupload.css",
                                "~/Content/css/Upload/style.css",
                                "~/Content/css/flags32-combo.css",
                                "~/Content/css/bootstrap-datetimepicker.css",
                                "~/Content/css/bootstrap-table.css",
                                "~/Content/css/color-fonts.css",
                                "~/Content/css/font-awesome.css",
                                "~/Content/css/star-rating.css",
                                "~/Content/css/Upload/jquery.fileupload.css",
                                "~/Content/css/miscellaneous.css",
                                "~/Content/css/bootstrap-select.css",
                                "~/Content/css/overridecss.css"
            ));

            #endregion

            //#region Tagit CSS Bundles

            //bundles.Add(new StyleBundle("~/Content/css/tagit").Include(
            //                   "~/Content/css/Tagit/ui-lightness/jquery-ui-1.9.2.custom.min.css",
            //                   "~/Content/css/Tagit/jquery.tagedit.css"

            //));
            //#endregion

            //#region Tag It JavaScipt
            //bundles.Add(new ScriptBundle("~/bundles/tagit").Include(
            //                "~/Content/Scripts/Tagit/jquery-ui-1.9.2.custom.min.js",
            //                "~/Content/Scripts/Tagit/jquery.autoGrowInput.js",
            //                "~/Content/Scripts/Tagit/jquery.tagedit.js"
            //));
            //#endregion

            #region WereViewApp Js
            bundles.Add(new ScriptBundle("~/bundles/WereViewApps").Include(

                           "~/Content/Scripts/FrontEnd/nivo-lightbox.min.js",
                           "~/Content/Scripts/FrontEnd/jquery.number.js",
                           "~/Scripts/CustomScripts/WereViewApps.js"
            ));
            #endregion



            #region Configs

            bundles.UseCdn = false;
            BundleTable.EnableOptimizations = false;

            #endregion

        }

    }
}
