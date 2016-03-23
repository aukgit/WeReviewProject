using FluentScheduler;

namespace WeReviewApp.Scheduler {
    internal class WeReviewDownloadScheduler : ITask {
        #region ITask Members


        public void Execute() {
            // keep the app running
            var contents = new System.Net.WebClient().DownloadString(AppVar.Url);
            var contentsSitemap = new System.Net.WebClient().DownloadString(AppVar.Url + "/Sitemap");
        }

        #endregion
    }
}