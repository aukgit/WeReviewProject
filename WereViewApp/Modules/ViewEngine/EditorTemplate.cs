using System.Web.Routing;
using System.Web.Mvc;

namespace WereViewApp.Modules.ViewEngine {
    internal static class EditorTemplate {
        internal static void CommonTemplateAssignments(string templateName, string additionalClass, DynamicViewDataDictionary viewBag, ref RouteValueDictionary htmlAttributes, string typeName = null) {
            var meta = (viewBag.ViewData).ModelMetadata as CachedDataAnnotationsModelMetadata;
            if (meta != null) {
                var isRequired = meta.IsRequired;
                string
                    displayName = meta.DisplayName ?? meta.PropertyName,
                    isRequiredStar = isRequired ? "*" : "",
                    prop = meta.PropertyName.ToString(),
                    propLower = prop.ToLower(),
                    styleProperty = propLower;

                if (displayName == null) {
                    displayName = meta.PropertyName;
                }
                if (viewBag.@class != null) {
                    htmlAttributes.Add("class", "form-control " + additionalClass + " " + viewBag.@class);
                } else {
                    htmlAttributes.Add("class", "form-control " + additionalClass);
                }

                if (viewBag.type != null) {
                    htmlAttributes.Add("type", viewBag.@type);
                } else if (!string.IsNullOrEmpty(typeName)) {
                    htmlAttributes.Add("type", viewBag.@type);
                }

                if (viewBag.placeholder != null) {
                    htmlAttributes.Add("placeholder", viewBag.placeholder);
                } else {
                    if (isRequired) {
                        htmlAttributes.Add("placeholder", displayName + isRequiredStar);
                    } else {
                        htmlAttributes.Add("placeholder", "[" + displayName + "]");
                    }
                }
                if (viewBag.Value != null) {
                    htmlAttributes.Add("Value", viewBag.Value);
                }
                if (viewBag.label != null) {
                    displayName = viewBag.label;
                }

                if (viewBag.labelColumn == null) {
                    viewBag.labelColumn = "col-md-2";
                }
                if (viewBag.textColumn == null) {
                    viewBag.textColumn = "col-md-10";
                }

                if (viewBag.description != null) {
                    meta.Description = viewBag.description;
                }
                htmlAttributes.Add("title", htmlAttributes["placeholder"]);
                htmlAttributes.Add("data-prop", styleProperty);
                htmlAttributes.Add("data-template", templateName);
            }
        }

    }
}