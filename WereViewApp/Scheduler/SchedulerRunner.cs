#region using block

using FluentScheduler;

#endregion

namespace WereViewApp.Scheduler {
    public class SchedulerRunner : Registry {
        public SchedulerRunner() {
            // keep the site running in the pool
            Schedule<WeReviewScheduler>().ToRunNow().AndEvery(2).Minutes();
            // load home page at every hour.
            Schedule<WeReviewDownloadScheduler>().ToRunNow().AndEvery(1).Hours();
        }
    }
}