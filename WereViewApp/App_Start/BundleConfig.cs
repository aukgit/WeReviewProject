using System.Web.Optimization;

namespace WeReviewApp {
    public static class BundleConfig {
        private enum BundleSelector {
            ShowEveryScript,
            EveryScriptThroughBundle
        }
        public static void RegisterBundles(BundleCollection bundles) {


            const BundleSelector bundleSelector = BundleSelector.ShowEveryScript;

            const string jQueryVersion = "2.2.3";
            const string jsFrameworkFolder = "~/JavaScript-Mvc-framework/";
            const string jsLibraryFolder = jsFrameworkFolder + "libs/";
            const string jqueryFolder = jsLibraryFolder + "jQuery/jquery-";
            const string stylesFolder = "~/Content/css/";


            #region CDN Constants

            const string jQueryCdn = @"//code.jquery.com/jquery-" + jQueryVersion + ".min.js";
            //const string respondJsCDN = "http://cdnjs.cloudflare.com/ajax/libs/respond.js/1.4.2/respond.min.js"
            #endregion


            #region jQuery
            
            bundles.Add(new ScriptBundle("~/bundles/jquery", jQueryCdn)
                        .Include(jqueryFolder + jQueryVersion + ".js") //if no CDN
            );
            #endregion


            switch (bundleSelector) {
                case BundleSelector.ShowEveryScript:
                    #region JavaScripts Bundle

                    #region Validation Bundle & Form Inputs Processing
                    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    jsLibraryFolder + "jquery.validate.js",
                                    jsLibraryFolder + "ckeditor.js",
                                    jsLibraryFolder + "ckeditor-jquery-adapter.js",
                                    jsLibraryFolder + "jquery.validate.unobtrusive.js",
                                    jsLibraryFolder + "moment.js",
                                    jsLibraryFolder + "bootstrap-datetimepicker.js",
                                    jsLibraryFolder + "bootstrap-select.js",
                                    jsLibraryFolder + "bootstrap-table.js",
                                    jsLibraryFolder + "bootstrap-table-filter.js",
                                    jsLibraryFolder + "bootstrap-table-export.js",
                                    jsLibraryFolder + "Tag-it/bootstrap3-typeahead.min.js",
                                    jsLibraryFolder + "Tag-it/bootstrap-tagsinput.js",
                                    jsLibraryFolder + "jquery.elastic.source.js",
                                    jsLibraryFolder + "DevOrgPlugins/developers-organism.dynamicSelect.js",
                                    jsLibraryFolder + "DevOrgPlugins/developers-organism.country-phone.js",
                                    jsLibraryFolder + "DevOrgPlugins/jquery.server-validate.js",
                                    jsLibraryFolder + "DevOrgPlugins/dev-component-runner.js",
                                    jsLibraryFolder + "DevOrgPlugins/WeReviewApps.js"

                                   ));
                    #endregion

