﻿using System.Linq;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Mail;

namespace WereViewApp.WereViewAppCommon {
    public static class ModerationAlgorithms {

        #region Blocking app and review

        public static bool BlockApp(long appId, bool isSendEmailWhenBlockIsSuccessful, WereViewAppEntities db) {
            var app = db.Apps.Find(appId);
            app.IsBlocked = true;
            app.Tags = "none";
            if (db.SaveChanges() > -1) {
                if (isSendEmailWhenBlockIsSuccessful) {
                    string subject = "Your app has been blocked.";
                    string mailBody = "Sorry! Your app <a href='" + app.GetAbsoluteUrl() + "'>" + app.AppName + "</a> is inappropriate thus blocked.";
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
                    string subject = "Your app has been unblocked.";
                    string mailBody = "Congratulations! Your app <a href='" + app.GetAbsoluteUrl() + "'>" + app.AppName + "</a> is now unblocked.";
                    SendAsyncEmailToUser(app.PostedByUserID, subject, mailBody);
                }
                return true;
            }
            return false;
        }

        public static bool BlockReview(long reviewId, bool isSendEmailWhenBlockIsSuccessful, WereViewAppEntities db, out Review review) {
            review = db.Reviews.Find(reviewId);
            var likeDislikes = db.ReviewLikeDislikes.Where(n => n.ReviewID == reviewId);
            foreach (var likeDislike in likeDislikes) {
                db.ReviewLikeDislikes.Remove(likeDislike);
            }
            db.Reviews.Remove(review);
            if (db.SaveChanges() > -1) {
                if (isSendEmailWhenBlockIsSuccessful) {
                    string subject = "Your review has been removed.";
                    string mailBody = "Sorry! Your review <q>" + review.Comments + "</q> is inappropriate thus removed.";
                    SendAsyncEmailToUser(review.UserID, subject, mailBody);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Featured App

        public static bool AppFeatured(long appId, bool isFeatured, WereViewAppEntities db) {
            var featured = db.FeaturedImages.FirstOrDefault(n => n.AppID == appId);

            if (featured == null) {
                featured = new FeaturedImage() {
                    AppID = appId,
                    IsFeatured = isFeatured,
                    UserID
                }
            }
        }

        #endregion

        #region Quick Async Email to user by Id
        public static void SendAsyncEmailToUser(long userId, string subject, string mailBody) {
            var user = UserManager.GetUser(userId);
            var mailer = new MailSender();
            mailer.Send(user.Email, subject, mailBody);
        } 
        #endregion
    }
}