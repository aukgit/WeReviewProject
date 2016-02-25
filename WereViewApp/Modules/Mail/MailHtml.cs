using System.Text;
using WereViewApp.Models.POCO.Identity;

namespace WereViewApp.Modules.Mail {
    public class MailHtml {
        #region Properties


        #endregion
        /// <summary>
        /// Add the footnote to the stringbuilder
        /// </summary>
        /// <param name="footerSenderName"></param>
        /// <param name="department"></param>
        /// <param name="sb"></param>
        public static void AddThanksFooterOnStringBuilder(string footerSenderName, string department, StringBuilder sb) {
            sb.Append(string.Format(DivTag, "", "Thank you", "Thank  you,"));
            sb.Append(string.Format(DivTag, "font-size:14px;font-weight:bold;", "", footerSenderName));
            sb.Append(string.Format(DivTag, "", "Department", department));
            sb.Append(string.Format(DivTag, "", AppVar.Name, AppVar.Name));
            sb.Append(string.Format(DivTag, "", AppVar.Subtitle, AppVar.Subtitle));
        }

        public static string EmailConfirmHtml(ApplicationUser user, string callBackUrl, string footerSenderName = "",
            string department = "Administration", string body = null) {
            var sb = new StringBuilder(100);
            if (body == null) {
                body = string.Format(DefaultMailConfirmBody, AppVar.Url, AppVar.Name, callBackUrl, callBackUrl);
            }

            AddGreetingsToStringBuilder(user, sb);
            sb.AppendLine(body);
            sb.AppendLine(LineBreak);
            sb.AppendLine(string.Format(DivTag, "", "", "Name : " + user.DisplayName));
            sb.AppendLine(string.Format(DivTag, "", "", "Login(username) : " + user.UserName));
            sb.AppendLine(string.Format(DivTag, "", "", "Email : " + user.Email));
            sb.AppendLine(string.Format(DivTag, "", "", "Phone : " + user.PhoneNumber));
            sb.AppendLine(LineBreak);
            AddThanksFooterOnStringBuilder(AppVar.Setting.AdminName, department, sb);
            return sb.ToString();
        }

        public static string PasswordResetHtml(ApplicationUser user, string callBackUrl, string footerSenderName = "",
       string department = "Administration", string body = null) {
            var sb = new StringBuilder(100);
            if (body == null) {
                body = string.Format(DefaultResetAccountBody, callBackUrl, "this reset form");
            }

            AddGreetingsToStringBuilder(user, sb);
            sb.AppendLine(body);
            sb.AppendLine(LineBreak);
            if (footerSenderName == "") {
                footerSenderName = AppVar.Setting.AdminName;
            }
            AddThanksFooterOnStringBuilder(footerSenderName, department, sb);
            return sb.ToString();
        }

        /// <summary>
        ///     Usages line break after greetings
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sb"></param>
        /// <param name="showFullName">Full name gives First+ ' ' + LastName</param>
        public static void AddGreetingsToStringBuilder(ApplicationUser user, StringBuilder sb, bool showFullName = false) {
            if (showFullName) {
                sb.AppendLine("Hello " + user.LastName + ", <br>");
            } else {
                sb.AppendLine("Hello " + user.DisplayName + ", <br>");
            }
            sb.AppendLine(LineBreak);
        }

