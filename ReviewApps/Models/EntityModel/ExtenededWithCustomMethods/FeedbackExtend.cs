using System.Linq;
using ReviewApps.Common;
using ReviewApps.Models.POCO.Enum;
using ReviewApps.Models.POCO.IdentityCustomization;

namespace ReviewApps.Models.EntityModel.ExtenededWithCustomMethods {
    public static class FeedbackExtend {
        public static FeedbackState GetStatus(this Feedback feedback) {
            if (!feedback.IsViewed) {
                return new FeedbackState {
                    Status = "Not viewed yet.",
                    StyleClass = "warning"
                };
            }
            if (feedback.IsInProcess) {
                return new FeedbackState {
                    Status = "Is in process!",
                    StyleClass = "warning"
                };
            }
            if (feedback.HasMarkedToFollowUpDate) {
                return new FeedbackState {
                    Status = "Has a follow up.",
                    StyleClass = "info"
                };
            }
            if (feedback.IsUnSolved) {
                return new FeedbackState {
                    Status = "Unsolved feedback.",
                    StyleClass = "danger"
                };
            }
            if (feedback.IsSolved) {
                return new FeedbackState {
                    Status = "Solved",
                    StyleClass = "success"
                };
            }
            return new FeedbackState {
                Status = "Viewed but not replied.",
                StyleClass = "warning"
            };
        }

        public static string GetCategory(this Feedback feedback) {
            var feedbackCategory =
                Statics.FeedbackCategories.FirstOrDefault(n => n.FeedbackCategoryID == feedback.FeedbackCategoryID);
            if (feedbackCategory != null) {
                return feedbackCategory.Category;
            }
            return "";
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
                case FeedbackStatusTypes.IsNonViewed:
                    feedback.IsViewed = false;
                    feedback.IsSolved = false;
                    feedback.HasMarkedToFollowUpDate = false;
                    feedback.IsInProcess = false;
                    feedback.IsUnSolved = false;
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