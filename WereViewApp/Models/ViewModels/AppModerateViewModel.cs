using System.ComponentModel.DataAnnotations;
using WereViewApp.Models.EntityModel;
namespace WereViewApp.Models.ViewModels {
    public class AppModerateViewModel {
        private long? _appId;
        private bool _isBlocked;
        private App _app;

        public long AppId
        {
            get
            {
                if (!_appId.HasValue) {
                    _appId = App != null ? App.AppID : -1;
                }

                return _appId.Value;
            }
            set { _appId = value; }
        }

        public App App
        {
            get { return _app; }
            set
            {
                _app = value;
                AppId = _app.AppID;
            }
        }

        [Display(Name = "Should block the app.", Description = "App is blocked or not?")]
        public bool IsBlocked
        {
            get { return _isBlocked; }
            set { _isBlocked = value; }
        }
        [Display(Name = "Is featured app.", Description = "Will this app displayed in the home page?")]
        public bool IsFeatured { get; set; }
        [Display(Name = "Like to hear from that developer.", Description = "Add an email footer note that admin is like to hear from them.")]
        public bool LikeToHearFromYou { get; set; }
        [Display(Name = "Message", Description = "Add an email footer note that admin is like to hear from them.")]
        public string Message { get; set; }
    }
}