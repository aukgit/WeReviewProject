#region using block

using FluentScheduler;
using WeReviewApp.Common;

#endregion

namespace WeReviewApp.Scheduler {
    internal class WeReviewScheduler : ITask {
        #region ITask Members


        public void Execute() {
            // keep the app running
            if (WereViewStatics.AppCategoriesCache.Count > 0 || CommonVars.StaticAppsList.Count > 0) {
                //string text = DateTime.Now.ToString();
                //UploadProcessor uploader = new UploadProcessor("");

                //var appPath = uploader.GetCombinePathWithAdditionalRoots();

                //File.WriteAllText(appPath + "done.txt", text);
            }
        }

        #endregion
    }
}