                    #region Upload
                    bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                                        jsLibraryFolder + "Upload/jquery.ui.widget.js",
                                        jsLibraryFolder + "Upload/load-image.all.min.js",
                                        jsLibraryFolder + "Upload/canvas-to-blob.min.js",
                                        jsLibraryFolder + "Upload/jquery.iframe-transport.js",
                                        jsLibraryFolder + "Upload/jquery.fileupload.js",
                                        jsLibraryFolder + "Upload/jquery.fileupload-process.js",
                                        jsLibraryFolder + "Upload/jquery.fileupload-image.js",
                                        jsLibraryFolder + "Upload/jquery.fileupload-audio.js",
                                        jsLibraryFolder + "Upload/jquery.fileupload-video.js",
                                        jsLibraryFolder + "Upload/jquery.fileupload-validate.js",
                                        jsLibraryFolder + "DevOrgPlugins/developers-organism.upload.js"
                                   ));
                    #endregion

                    #region Bootstrap
                    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                  jsLibraryFolder + "DevOrgPlugins/byId.js",
                                  jsLibraryFolder + "bootstrap.js", // 3.1.2
                                  jsLibraryFolder + "star-rating.js",
                                  jsLibraryFolder + "toastr.js",
                                  jsLibraryFolder + "underscore.js",
                                  jsLibraryFolder + "FrontEnd/wow.js",
                                  jsLibraryFolder + "FrontEnd/jquery.sticky.js",
                                  jsLibraryFolder + "FrontEnd/jquery.stellar.js",
                                  jsLibraryFolder + "FrontEnd/base-theme.js",
                                  jsLibraryFolder + "FrontEnd/jquery.isotope.min.js",
                                  jsLibraryFolder + "FrontEnd/owl.carousel.min.js",
                                  jsLibraryFolder + "FrontEnd/jquery.number.js",
                                  jsLibraryFolder + "revolution-slider/js/jquery.themepunch.tools.min.js",
                                  jsLibraryFolder + "revolution-slider/js/jquery.themepunch.revolution.min.js",
                                  jsLibraryFolder + "DevOrgPlugins/developers-organism.component.js",
                                  jsLibraryFolder + "FrontEnd/front-developer.js",
                                  jsLibraryFolder + "jquery.blockUI.js",

                                  jsFrameworkFolder + "Prototype/Array.js",

                                  jsFrameworkFolder + "app.js",
                                  jsFrameworkFolder + "app.executeBefore.js",
                                  jsFrameworkFolder + "app.executeAfter.js",
                                  jsFrameworkFolder + "jQueryCaching.js",
                                  jsFrameworkFolder + "attachInitialize.js",

                                  jsFrameworkFolder + "schema/schema.js",
                                  jsFrameworkFolder + "schema/hashset.js",
                                  jsFrameworkFolder + "schema/url.js",

                                  jsFrameworkFolder + "extensions/ajax.js",
                                  jsFrameworkFolder + "extensions/hiddenContainer.js",
                                  jsFrameworkFolder + "extensions/clone.js",
                                  jsFrameworkFolder + "extensions/constants.js",
                                  jsFrameworkFolder + "extensions/initialize.js",
                                  jsFrameworkFolder + "extensions/inputChangeTracker.js",
                                  jsFrameworkFolder + "extensions/modal.js",
                                  jsFrameworkFolder + "extensions/pagination.js",
                                  jsFrameworkFolder + "extensions/regularExp.js",
                                  jsFrameworkFolder + "extensions/selectors.js",
                                  jsFrameworkFolder + "extensions/spinner.js",
                                  jsFrameworkFolder + "extensions/urls.js",

                                  jsFrameworkFolder + "controllers/controllers.js",
                                  jsFrameworkFolder + "controllers/initialize.js",
                                  jsFrameworkFolder + "controllers/AppController.js",

                                  jsFrameworkFolder + "component/component.js",
                                  jsFrameworkFolder + "component/component.list.js",

                                  jsFrameworkFolder + "areas/admin/controllers/NavItemsController.js",

                                  jsFrameworkFolder + "jQueryExtend.js",
                                  jsFrameworkFolder + "jQueryExtend.fn.js",
                                  jsFrameworkFolder + "app.config.js",
                                  jsFrameworkFolder + "app.global.js",
              
                                  jsFrameworkFolder + "app.run.js"

                                  ));
                    #endregion

                    #endregion

                    #region CSS Bundles

                    bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                                        stylesFolder + "bootstrap.css",
                                        stylesFolder + "less-imports.css",
                                        stylesFolder + "animate.min.css",
                                        stylesFolder + "font-awesome.min.css",
                                        stylesFolder + "site.css",
                                        stylesFolder + "header.css",
                                        stylesFolder + "flags32.css",
                                        stylesFolder + "flags32-combo.css",
                                        stylesFolder + "Upload/jquery.fileupload.css",

                                        stylesFolder + "bootstrap-datetimepicker.css",
                                        stylesFolder + "bootstrap-table.css",
                                        stylesFolder + "bootstrap-select.css",
                                        stylesFolder + "bootstrap-select-overrides.css",
                                        stylesFolder + "bootstrap-tagsinput.css",
                                        //stylesFolder + "ckedit-skin-bootstrap.css",

                                        stylesFolder + "color-fonts.css",
                                        stylesFolder + "loader-spinner.css",
                                        stylesFolder + "star-rating.css",
                                        stylesFolder + "toastr.css",
                                        stylesFolder + "override-mvc.css",
                                        stylesFolder + "validator.css",
                                        stylesFolder + "editor-templates.css",

                                        stylesFolder + "base-theme-styles.css",
                                        stylesFolder + "style.css",

                                        stylesFolder + "responsive.css",

                                        jsLibraryFolder + "revolution-slider/css/settings.css",

                                        stylesFolder + "owl.carousel.css",
                                        stylesFolder + "owl.theme.css",
                                        stylesFolder + "owl.custom.finalize.css",

                                        stylesFolder + "seo-optimize.css",
                                        stylesFolder + "front-developer.css",
                                        stylesFolder + "footer-fixing.css",
                                        stylesFolder + "utilities.css"

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

            }




            #region Configs

            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = false;

            #endregion

        }

    }
}
