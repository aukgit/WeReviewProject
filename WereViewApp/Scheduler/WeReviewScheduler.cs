#region using block

using System;
using System.IO;
using FluentScheduler;
using WeReviewApp.Common;
using WeReviewApp.Modules.Uploads;

#endregion

namespace WeReviewApp.Scheduler {
    internal class WeReviewScheduler : ITask {
        #region ITask Members


        public void Execute() {
            // keep the app running
            if (WereViewStatics.AppCategoriesCache.Count > 0 || CommonVars.StaticAppsList.Count > 0) {
                //string text = DateTime.Now.ToString();
                //UploadProcessor uploader = new UploadProcessor("");

                //var appPath = uploader.GetCombinationOfRootAndAdditionalRoot();

                //File.WriteAllText(appPath + "done.txt", text);
            }
        }

        #endregion
    }
}