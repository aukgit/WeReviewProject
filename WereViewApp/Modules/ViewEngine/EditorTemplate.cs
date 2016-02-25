using System.Web.Routing;

namespace WereViewApp.Modules.ViewEngine {
    public static class EditorTemplate {
        public static void CommonTemplateAssignments(string templateName, string additionalClass, dynamic viewBag, ref RouteValueDictionary htmlAttributes, string typeName = null) {
            var isRequired = viewBag.ModelMetadata.IsRequired;
            string
                displayName = viewBag.ModelMetadata.DisplayName ?? viewBag.ModelMetadata.PropertyName,
                isRequiredStar = isRequired ? "*" : "",
                prop = viewBag.ModelMetadata.PropertyName.ToString(),
                propLower = prop.ToLower(),
                styleProperty = propLower;

            if (displayName == null) {
                displayName = viewBag.ModelMetadata.PropertyName;
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
                viewBag.ModelMetadata.Description = viewBag.description;
            }
            htmlAttributes.Add("title", htmlAttributes["placeholder"]);
            htmlAttributes.Add("data-prop", styleProperty);
            htmlAttributes.Add("data-template", templateName);
        }

    }
}