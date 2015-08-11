using System;

namespace WereViewApp.Modules.Mail {
    public class MailSender {
        private readonly bool isCompanyNameOnEmailSubject = false;

        public string GetSubject(string sub, string type = "") {
            if (isCompanyNameOnEmailSubject) {
                if (string.IsNullOrEmpty(type))
                    return "[" + AppVar.Name + "][" + AppVar.Setting.CompanyName + "] " + sub;
                return "[" + AppVar.Name + "][" + AppVar.Setting.CompanyName + "][" + type + "] " + sub;
            }
            if (string.IsNullOrEmpty(type))
                return "[" + AppVar.Name + "] " + sub;
            return "[" + AppVar.Name + "][" + type + "] " + sub;
        }

        public void NotifyAdmin(string subject, string HTMLMessage, string type = "", bool GenerateDecentSubject = true) {
            if (GenerateDecentSubject) {
                subject = GetSubject(subject, type);
            }
            Starter.Mailer.QuickSend(AppVar.Setting.AdminEmail, subject, HTMLMessage);
        }

        /// <summary>
        ///     Notify someone with an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="HTMLMessage"></param>
        /// <param name="type"></param>
        /// <param name="GenerateDecentSubject"></param>
        public void Send(string to, string subject, string HTMLMessage, string type = "",
            bool GenerateDecentSubject = true) {
            if (GenerateDecentSubject) {
                subject = GetSubject(subject, type);
            }
            Starter.Mailer.QuickSend(to, subject, HTMLMessage);
        }

        public void NotifyDeveloper(string subject, string HTMLMessage, string type = "",
            bool GenerateDecentSubject = true) {
            if (AppVar.Setting.NotifyDeveloperOnError) {
                if (GenerateDecentSubject) {
                    subject = GetSubject(subject, type);
                }
                Starter.Mailer.QuickSend(AppVar.Setting.DeveloperEmail, subject, HTMLMessage);
            }
        }

        public void NotifyUserMulti(string subject, string toEmail, string[] UserEamil, string HTMLMessage,
            string type = "", bool GenerateDecentSubject = true) {
            if (GenerateDecentSubject) {
                subject = GetSubject(subject, type);
            }
            Starter.Mailer.QuickSend(toEmail, UserEamil, subject, HTMLMessage);
        }

        public void HandleError(Exception exception, string method, string subject = "", object entity = null,
            string type = "", bool GenerateDecentSubject = true) {
                {
                    if (GenerateDecentSubject) {
                        subject = GetSubject(subject, type);
                    }
                    subject += " on method [" + method + "()]";

                    Starter.Error.HandleBy(exception, method, subject, entity);
                }
        }
    }
}