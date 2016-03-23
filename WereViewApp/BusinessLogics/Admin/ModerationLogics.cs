using System.Data.Entity;
using System.Linq;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Mail;

namespace WereViewApp.BusinessLogics.Admin {
    public static class ModerationLogics {
        #region Featured App

        /// <summary>
        ///     Returns true when saving is successful.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="isFeatured"></param>
        /// <param name="isSendEmailWhenOperationIsSuccessful"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static bool AppFeatured(long appId, bool isFeatured, bool isSendEmailWhenOperationIsSuccessful,
            WereViewAppEntities db) {
            if (UserManager.IsAuthenticated()) {
                var featured = db.FeaturedImages.FirstOrDefault(n => n.AppID == appId);
                var user = UserManager.GetCurrentUser();
                if (featured == null) {
                    featured = new FeaturedImage {
                        AppID = appId,
                        IsFeatured = isFeatured,
                        UserID = user.UserID
                    };
                    db.FeaturedImages.Add(featured);
                } else {
                    featured.IsFeatured = isFeatured;
                    featured.UserID = user.UserID;
                    db.Entry(featured).State = EntityState.Modified;
                }
                var saved = db.SaveChanges() != -1;
                if (saved && isSendEmailWhenOperationIsSuccessful) {
                    var app = db.Apps.Find(appId);
                    if (app != null) {
                        string subject, body, appDisplayName;
                        appDisplayName = "Your app '" + app.AppName + "'";
                        if (featured.IsFeatured) {
                            subject = "Congrats! " + appDisplayName +
                                      " has been selected as featured app and will be display at our front page.";
                            body = "Hi , <br><br>" + appDisplayName +
                                   "has been selected as featured app and will be displayed at our front page. Your app url : " +
                                   app.GetAbsoluteUrl();
                        } else {
                            subject = "Sorry ! " + appDisplayName +
                                      " has been removed from featured app.";
                            body = "Hi , <br><br>" + appDisplayName +
                                   "has been removed from our front page featured apps list. Your app url : " +
                                   app.GetAbsoluteUrl();
                        }
                        SendAsyncEmailToUser(app.PostedByUserID, subject, body);
                    }
                }
            }
            return false;
        }

        #endregion

        #region Quick Async Email to user by Id

        public static void SendAsyncEmailToUser(long userId, string subject, string mailBody) {
            var user = UserManager.GetUser(userId);
            var mailer = new MailSender();
            mailer.Send(user.Email, subject, mailBody);
        }

        #endregion

        #region Blocking app and review

        public static bool BlockApp(long appId, bool isSendEmailWhenBlockIsSuccessful, WereViewAppEntities db) {
            var app = db.Apps.Find(appId);
            app.IsBlocked = true;
            app.Tags = "none";
            if (db.SaveChanges() > -1) {
                if (isSendEmailWhenBlockIsSuccessful) {
                    var subject = "Your app has been blocked.";
                    var mailBody = "Sorry! Your app <a href='" + app.GetAbsoluteUrl() + "'>" + app.AppName +
                                   "</a> is inappropriate thus blocked.";
                    SendAsyncEmailToUser(app.PostedByUserID, subject, mailBody);
                }
                return true;
            }
            return false;
        }

        public static bool UnBlockApp(long appId, bool isSendEmailWhenBlockIsSuccessful, WereViewAppEntities db) {
            var app = db.Apps.Find(appId);
            app.IsBlocked = false;
            app.Tags = "none";
            if (db.SaveChanges() > -1) {
                if (isSendEmailWhenBlockIsSuccessful) {
                    var subject = "Your app has been unblocked.";
                    var mailBody = "Congratulations! Your app <a href='" + app.GetAbsoluteUrl() + "'>" + app.AppName +
                                   "</a> is now unblocked.";
                    SendAsyncEmailToUser(app.PostedByUserID, subject, mailBody);
                }
                return true;
            }
            return false;
        }

        public static bool BlockReview(long reviewId, bool isSendEmailWhenBlockIsSuccessful, WereViewAppEntities db,
            out Review review) {
            review = db.Reviews.Find(reviewId);
            var likeDislikes = db.ReviewLikeDislikes.Where(n => n.ReviewID == reviewId);
            foreach (var likeDislike in likeDislikes) {
                db.ReviewLikeDislikes.Remove(likeDislike);
            }
            db.Reviews.Remove(review);
            if (db.SaveChanges() > -1) {
                if (isSendEmailWhenBlockIsSuccessful) {
                    var subject = "Your review has been removed.";
                    var mailBody = "Sorry! Your review <q>" + review.Comments + "</q> is inappropriate thus removed.";
                    SendAsyncEmailToUser(review.UserID, subject, mailBody);
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}