using System;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using DevMvcComponent;
using DevMvcComponent.EntityConversion;

namespace ReviewApps.Modules.Mail {
    /// <summary>
    ///     Sends mails through Mvc.Mailer with threads
    /// </summary>
    public class MailSender {
        private readonly bool _isCompanyNameOnEmailSubject = false;

        public string GetSubject(string sub, string type = "") {
            if (_isCompanyNameOnEmailSubject) {
                if (string.IsNullOrEmpty(type)) {
                    return "[" + AppVar.Name + "][" + AppVar.Setting.CompanyName + "] " + sub;
                }
                return "[" + AppVar.Name + "][" + AppVar.Setting.CompanyName + "][" + type + "] " + sub;
            }
            if (string.IsNullOrEmpty(type)) {
                return "[" + AppVar.Name + "] " + sub;
            }
            return "[" + AppVar.Name + "][" + type + "] " + sub;
        }

        public async void NotifyAdmin(string subject, string htmlMessage, string type = "",
            bool generateDecentSubject = true) {
            if (generateDecentSubject) {
                subject = GetSubject(subject, type);
            }
            Mvc.Mailer.QuickSend(AppVar.Setting.AdminEmail, subject, htmlMessage);
        }

        /// <summary>
        ///     Notify someone with an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="type"></param>
        /// <param name="generateDecentSubject"></param>
        public async void Send(string to, string subject, string htmlMessage, string type = "",
            bool generateDecentSubject = true) {
            if (generateDecentSubject) {
                subject = GetSubject(subject, type);
            }
            new Thread(() => {
                Mvc.Mailer.QuickSend(to, subject, htmlMessage,isAsync:false);
            }).Start();
        }
        /// <summary>
        /// Send email to the developer
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="type"></param>
        /// <param name="generateDecentSubject"></param>
        /// <param name="entityObject"></param>
        /// <param name="modelStateDictionary"></param>
        public async void NotifyDeveloper(
            string subject,
            string htmlMessage,
            string type = "",
            bool generateDecentSubject = true,
            object entityObject = null,
            ModelStateDictionary modelStateDictionary = null) {
            if (AppVar.Setting.NotifyDeveloperOnError) {
                if (generateDecentSubject) {
                    subject = GetSubject(subject, type);
                }
                var entityHtml = EntityToString.AsHtmlTable(entityObject);
                htmlMessage += entityHtml;
                if (modelStateDictionary != null) {
                    var sb = new StringBuilder(modelStateDictionary.Count * 4 + 2);
                    sb.Append("<ul>");
                    foreach (var state in modelStateDictionary) {
                        var errors = state.Value.Errors;
                        if (errors != null) {
                            foreach (var error in errors) {
                                sb.Append("<li style='color:red;'>" + error.ErrorMessage + "</li>");
                            }
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("<br >");
                    sb.Append("<br >");
                    htmlMessage += sb.ToString();
                }
                new Thread(() => {
                    Mvc.Mailer.QuickSend(AppVar.Setting.DeveloperEmail, subject, htmlMessage, isAsync: false);
                }).Start();
            }
        }

        public async void HandleError(Exception exception, string method, string subject = "", object entity = null,
            string type = "", bool generateDecentSubject = true) {
            if (generateDecentSubject) {
                subject = GetSubject(subject, type);
            }
            subject += " on method [" + method + "()]";

            Mvc.Error.HandleBy(exception, method, subject, entity);
        }
    }
}