using System.Web.Optimization;

namespace WeReviewApp {
    public static class BundleConfig {
        private enum BundleSelector {
            ShowEveryScript,
            EveryScriptThroughBundle,
            EveryScriptGzip
        }
        public static void RegisterBundles(BundleCollection bundles) {

            #region CDN Constants

            const string jQueryVersion = "2.1.4";
            const string jQueryCdn = @"//code.jquery.com/jquery-" + jQueryVersion + ".min.js";
            //const string respondJsCDN = "http://cdnjs.cloudflare.com/ajax/libs/respond.js/1.4.2/respond.min.js"
            #endregion

            const BundleSelector bundleSelector = BundleSelector.ShowEveryScript;

            #region jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery", jQueryCdn)
                        .Include("~/Content/Scripts/jquery-" + jQueryVersion + ".js") //if no CDN
            );
            #endregion


            switch (bundleSelector) {
                case BundleSelector.ShowEveryScript:
                    #region JavaScripts Bundle

                    #region Validation Bundle & Form Inputs Processing
                    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    "~/Content/Scripts/jquery.validate.js",
                                    "~/Content/Scripts/ckeditor.js",
                                    "~/Content/Scripts/ckeditor-jquery-adapter.js",
                                    "~/Content/Scripts/jquery.validate.unobtrusive.js",
                                    "~/Content/Scripts/moment.js",
                                    "~/Content/Scripts/bootstrap-datetimepicker.js",
                                    "~/Content/Scripts/bootstrap-select.js",
                                    "~/Content/Scripts/bootstrap-table.js",
                                    "~/Content/Scripts/bootstrap-table-filter.js",
                                    "~/Content/Scripts/bootstrap-table-export.js",
                                    "~/Content/Scripts/Tag-it/bootstrap3-typeahead.min.js",
                                    "~/Content/Scripts/Tag-it/bootstrap-tagsinput.js",
                                    "~/Content/Scripts/jquery.elastic.source.js",
                                    "~/Content/Scripts/DevOrgPlugins/developers-organism.dynamicSelect.js",
                                    "~/Content/Scripts/DevOrgPlugins/developers-organism.country-phone.js",
                                    "~/Content/Scripts/DevOrgPlugins/jquery.server-validate.js",
                                    "~/Content/Scripts/DevOrgPlugins/dev-component-runner.js",
                                    "~/Content/Scripts/DevOrgPlugins/WeReviewApps.js"

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
                                  "~/Content/Scripts/toastr.js",
                                  "~/Content/Scripts/underscore.js",
                                  "~/Content/Scripts/FrontEnd/wow.js",
                                  "~/Content/Scripts/FrontEnd/jquery.sticky.js",
                                  "~/Content/Scripts/FrontEnd/jquery.stellar.js",
                                  "~/Content/Scripts/FrontEnd/base-theme.js",
                                  "~/Content/Scripts/FrontEnd/jquery.isotope.min.js",
                                  "~/Content/Scripts/FrontEnd/owl.carousel.min.js",
                                  "~/Content/Scripts/FrontEnd/jquery.number.js",
                                  "~/Content/revolution-slider/js/jquery.themepunch.tools.min.js",
                                  "~/Content/revolution-slider/js/jquery.themepunch.revolution.min.js",
                                  "~/Content/Scripts/DevOrgPlugins/developers-organism.component.js",

                                  "~/Content/Scripts/jquery.blockUI.js",

                                  "~/JavaScript-Mvc-framework/Prototype/Array.js",

                                  "~/JavaScript-Mvc-framework/app.js",
                                  "~/JavaScript-Mvc-framework/app.executeBefore.js",
                                  "~/JavaScript-Mvc-framework/app.executeAfter.js",
                                  "~/JavaScript-Mvc-framework/jQueryCaching.js",
                                  "~/JavaScript-Mvc-framework/attachInitialize.js",

                                  "~/JavaScript-Mvc-framework/schema/schema.js",
                                  "~/JavaScript-Mvc-framework/schema/hashset.js",
                                  "~/JavaScript-Mvc-framework/schema/url.js",

                                  "~/JavaScript-Mvc-framework/extensions/ajax.js",
                                  "~/JavaScript-Mvc-framework/extensions/hiddenContainer.js",
                                  "~/JavaScript-Mvc-framework/extensions/clone.js",
                                  "~/JavaScript-Mvc-framework/extensions/constants.js",
                                  "~/JavaScript-Mvc-framework/extensions/initialize.js",
                                  "~/JavaScript-Mvc-framework/extensions/inputChangeTracker.js",
                                  "~/JavaScript-Mvc-framework/extensions/modal.js",
                                  "~/JavaScript-Mvc-framework/extensions/pagination.js",
                                  "~/JavaScript-Mvc-framework/extensions/regularExp.js",
                                  "~/JavaScript-Mvc-framework/extensions/selectors.js",
                                  "~/JavaScript-Mvc-framework/extensions/spinner.js",
                                  "~/JavaScript-Mvc-framework/extensions/urls.js",

                                  "~/JavaScript-Mvc-framework/controllers/controllers.js",
                                  "~/JavaScript-Mvc-framework/controllers/initialize.js",
                                  "~/JavaScript-Mvc-framework/controllers/AppController.js",

                                  "~/JavaScript-Mvc-framework/areas/admin/controllers/NavItemsController.js",

                                  "~/JavaScript-Mvc-framework/jQueryExtend.js",
                                  "~/JavaScript-Mvc-framework/jQueryExtend.fn.js",
                                  "~/JavaScript-Mvc-framework/app.config.js",
                                  "~/JavaScript-Mvc-framework/app.global.js",
              
                                  "~/JavaScript-Mvc-framework/app.run.js"

                                  ));
                    #endregion

                    #endregion

                    #region CSS Bundles

                    bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                        "~/Content/css/bootstrap.css",
                                        "~/Content/css/less-imports.css",
                                        "~/Content/css/animate.min.css",
                                        "~/Content/css/font-awesome.min.css",
                                        "~/Content/css/site.css",
                                        "~/Content/css/header.css",
                                        "~/Content/css/flags32.css",
                                        "~/Content/css/flags32-combo.css",
                                        "~/Content/css/Upload/jquery.fileupload.css",

                                        "~/Content/css/bootstrap-datetimepicker.css",
                                        "~/Content/css/bootstrap-table.css",
                                        "~/Content/css/bootstrap-select.css",
                                        "~/Content/css/bootstrap-tagsinput.css",
                                        //"~/Content/css/ckedit-skin-bootstrap.css",

                                        "~/Content/css/color-fonts.css",
                                        "~/Content/css/loader-spinner.css",
                                        "~/Content/css/star-rating.css",
                                        "~/Content/css/toastr.css",
                                        "~/Content/css/override-mvc.css",
                                        "~/Content/css/validator.css",
                                        "~/Content/css/editor-templates.css",

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
                                    "~/Content/Published.Scripts/jqueryval.min.js"
                                   ));
                    #endregion


                    #region Upload
                    bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                                   "~/Content/Published.Scripts/upload.min.js"
                                   ));
                    #endregion

                    #region Bootstrap
                    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                  "~/Content/Published.Scripts/front-end.min.js"
                                  ));
                    #endregion

                    #endregion

                    #region CSS Bundles

                    bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                  "~/Content/Published.Styles/Styles.min.css"

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

            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = false;

            #endregion

        }

    }
}