        /// <summary>
        ///     Don't give a line break. Use your own.
        /// </summary>
        public static void AddContactUsToStringBuilder(StringBuilder sb) {
            sb.AppendLine("If you have any further query, we would love to hear it. Please drop your feedbacks at " +
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
            return string.Format(ContactUsLink, title, linkName, addClass, AppVar.Url);
        }

        public static string BlockEmailHtml(ApplicationUser user, string reasonForBlocking, string footerSenderName = "",
            string department = "Administration", string body = null) {
            var sb = new StringBuilder(100);

            AddGreetingsToStringBuilder(user, sb); // greetings

            sb.AppendLine("You have been blocked from " + AppVar.Name + ".<br>");
            sb.AppendLine("Reason : " + reasonForBlocking + ".<br>");
            sb.AppendLine(LineBreak);

            AddContactUsToStringBuilder(sb); //contact us

            AddThanksFooterOnStringBuilder(footerSenderName, department, sb);
            return sb.ToString();
        }

        public static string ReleasedFromBlockEmailHtml(ApplicationUser user, string footerSenderName = "",
            bool saySorry = false, string department = "Administration", string body = null) {
            var sb = new StringBuilder(100);

            AddGreetingsToStringBuilder(user, sb); // greetings

            sb.AppendLine("You have been re-enabled again in " + AppVar.Name + ".");
            if (saySorry) {
                sb.AppendLine("We are deeply sorry for your inconvenience.");
            }

            sb.AppendLine(LineBreak);
            AddContactUsToStringBuilder(sb); //contact us
            sb.AppendLine(LineBreak);
            AddThanksFooterOnStringBuilder(footerSenderName, department, sb);
            return sb.ToString();
        }

        #region Declaration


        /// <summary>
        ///     We are very delighted to have you in [a href='{0}' title='{1}']{1}[/a]. [a href='{2}' title='{3}']Here[/a] is the
        ///     [a href='{2}' title='{3}']link[/a] to active your account. Or you can also copy paste the raw version below to your
        ///     browser's address bar. Raw : {3}
        /// </summary>
        private const string DefaultMailConfirmBody =
            "We are very delighted to have you in <a href='{0}' title='{1}'>{1}</a>. <a href='{2}' title='{3}'>Here</a> is the <a href='{2}' title='{3}'>link</a> to active your account. Or you can also copy paste the raw version below to your browser's address bar.<br><br> Raw : {3} <br><br>";

        /// <summary>
        /// You can reset your password from <a href='{0}' title='{1}'>{1}</a>. Or you can also copy paste the raw version below to your browser's address bar to reset your account. Raw URL: {0}
        /// </summary>
        private const string DefaultResetAccountBody =
            "You can reset your password from <a href='{0}' title='{1}'>{1}</a>. Or you can also copy paste the raw version below to your browser's address bar to reset your account.<br><br> Raw URL: {0} <br><br>";

        #endregion

        #region HTMl Tag Constants

        /// <summary>
        ///     [a id='contact-us-page-link' class='contact-us-page-link' href='{3}/ContactUs' class='{2}' title='{0}']{1}[/a]
        /// </summary>
        public const string ContactUsLink =
            "<a id='contact-us-page-link' class='contact-us-page-link' href='{3}/ContactUs' class='{2}' title='{0}'>{1}</a>";

        /// <summary>
        ///     br tag
        /// </summary>
        public const string LineBreak = "<br>";

        /// <summary>
        ///     [h1 style='{0}' title='{1}']{2}[/h1]
        /// </summary>
        public const string H1 = "<h1 style='{0}' title='{1}'>{2}</h1>";

        /// <summary>
        ///     [h2 style='{0}' title='{1}']{2}[/h2]
        /// </summary>
        public const string H2 = "<h2 style='{0}' title='{1}'>{2}</h2>";

        /// <summary>
        ///     [h3 style='{0}' title='{1}']{2}[/h3]
        /// </summary>
        public const string H3 = "<h3 style='{0}' title='{1}'>{2}</h3>";

        /// <summary>
        ///     [h4 style='{0}' title='{1}']{2}[/h4]
        /// </summary>
        public const string H4 = "<h4 style='{0}' title='{1}'>{2}</h4>";

        /// <summary>
        ///     [div style='{0}' title='{1}']{2}[/div]
        /// </summary>
        public const string DivTag = "<div style='{0}' title='{1}'>{2}</div>";

        /// <summary>
        ///     [span style='{0}' title='{1}']{2}[/span]
        /// </summary>
        public const string SpanTag = "<span style='{0}' title='{1}'>{2}</span>";

        /// <summary>
        ///     [strong style='{0}' title='{1}']{2}[/strong]
        /// </summary>
        public const string StrongTag = "<strong style='{0}' title='{1}'>{2}</strong>";

        #endregion
    }
}