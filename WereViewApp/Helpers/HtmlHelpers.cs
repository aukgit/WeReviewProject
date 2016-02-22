using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DevMvcComponent.Enums;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.Cache;
using WereViewApp.Modules.Constants;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Mail;
using WereViewApp.Modules.Menu;
using WereViewApp.Modules.TimeZone;
using WereViewApp.Modules.Uploads;

namespace WereViewApp.Helpers {
    public static class HtmlHelpers {
        private const string Selected = "selected='selected'";
        public static int TruncateLength = AppConfig.TruncateLength;
        /// <summary>
        /// Returns BaseUrl and slash.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequestBase request) {
            if (request.Url == null) {
                return "";
            }
            return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }
        /// <summary>
        /// Returns BaseUrl and slash.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpContext context) {
            if (context != null) {
                var request = context.Request;
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
            }
            return "";
        }
        #region Icons generate : badge

        public static HtmlString GetBadge(this HtmlHelper helper, long number) {
            var markup = string.Format(@"<span class='badge'>{0}</span>", number);

            return new HtmlString(markup);
        }

        #endregion

        #region Generate Navigation

        public static HtmlString GetMenu(this HtmlHelper helper, string menuName, bool isDependOnUserLogState = false) {
            var cacheName = menuName + "-menu-";
            if (isDependOnUserLogState && UserManager.IsAuthenticated()) {
                cacheName += UserManager.GetCurrentUserName();
            }
            var cache = (string)AppConfig.Caches.Get(cacheName);

            if (cache != null && !string.IsNullOrWhiteSpace(cache)) {
                return new HtmlString(cache);
            }

            using (var menuGenerator = new GenerateMenu()) {
                var menuItems = menuGenerator.GetMenuItem(menuName);

                if (menuItems != null && menuItems.NavigationItems != null) {
                    var items = menuItems.NavigationItems.ToList();
                    var menuListItems = menuGenerator.GenerateRecursiveMenuItems(items);
                    // keeping cache
                    AppConfig.Caches.Set(cacheName, menuListItems);
                    return new HtmlString(menuListItems);
                }
            }
            return new HtmlString("");
        }

        #endregion

        #region List, Item Generate / Route Generates

        public static HtmlString RouteListItemGenerate(this HtmlHelper helper, string area, string display,
            string controller, string currentController) {
            var addClass = " class='active' ";
            if (controller != currentController)
                addClass = "";
            var markup = string.Format("<li{0}><a href='{1}'>{2}</a></li>", addClass, "/" + area + "/" + controller,
                display);
            //return  new HtmlString(markup);
            return new HtmlString(markup);
        }

        #endregion

        #region Confirming Buttons
        /// <summary>
        /// Confirms before submit.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="alertMessage"></param>
        /// <returns></returns>
        public static HtmlString ConfirmingSubmitButton(this HtmlHelper helper, string buttonName = "Save",
            string alertMessage = "Are you sure about this action?") {
            var sendbtn = String.Format(
                "<input type=\"submit\" value=\"{0}\" onClick=\"return confirm('{1}');\" />",
                buttonName, alertMessage);
            return new HtmlString(sendbtn);
        }

        /// <summary>
        /// Renders a submit button with icon.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="alertMessage"></param>
        /// <param name="iconClass"></param>
        /// <param name="btnType"></param>
        /// <param name="placeIconLeft"></param>
        /// <param name="additionalClasses"></param>
        /// <returns></returns>
        public static HtmlString SubmitButton(this HtmlHelper helper, string buttonName = "Submit",
            string iconClass = "fa fa-floppy-o",
            string tooltip = "",
            string additionalClasses = "btn btn-success",
            bool placeIconLeft = true,
            string btnType = "Submit"
            ) {
            const string iconFormt = "<i class=\"{0}\"></i>";
            string leftIcon = "",
                   rightIcon = "";
            if (placeIconLeft) {
                leftIcon = string.Format(iconFormt, iconClass);
            } else {
                rightIcon = string.Format(iconFormt, iconClass);
            }
            var buttonHtml = String.Format(
                "<button type=\"{0}\"  title=\"{1}\" class=\"{2}\">{3} {4} {5}</button>",
                btnType, tooltip, additionalClasses, leftIcon, buttonName, rightIcon);
            return new HtmlString(buttonHtml);
        }

        public static HtmlString SubmitButtonIconRight(this HtmlHelper helper, string buttonName = "Submit",
           string iconClass = "fa fa-floppy-o",
           string tooltip = "",
           string additionalClasses = "btn btn-success",
           bool placeIconLeft = false,
           string btnType = "Submit"
           ) {
            return SubmitButton(helper, buttonName,
                iconClass,
                tooltip,
                additionalClasses,
                placeIconLeft,
                btnType);
        }

        public static HtmlString EmailButtonIconRight(this HtmlHelper helper, string buttonName = "Send",
          string iconClass = Icons.EmailO,
          string tooltip = "",
          string additionalClasses = "btn btn-success",
          bool placeIconLeft = false,
          string btnType = "Submit"
          ) {
            return SubmitButton(helper, buttonName,
                iconClass,
                tooltip,
                additionalClasses,
                placeIconLeft,
                btnType);
        }
        /// <summary>
        /// Renders a remove button with icon.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="alertMessage"></param>
        /// <param name="iconClass"></param>
        /// <param name="btnType"></param>
        /// <param name="placeIconLeft"></param>
        /// <param name="additionalClasses"></param>
        /// <returns></returns>
        public static HtmlString RemoveButton(this HtmlHelper helper, string buttonName = "Delete",
            string iconClass = "fa fa-times",
            string tooltip = "",
            string btnType = "Submit",
            bool placeIconLeft = true,
            string additionalClasses = "btn btn-danger") {
            return SubmitButton(helper, buttonName, iconClass, tooltip, btnType, placeIconLeft, additionalClasses);
        }
        #endregion

        #region jQueryMobile Options

        /// <summary>
        ///     JqueryMobile BackButton
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static HtmlString BackButton(this HtmlHelper helper, string buttonName = "Back", bool isMini = false,
            string icon = "arrow-l") {
            var mini = (isMini)
                ? "data-mini='true'"
                : "";
            var backbtn = "<a href='#' data-role='button' class = 'back-button' data-rel='back' data_icon='" + icon +
                          "' " + mini + " >" + buttonName + "</a>";
            return new HtmlString(backbtn);
        }

        #endregion

        #region Drop Downs:  General, Country

        #region Country
        /// <summary>
        /// </summary>
        /// <param name="countries"></param>
        /// <param name="classes">use  spaces to describe the classes</param>
        /// <param name="otherAttributes"></param>
        /// <returns></returns>
        public static string DropDownCountry(List<Country> countries, string classes = "",
            string otherAttributes = "", string contentAddedString = "") {
            var countryOptionsGenerate = "<select class='form-control selectpicker " + classes +
                                         " country-combo' data-live-search='true' name='CountryID' " + otherAttributes +
                                         " title='Country' data-style='btn-success flag-combo fc-af'>";
            var sb = new StringBuilder(countryOptionsGenerate, countries.Count * 7);
            foreach (var country in countries) {
                sb.Append(string.Format("<option class='flag-country-combo flag {0}' title='| {1}' value='{2}'>",
                    country.Alpha2Code.ToLower(), country.DisplayCountryName, country.CountryID));
                sb.Append(contentAddedString);
                //sb.Append();
                sb.Append(country.DisplayCountryName);
                sb.Append("</option>").AppendLine();
            }
            sb.AppendLine("</select>");
            return sb.ToString();
        }
        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="countries"></param>
        /// <param name="classes">use  spaces to describe the classes</param>
        /// <param name="otherAttributes"></param>
        /// <returns></returns>
        public static HtmlString DropDownCountry(this HtmlHelper helper, List<Country> countries, string classes = "",
            string otherAttributes = "", string contentAddedString = "") {
            var strHtml = DropDownCountry(countries, classes, otherAttributes, contentAddedString);
            return new HtmlString(strHtml);
        }

        #endregion

        #region General DropDowns

        public static HtmlString DropDowns(this HtmlHelper helper, string valueField, string textField,
            string htmlName = null, string displayName = null, string modelValue = null, string isRequried = "*",
            string classes = null, string toolTipValue = null, string otherAttributes = "", string tableName = null,
            AppVar.ConnectionStringType connectionType = AppVar.ConnectionStringType.DefaultConnection) {
            var divElement = @"<div class='form-group {0}-main'>
                             <div class='controls'>
                                <label class='col-md-2 control-label' for='{0}'>{1}<span class='red '>{2}</span></label>
                                <div class='col-md-10 {0}-combo-div'>
                                    {3}
                                    <a href='#' data-toggle='tooltip' data-original-title='{4}' title='{4}' class='tooltip-show'><span class='glyphicon glyphicon-question-sign font-size-22 glyphicon-top-fix almost-white'></span></a>
                                </div>
                            </div>
                        </div>";
            // 0- name
            // 1- displayName
            // 2 - isRequried
            // 3 - select element
            // 4 - tooltipValue
            if (tableName == null) {
                tableName = valueField.Replace("ID", "");
            }
            if (modelValue == null) {
                modelValue = "";
            }
            if (classes == null) {
                classes = "btn btn-success";
            }
            if (displayName == null) {
                displayName = textField;
            }
            if (toolTipValue == null) {
                toolTipValue = "Please select " + displayName;
            }
            if (htmlName == null) {
                htmlName = valueField;
            }
            var selected = "";
            var countryOptionsGenerate = "<select class='form-control selectpicker " + classes +
                                         "' data-live-search='true' name='" + htmlName + "' " + otherAttributes +
                                         " title='Choose...' data-style='" + classes + "'>";
            var dt = CachedQueriedData.GetTable(tableName, connectionType, new[] { valueField, textField });
            if (dt != null && dt.Rows.Count > 0) {
                var sb = new StringBuilder(countryOptionsGenerate, dt.Rows.Count + 10);
                DataRow row;
                for (var i = 0; i < dt.Rows.Count; i++) {
                    row = dt.Rows[i];
                    if (row[valueField].Equals(modelValue)) {
                        selected = Selected;
                    }
                    sb.Append(string.Format("<option value='{0}' {1} {2}>{2}</option>", row[valueField], selected,
                        row[textField]));
                }
                sb.AppendLine("</select>");
                var complete = string.Format(divElement, htmlName, displayName, isRequried, sb, toolTipValue);

                return new HtmlString(complete);
            }
            return new HtmlString("");
        }

        #endregion

        #endregion

        #region Truncates

        public static string Truncate(this HtmlHelper helper, string input, int? length, bool isShowElipseDot = true) {
            if (string.IsNullOrEmpty(input))
                return "";
            if (length == null) {
                length = TruncateLength;
            }
            if (input.Length <= length) {
                return input;
            }
            if (isShowElipseDot) {
                return input.Substring(0, (int)length) + "...";
            }
            return input.Substring(0, (int)length);
        }

        public static string Truncate(this HtmlHelper helper, string input, int starting, int length) {
            if (string.IsNullOrEmpty(input))
                return "";
            if (length == -1) {
                length = input.Length;
            }
            if (input.Length <= length) {
                length = input.Length;
            }
            length = length - starting;
            if (input.Length < starting) {
                return "";
            }
            return input.Substring(starting, length);
        }

        public static bool IsTruncateNeeded(this HtmlHelper helper, string input, int mid) {
            if (string.IsNullOrEmpty(input))
                return false;
            if (input.Length > mid) {
                return false;
            }
            return true;
        }

        #endregion

        #region Link Generates

        public static HtmlString ContactFormActionLink(this HtmlHelper helper, string linkName, string title,
            string addClass = "") {
            var markup = string.Format(MailHtml.ContactUsLink, title, linkName, addClass, AppVar.Url);
            return new HtmlString(markup);
        }

        public static HtmlString SamePageLink(this HtmlHelper helper, string linkName, string title, bool h1 = true,
            string addClass = "") {
            //var area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
            //var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            //var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];
            return SamePageLinkWithIcon(helper, linkName, title, null, h1, addClass);
        }

        /// <summary>
        /// Generates same page url anchor with an icon left or right
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkName">Link display name</param>
        /// <param name="title">Link tooltip title</param>
        /// <param name="iconClass">Icon classes: Font-awesome icons or bootstrap icon classes</param>
        /// <param name="h1">Is nested inside a H1 element (W3c valid).</param>
        /// <param name="addClass">Anchor class</param>
        /// <param name="isLeft"></param>
        /// <returns></returns>
        public static HtmlString SamePageLinkWithIcon(this HtmlHelper helper,
            string linkName,
            string title,
            string iconClass,
            bool h1 = true,
            string addClass = "",
            bool isLeft = true) {
            var markup = "";
            var uri = HttpContext.Current.Request.RawUrl;
            uri = AppVar.Url + uri;

            var icon = "";
            if (!string.IsNullOrEmpty(iconClass)) {
                icon = string.Format("<i class='{0}'></i>", iconClass);
            }
            if (isLeft) {
                //left icon
                markup = string.Format("<div class='header-margin-space-type-1'><a href='{0}' class='{1}' title='{2}'>{4} {3}</a></div>", uri, addClass, title, linkName, icon);
            } else {
                //right icon
                markup = string.Format("<div class='header-margin-space-type-1'><a href='{0}' class='{1}' title='{2}'>{3} {4}</a></div>", uri, addClass, title, linkName, icon);
            }
            if (h1) {
                markup = string.Format("<h1 title='{0}' class='h3'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        /// <summary>
        /// Generates same page url anchor with an icon left or right
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkDisplayName">Link display name</param>
        /// <param name="routeValues"></param>
        /// <param name="title">Link tooltip title</param>
        /// <param name="iconClass">Icon classes: Font-awesome icons or bootstrap icon classes</param>
        /// <param name="h1">Is nested inside a H1 element (W3c valid).</param>
        /// <param name="addClass">Anchor class</param>
        /// <param name="isLeft"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static HtmlString ActionLinkWithIcon(this HtmlHelper helper,
            string linkDisplayName,
            string actionName,
            string controllerName,
            object routeValues,
            string title,
            string iconClass,
            string addClass = "",
            bool h1 = false,
            bool isLeft = true) {
            var markup = "";
            string uri = "";
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (!string.IsNullOrWhiteSpace(controllerName)) {
                uri = urlHelper.Action(actionName, controllerName, routeValues);
            } else if (routeValues != null) {
                uri = urlHelper.Action(actionName, routeValues);
            } else {
                uri = urlHelper.Action(actionName);
            }
            uri = AppVar.Url + uri;

            var icon = "";
            if (!string.IsNullOrEmpty(iconClass)) {
                icon = string.Format("<i class='{0}'></i>", iconClass);
            }
            if (isLeft) {
                //left icon
                markup = string.Format("<a href='{0}' class='{1}' title='{2}'>{4} {3}</a>", uri, addClass, title, linkDisplayName, icon);
            } else {
                //right icon
                markup = string.Format("<a href='{0}' class='{1}' title='{2}'>{3} {4}</a>", uri, addClass, title, linkDisplayName, icon);
            }
            if (h1) {
                markup = string.Format("<h1 title='{0}' class='h3'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }
        /// <summary>
        /// Returns url without the host name. 
        /// Slash is included
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>Returns url without the host name.</returns>
        public static string GetCurrentUrlString(this HtmlHelper helper) {
            return HttpContext.Current.Request.RawUrl;
        }
        /// <summary>
        /// Returns url whole page url with the host name. 
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>Returns url whole page url with the host name. </returns>
        public static string GetCurrentUrlWithHostName(this HtmlHelper helper) {
            return AppVar.Url + HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkName">null gives the number on the display</param>
        /// <param name="title"></param>
        /// <param name="number"></param>
        /// <param name="h1"></param>
        /// <param name="addClass"></param>
        /// <returns></returns>
        public static HtmlString PhoneNumberLink(this HtmlHelper helper, long phonenumber, string title, bool h1 = false,
            string addClass = "") {
            var phone = "+" + phonenumber;

            var markup = string.Format("<a href='tel:{0}' class='{1}' title='{2}'>{3}</a>", phone, addClass, title,
                phone);

            if (h1) {
                markup = string.Format("<h1 title='{0}'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkName">null gives the number on the display</param>
        /// <param name="title"></param>
        /// <param name="number"></param>
        /// <param name="h1"></param>
        /// <param name="addClass"></param>
        /// <returns></returns>
        public static HtmlString PhoneNumberLink(this HtmlHelper helper, string phonenumber, string title,
            bool h1 = false, string addClass = "") {
            var phone = "+" + phonenumber;

            var markup = string.Format("<a href='tel:{0}' class='{1}' title='{2}'>{3}</a>", phone, addClass, title,
                phone);

            if (h1) {
                markup = string.Format("<h1 title='{0}'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        public static HtmlString EmailLink(this HtmlHelper helper, string email, string title, bool h1 = false,
            string addClass = "") {
            var markup = string.Format("<a href='mailto:{0}' class='{1}' title='{2}'>{3}</a>", email, addClass, title,
                email);

            if (h1) {
                markup = string.Format("<h1 title='{0}'><strong title='" + title + "'>{1}</strong></h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        #endregion

        #region Generate Publisher, Ideas, Tags

        #endregion

        #region Image Generates

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="addtionalRootPath"></param>
        /// <param name="file"></param>
        /// <param name="isPrivate"></param>
        /// <param name="asTemp"></param>
        /// <param name="tempString"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public static string UploadedImageSrc(this HtmlHelper helper, string addtionalRootPath, IUploadableFile file,
            bool isPrivate = false, bool asTemp = false, string tempString = "_temp",
            string rootPath = "~/Uploads/Images/") {
            if (isPrivate) {
                rootPath += "Private/";
            }
            rootPath += addtionalRootPath;
            if (!asTemp) {
                tempString = "";
            }
            var fileName = file.UploadGuid + "-" + file.Sequence + tempString + "." + file.Extension;

            var path = string.Format("{0}{1}", rootPath, fileName);
            return AppVar.Url + VirtualPathUtility.ToAbsolute(path);
            //return (markup);
        }

        public static string GetOrganizeName(IUploadableFile file, bool includeExtention = false, bool asTemp = false,
            string tempString = "_temp") {
            var ext = "";
            if (!asTemp) {
                tempString = "";
            }
            if (includeExtention) {
                ext = "." + file.Extension;
            }
            return file.UploadGuid + "-" + file.Sequence + tempString + ext;
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">use absolute http url for image src.</param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static HtmlString ImageFromAbsolutePath(this HtmlHelper helper, string src, string alt) {
            var markup = string.Format("<img src='{0}' alt='{1}'/>", src, alt);
            return new HtmlString(markup);
            //return (markup);
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">relative url.</param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static HtmlString Image(this HtmlHelper helper, string src, string alt) {
            var markup = string.Format("<img src='{0}' alt='{1}'/>", VirtualPathUtility.ToAbsolute(src), alt);
            return new HtmlString(markup);
            //return (markup);
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="folder"></param>
        /// <param name="src">relative from folder</param>
        /// <param name="ext"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static HtmlString Image(this HtmlHelper helper, string folder, string src, string ext, string alt) {
            var markup = string.Format("<img src='{0}{1}.{2}' alt='{3}'/>", VirtualPathUtility.ToAbsolute(folder), src,
                ext, alt);
            //return  new HtmlString(markup);
            return new HtmlString(markup);
        }

        #endregion

        #region Date and Time Display


        private static string GetDefaultTimeZoneFormat(DateTimeFormatType type = DateTimeFormatType.Date, string customFormat = null) {
            string format;
            if (!string.IsNullOrEmpty(customFormat)) {
                return customFormat;
            }
            switch (type) {
                case DateTimeFormatType.Date:
                    format = "dd-MMM-yy";
                    break;
                case DateTimeFormatType.DateTimeSimple:
                    format = "dd-MMM-yy hh:mm:ss tt";
                    break;
                case DateTimeFormatType.DateTimeFull:
                    format = "MMMM dd, yyyy hh:mm:ss tt";
                    break;
                case DateTimeFormatType.DateTimeShort:
                    format = "d-MMM-yy hh:mm:ss tt";
                    break;
                case DateTimeFormatType.Time:
                    format = "hh:mm:ss tt";
                    break;
                default:
                    format = "dd-MMM-yy";
                    break;
            }

            return format;
        }

        /// <summary>
        /// Returns a date-time using time-zone via TimeZoneSet
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="timeZone">Timezone set</param>
        /// <param name="dt"></param>
        /// <param name="formatType">
        /// switch (type) {
        ///     case DateTimeFormatType.Date:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        ///     case DateTimeFormatType.DateTimeSimple:
        ///         format = "dd-MMM-yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeFull:
        ///         format = "MMMM dd, yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeShort:
        ///         format = "d-MMM-yy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.Time:
        ///         format = "hh:mm:ss tt";
        ///         break;
        ///     default:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        /// }
        /// </param>
        /// <param name="customFormat">If anything passed then this format will be used.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns a data-time using given format and timezone</returns>
        public static string DisplayDateTime(
            this HtmlHelper helper,
            TimeZoneSet timeZone,
            DateTime? dt = null,
            DateTimeFormatType formatType = DateTimeFormatType.Date,
            string customFormat = null,
            bool addTimeZoneString = false) {
            var format = GetDefaultTimeZoneFormat(formatType, customFormat);
            return Zone.GetDateTime(timeZone, dt, format, addTimeZoneString);
        }

        /// <summary>
        /// Returns a date-time using time-zone via UserId
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="userId">User id</param>
        /// <param name="dt"></param>
        /// <param name="formatType">
        /// switch (type) {
        ///     case DateTimeFormatType.Date:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        ///     case DateTimeFormatType.DateTimeSimple:
        ///         format = "dd-MMM-yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeFull:
        ///         format = "MMMM dd, yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeShort:
        ///         format = "d-MMM-yy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.Time:
        ///         format = "hh:mm:ss tt";
        ///         break;
        ///     default:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        /// }
        /// </param>
        /// <param name="customFormat">If anything passed then this format will be used.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns a data-time using given format and timezone</returns>
        public static string DisplayDateTime(
            this HtmlHelper helper,
            long userId,
            DateTime? dt = null,
            DateTimeFormatType formatType = DateTimeFormatType.Date,
            string customFormat = null,
            bool addTimeZoneString = false) {
            var format = GetDefaultTimeZoneFormat(formatType, customFormat);
            return Zone.GetDateTime(userId, dt, format, addTimeZoneString);
        }

        /// <summary>
        /// Returns a date-time using current user.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="dt"></param>
        /// <param name="formatType">
        /// switch (type) {
        ///     case DateTimeFormatType.Date:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        ///     case DateTimeFormatType.DateTimeSimple:
        ///         format = "dd-MMM-yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeFull:
        ///         format = "MMMM dd, yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeShort:
        ///         format = "d-MMM-yy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.Time:
        ///         format = "hh:mm:ss tt";
        ///         break;
        ///     default:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        /// }
        /// </param>
        /// <param name="customFormat">If anything passed then this format will be used.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns a data-time using given format and timezone</returns>
        public static string DisplayDateTime(
            this HtmlHelper helper,
            DateTime? dt = null,
            DateTimeFormatType formatType = DateTimeFormatType.Date,
            string customFormat = null,
            bool addTimeZoneString = false) {
            var timezoneSet = Zone.Get();
            var format = GetDefaultTimeZoneFormat(formatType, customFormat);
            return Zone.GetDateTime(timezoneSet, dt, format, addTimeZoneString);
        }

        /// <summary>
        /// Returns a date-time using current user.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="dt"></param>
        /// <param name="formatType">
        /// switch (type) {
        ///     case DateTimeFormatType.Date:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        ///     case DateTimeFormatType.DateTimeSimple:
        ///         format = "dd-MMM-yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeFull:
        ///         format = "MMMM dd, yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeShort:
        ///         format = "d-MMM-yy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.Time:
        ///         format = "hh:mm:ss tt";
        ///         break;
        ///     default:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        /// }
        /// </param>
        /// <param name="customFormat">If anything passed then this format will be used.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns a data-time using given format and timezone</returns>
        public static string DisplayDate(
            this HtmlHelper helper,
            DateTime? dt = null,
            DateTimeFormatType formatType = DateTimeFormatType.Date,
            string customFormat = null,
            bool addTimeZoneString = false) {
            return DisplayDateTime(helper, dt, DateTimeFormatType.Date, customFormat, addTimeZoneString);
        }

        /// <summary>
        /// Returns a date-time without using any timezone.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="withoutTimezone"></param>
        /// <param name="dt"></param>
        /// <param name="formatType">
        /// switch (type) {
        ///     case DateTimeFormatType.Date:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        ///     case DateTimeFormatType.DateTimeSimple:
        ///         format = "dd-MMM-yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeFull:
        ///         format = "MMMM dd, yyyy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.DateTimeShort:
        ///         format = "d-MMM-yy hh:mm:ss tt";
        ///         break;
        ///     case DateTimeFormatType.Time:
        ///         format = "hh:mm:ss tt";
        ///         break;
        ///     default:
        ///         format = "dd-MMM-yyyy";
        ///         break;
        /// }
        /// </param>
        /// <param name="customFormat">If anything passed then this format will be used.</param>
        /// <returns>Returns a data-time without using timezone</returns>
        public static string DisplayDate(
            this HtmlHelper helper,
            bool withoutTimezone = true,
            DateTime? dt = null,
            DateTimeFormatType formatType = DateTimeFormatType.Date,
            string customFormat = null) {
            var format = GetDefaultTimeZoneFormat(formatType, customFormat);
            return dt.HasValue ? dt.Value.ToString(format) : "";
        }


        #endregion
    }
}