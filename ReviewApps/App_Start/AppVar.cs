using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using ReviewApps.Models.POCO.IdentityCustomization;
using ReviewApps.Modules.Mail;

namespace ReviewApps {
    /// <summary>
    ///     Application Global Variables
    /// </summary>
    public struct AppVar {
        #region Enums

        #endregion

        #region Constants

        #endregion

        #region Connection Strings and Constants

        //public const string DefaultConnection = @"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\WeReviewApp-Accounts.mdf;Initial Catalog=WeReviewApp-Accounts;Integrated Security=True";
        private static readonly string DefaultConnection = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        public enum ConnectionStringType {
            DefaultConnection,
            Secondary
        }

        #endregion

        #region Properties

        private static string _productNameMeta;

        /// <summary>
        ///     Application Name
        /// </summary>
        public static string Name {
            get { return Setting.ApplicationName; }
        }

        /// <summary>
        ///     Application Subtitle
        /// </summary>
        public static string Subtitle {
            get { return Setting.ApplicationSubtitle; }
        }

        /// <summary>
        ///     Is application in testing environment or not?
        /// </summary>
        public static bool IsInTestEnvironment {
            get { return Setting.IsInTestingEnvironment; }
        }

        public static CoreSetting Setting;

        /// <summary>
        ///     Get the application URL based on the application environment.
        ///     Without slash.
        /// </summary>
        public static string Url {
            get {
                if (IsInTestEnvironment) {
                    return Setting.TestingUrl;
                }
                return Setting.LiveUrl;
            }
        }

        public static MailSender Mailer = new MailSender();

        #endregion

        #region Functions

        public static string GetConnectionString(ConnectionStringType type) {
            switch (type) {
                case ConnectionStringType.DefaultConnection:
                    return DefaultConnection;
                case ConnectionStringType.Secondary:
                    break;
                default:
                    break;
            }
            return null;
        }

        private static string GetCommonMetadescription() {
            var finalMeta = "";
            if (_productNameMeta == null) {
                var nameList = Name.Split(' ').ToList();
                nameList.Add(Name);
                nameList.Add(Subtitle);
                foreach (var item in nameList) {
                    if (finalMeta.Equals("")) {
                        finalMeta += ",";
                    }
                    finalMeta += item;
                }
                _productNameMeta = finalMeta;
            }
            return _productNameMeta;
        }

        internal static void SetCommonMetaDescriptionToEmpty() {
            _productNameMeta = null;
        }

        public static ActionResult GetFriendlyError(string title, string message) {
            var dictionary = new ViewDataDictionary {
                {"Title", title},
                {"ErrorMessage", message}
            };
            return new ViewResult {
                ViewName = "_FriendlyError",
                ViewData = dictionary
            };
        }

        public static ActionResult GetAuthenticationError(string title, string message) {
            var dictionary = new ViewDataDictionary {
                {"Title", title},
                {"ErrorMessage", message}
                
            };
            return new ViewResult {
                ViewName = "_AuthenticationError",
                ViewData = dictionary
            };
        }

        public static void GetTitlePageMeta(dynamic viewBag, string title, string msg = "", string meta = null,
            string keywords = null) {
            viewBag.Title = title;
            viewBag.Message = msg;
            viewBag.Meta = meta + "," + GetCommonMetadescription();
            viewBag.Keywords = keywords + "," + GetCommonMetadescription();
        }

        public static void SetSavedStatus(dynamic viewBag, string msg = null) {
            if (msg == null) {
                msg = "Your previous transaction is successfully saved.";
            }
            viewBag.Success = msg;
        }

        public static void SetErrorStatus(dynamic viewBag, string msg = null) {
            if (msg == null) {
                msg = "Your last transaction is not saved.";
            }
            viewBag.ErrorSave = msg;
        }

        #endregion
    }
}