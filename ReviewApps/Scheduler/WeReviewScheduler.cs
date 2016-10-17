#region using block

using FluentScheduler;
using ReviewApps.Common;

#endregion

namespace ReviewApps.Scheduler {
    internal class WeReviewScheduler : IJob {
        #region ITask Members


        public void Execute() {
            // keep the app running
            if (Statics.AppCategoriesCache.Count > 0 || Variables.StaticAppsList.Count > 0) {
                //string text = DateTime.Now.ToString();
                //UploadProcessor uploader = new UploadProcessor("");

                //var appPath = uploader.GetCombinePathWithAdditionalRoots();

                //File.WriteAllText(appPath + "done.txt", text);
            }
        }

        #endregion
    }
}