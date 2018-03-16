using System.Net;
using FluentScheduler;

namespace WeReviewApp.Scheduler {
    internal class WeReviewDownloadScheduler : IJob {
        #region ITask Members

        public void Execute() {
            // keep the app running
            var contents = new WebClient().DownloadString(AppVar.Url);
            var contentsSitemap = new WebClient().DownloadString(AppVar.Url + "/Sitemap");
        }

        #endregion
    }
}