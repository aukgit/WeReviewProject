using WereViewApp.Models.POCO.Enum;
using WereViewApp.Models.POCO.IdentityCustomization;

namespace WereViewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class FeedbackExtend {
        public static FeedbackState GetStatus(this Feedback feedback) {
            if (!feedback.IsViewed) {
                return new FeedbackState() {
                    Status = "Not viewed yet.",
                    StyleClass = "warning"
                };
            } else if (feedback.IsInProcess) {
                return new FeedbackState() {
                    Status = "Is in process!",
                    StyleClass = "warning"
                };
            } else if (feedback.HasMarkedToFollowUpDate) {
                return new FeedbackState() {
                    Status = "Has a follow up.",
                    StyleClass = "info"
                };
            } else if (feedback.IsUnSolved) {
                return new FeedbackState() {
                    Status = "Unsolved feedback.",
                    StyleClass = "danger"
                };
            } else if (feedback.IsSolved) {
                return new FeedbackState() {
                    Status = "Solved",
                    StyleClass = "success"
                };
            } else {
                return new FeedbackState() {
                    Status = "Unknown",
                    StyleClass = "warning"
                };
            }
        }
        /// <summary>
        /// Please check the condition.
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="statusType"></param>
        public static void SetStatus(this Feedback feedback, FeedbackStatusTypes statusType) {
            switch (statusType) {
                case FeedbackStatusTypes.IsViewed:
                    feedback.IsViewed = true;
                    feedback.IsSolved = false;
                    feedback.HasMarkedToFollowUpDate = false;
                    feedback.IsInProcess = false;
                    feedback.IsUnSolved = false;
                    break;
                case FeedbackStatusTypes.IsInProcess:
                    feedback.IsViewed = true;
                    feedback.IsSolved = false;
                    feedback.HasMarkedToFollowUpDate = false;
                    feedback.IsInProcess = true;
                    feedback.IsUnSolved = false;
                    break;
                case FeedbackStatusTypes.HasFollowupDate:
                    feedback.IsViewed = true;
                    feedback.IsSolved = false;
                    feedback.HasMarkedToFollowUpDate = true;
                    feedback.IsInProcess = true;
                    feedback.IsUnSolved = false;
                    break;
                case FeedbackStatusTypes.IsSolved:
                    feedback.IsViewed = true;
                    feedback.IsSolved = true;
                    feedback.HasMarkedToFollowUpDate = false;
                    feedback.IsInProcess = false;
                    feedback.IsUnSolved = false;
                    break;
                case FeedbackStatusTypes.IsUnsolved:
                    feedback.IsViewed = true;
                    feedback.IsSolved = false;
                    feedback.HasMarkedToFollowUpDate = false;
                    feedback.IsInProcess = false;
                    feedback.IsUnSolved = true;
                    break;
                default:
                    feedback.IsViewed = true;
                    feedback.IsSolved = false;
                    feedback.HasMarkedToFollowUpDate = false;
                    feedback.IsInProcess = false;
                    feedback.IsUnSolved = false;
                    break;

            }
        }
    }
}