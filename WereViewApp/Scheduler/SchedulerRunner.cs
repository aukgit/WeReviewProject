#region using block

using FluentScheduler;

#endregion

namespace WereViewApp.Scheduler {
    public class SchedulerRunner : Registry {
        public SchedulerRunner() {
            
            
            Schedule<WeReviewScheduler>().ToRunNow().AndEvery(2).Minutes();
        }
    }
}