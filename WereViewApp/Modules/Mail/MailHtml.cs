using System.Text;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Modules.Cache;

namespace WereViewApp.Modules.Mail {
    public static class MailHtml {
        #region Propertise

        private static StringBuilder SB
        {
            get
            {
                if (_sb != null) {
                    return _sb;
                }
                _sb = new StringBuilder(_len);
                return _sb;
            }
        }

        #endregion

        public static string ThanksFooter(string footerSenderName, string department) {
            var s = string.Format(DIV_TAG, "", "Thank you", "Thank  you,");
            s += string.Format(DIV_TAG, "font-size:14px;font-weight:bold;", "", footerSenderName);
            s += string.Format(DIV_TAG, "", "Department", department);
            s += string.Format(DIV_TAG, "", AppVar.Name, AppVar.Name);
            s += string.Format(DIV_TAG, "", AppVar.Subtitle, AppVar.Subtitle);
            return s;
        }

        public static string EmailConfirmHtml(ApplicationUser user, string callBackUrl, string footerSenderName = "",
            string department = "Administration", string body = null) {
            SB.Clear();
            if (body == null) {
                body = string.Format(_defaultMailConfirmBody, AppVar.Url, AppVar.Name, callBackUrl, callBackUrl);
            }

            AddGreetingsToStringBuilder(user);
            SB.AppendLine(body);
            SB.AppendLine(LINE_BREAK);
            SB.AppendLine(string.Format(DIV_TAG, "", "", "Name : " + user.DisplayName));
            SB.AppendLine(string.Format(DIV_TAG, "", "", "Login(username) : " + user.UserName));
            SB.AppendLine(string.Format(DIV_TAG, "", "", "Email : " + user.Email));
            SB.AppendLine(string.Format(DIV_TAG, "", "", "Phone : " + user.PhoneNumber));
            SB.AppendLine(string.Format(DIV_TAG, "", "", "Timezone : " + CachedQueriedData.GetTimezone(user).UTCName));
            SB.AppendLine(string.Format(DIV_TAG, "", "", "Country : " + CachedQueriedData.GetCountry(user).CountryName));
            SB.AppendLine(LINE_BREAK);
            SB.AppendLine(ThanksFooter(AppVar.Setting.AdminName, department));
            return SB.ToString();
        }

        /// <summary>
        ///     Usages line break after greetings
        /// </summary>
        /// <param name="user"></param>
        /// <param name="showFullName">Full name gives First+ ' ' + LastName</param>
        public static void AddGreetingsToStringBuilder(ApplicationUser user, bool showFullName = false) {
            if (showFullName) {
                SB.AppendLine("Hello " + user.LastName + ", <br>");
            } else {
                SB.AppendLine("Hello " + user.DisplayName + ", <br>");
            }
            SB.AppendLine(LINE_BREAK);
        }

        /// <summary>
        ///     Don't give a line break. Use your own.
        /// </summary>
        public static void AddContactUsToStringBuilder() {
            SB.AppendLine("If you have any further query, we would love to hear it. Please drop your feedbacks at " +
                          GetContactUsLinkHtml() + ".");
        }

        public static string GetContactUsLinkHtml(string linkName = null, string title = null, string addClass = "") {
            if (linkName == null) {
                linkName = AppVar.Url + "/ContactUs";
                linkName = linkName.Replace("http://", "");
            }
            if (title == null) {
                title = "Contact us and drop your feedback about anything.";
            }
            return string.Format(CONTACT_US_LINK, title, linkName, addClass, AppVar.Url);
        }

        public static string BlockEmailHtml(ApplicationUser user, string reasonForBlocking, string footerSenderName = "",
            string department = "Administration", string body = null) {
            SB.Clear();

            AddGreetingsToStringBuilder(user); // greetings

            SB.AppendLine("You have been blocked from " + AppVar.Name + ".<br>");
            SB.AppendLine("Reason : " + reasonForBlocking + ".<br>");
            SB.AppendLine(LINE_BREAK);

            AddContactUsToStringBuilder(); //contact us

            SB.AppendLine(ThanksFooter(footerSenderName, department));
            return SB.ToString();
        }

        public static string ReleasedFromBlockEmailHtml(ApplicationUser user, string footerSenderName = "",
            bool saySorry = false, string department = "Administration", string body = null) {
            SB.Clear();

            AddGreetingsToStringBuilder(user); // greetings

            SB.AppendLine("You have been re-enabled again in " + AppVar.Name + ".");
            if (saySorry) {
                SB.AppendLine("We are deeply sorry for your inconvenience.");
            }

            SB.AppendLine(LINE_BREAK);
            AddContactUsToStringBuilder(); //contact us
            SB.AppendLine(LINE_BREAK);
            SB.AppendLine(ThanksFooter(footerSenderName, department));
            return SB.ToString();
        }

        #region Declaration

        private const int _len = 2000;

        /// <summary>
        ///     We are very delighted to have you in [a href='{0}' title='{1}']{1}[/a]. [a href='{2}' title='{3}']Here[/a] is the
        ///     [a href='{2}' title='{3}']link[/a] to active your account. Or you can also copy paste the raw version below to your
        ///     browser's address bar. Raw : {3}
        /// </summary>
        private const string _defaultMailConfirmBody =
            "We are very delighted to have you in <a href='{0}' title='{1}'>{1}</a>. <a href='{2}' title='{3}'>Here</a> is the <a href='{2}' title='{3}'>link</a> to active your account. Or you can also copy paste the raw version below to your browser's address bar.<br><br> Raw : {3} <br><br>";


        private static StringBuilder _sb;

        #endregion

        #region HTMl Tag Constants

        /// <summary>
        ///     [a id='contact-us-page-link' class='contact-us-page-link' href='{3}/ContactUs' class='{2}' title='{0}']{1}[/a]
        /// </summary>
        internal const string CONTACT_US_LINK =
            "<a id='contact-us-page-link' class='contact-us-page-link' href='{3}/ContactUs' class='{2}' title='{0}'>{1}</a>";

        /// <summary>
        ///     br tag
        /// </summary>
        internal const string LINE_BREAK = "<br>";

        /// <summary>
        ///     [h1 style='{0}' title='{1}']{2}[/h1]
        /// </summary>
        internal const string H1 = "<h1 style='{0}' title='{1}'>{2}</h1>";

        /// <summary>
        ///     [h2 style='{0}' title='{1}']{2}[/h2]
        /// </summary>
        internal const string H2 = "<h2 style='{0}' title='{1}'>{2}</h2>";

        /// <summary>
        ///     [h3 style='{0}' title='{1}']{2}[/h3]
        /// </summary>
        internal const string H3 = "<h3 style='{0}' title='{1}'>{2}</h3>";

        /// <summary>
        ///     [h4 style='{0}' title='{1}']{2}[/h4]
        /// </summary>
        internal const string H4 = "<h4 style='{0}' title='{1}'>{2}</h4>";

        /// <summary>
        ///     [div style='{0}' title='{1}']{2}[/div]
        /// </summary>
        internal const string DIV_TAG = "<div style='{0}' title='{1}'>{2}</div>";

        /// <summary>
        ///     [span style='{0}' title='{1}']{2}[/span]
        /// </summary>
        internal const string SPAN_TAG = "<span style='{0}' title='{1}'>{2}</span>";

        /// <summary>
        ///     [strong style='{0}' title='{1}']{2}[/strong]
        /// </summary>
        internal const string STRONG_TAG = "<strong style='{0}' title='{1}'>{2}</strong>";

        #endregion
    }
}