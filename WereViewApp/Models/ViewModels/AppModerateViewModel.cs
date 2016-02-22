using WereViewApp.Models.EntityModel;
namespace WereViewApp.Models.ViewModels {
    public class AppModerateViewModel {
        private long? _appId;
        private bool _isBlocked;

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

        public App App { get; set; }

        public bool IsBlocked
        {
            get { return _isBlocked; }
            set { _isBlocked = value; }
        }

        public bool IsFeatured { get; set; }
        public bool IsMessageNeededToSent { get; set; }
        public string Message { get; set; }
    }
}