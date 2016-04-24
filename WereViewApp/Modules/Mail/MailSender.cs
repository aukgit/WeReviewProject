using System;
using System.Threading;
using DevMvcComponent;

namespace WeReviewApp.Modules.Mail {
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
            new Thread(() => { Mvc.Mailer.QuickSend(AppVar.Setting.AdminEmail, subject, htmlMessage); }).Start();
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
            new Thread(() => { Mvc.Mailer.QuickSend(to, subject, htmlMessage); }).Start();
        }

        public async void NotifyDeveloper(string subject, string htmlMessage, string type = "",
            bool generateDecentSubject = true) {
            if (AppVar.Setting.NotifyDeveloperOnError) {
                if (generateDecentSubject) {
                    subject = GetSubject(subject, type);
                }
                new Thread(() => { Mvc.Mailer.QuickSend(AppVar.Setting.DeveloperEmail, subject, htmlMessage); }).Start();
            }
        }

        public async void HandleError(Exception exception, string method, string subject = "", object entity = null,
            string type = "", bool generateDecentSubject = true) {
                {
                    if (generateDecentSubject) {
                        subject = GetSubject(subject, type);
                    }
                    subject += " on method [" + method + "()]";

                    Mvc.Error.HandleBy(exception, method, subject, entity);
                }
        }
    }
}