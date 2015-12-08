#region using block

using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DevMvcComponent.Pagination;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Modules;
using WereViewApp.Modules.DevUser;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {
    public class ReviewsController : AdvanceController {
        #region Declarations

        private readonly Algorithms algorithms = new Algorithms();

        #endregion



        #region Constructors

        public ReviewsController()
            : base(true) {
            ViewBag.controller = _controllerName;
        }

        #endregion

        #region View tapping

        /// <summary>
        ///     Always tap once before going into the view.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="Review">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        private bool ViewTapping(ViewStates view, Review review = null) {
            switch (view) {
                case ViewStates.Index:
                    break;
                case ViewStates.Create:
                    break;
                case ViewStates.CreatePost: // before saving it
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Details:
                    break;
                case ViewStates.EditPost: // before saving it
                    break;
                case ViewStates.Delete:
                    break;
            }
            return true;
        }

        #endregion

        #region Save database common method

        /// <summary>
        ///     Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="Review">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        private bool SaveDatabase(ViewStates view, Review review = null) {
            // working those at HttpPost time.
            switch (view) {
                case ViewStates.Create:
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Delete:
                    break;
            }

            try {
                var changes = db.SaveChanges(review);
                if (changes > 0) {
                    return true;
                }
            } catch (Exception ex) {
                throw new Exception("Message : " + ex.Message + " Inner Message : " + ex.InnerException.Message);
            }
            return false;
        }

        #endregion

        #region Display Review: user/reviews/id

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult User(string username, int page = 1) {
            ApplicationUser user;
            if (UserManager.IsUserNameExistWithValidation(username, out user)) {
                var reviews = db.Reviews.Where(n => n.UserID == user.UserID).OrderByDescending(n => n.ReviewID);

                var pageInfo = new PaginationInfo {
                    ItemsInPage = AppConfig.Setting.PageItems,
                    PageNumber = page,
                    PagesExists = -1
                };
                var pagedReviews = reviews.GetPageData(pageInfo, "user.review." + user.UserID).ToList();
                var eachUrl = "/user/reviews/" + user.UserName + "?page=@page";
                ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                    maxNumbersOfPagesShow: 8));
                ViewBag.user = user;
                ViewBag.currentUserlikeDislikes = algorithms.GetReviewsLikeDislikeBasedOnUser(pagedReviews, db);
                return View("User", pagedReviews);
            }
            return View("_404");
        }
        #endregion

      

        #region Like
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Like(long reviewId, long appId) {
            var userId = UserManager.GetLoggedUserId();
            var likeDislike = db.ReviewLikeDislikes.FirstOrDefault(n => n.ReviewID == reviewId && n.UserID == userId);
            var result = true;
            if (likeDislike == null) {
                var like = new ReviewLikeDislike();
                like.IsLiked = true;
                like.IsDisliked = false;
                like.IsNone = false;
                like.UserID = userId;
                like.ReviewID = reviewId;
                db.ReviewLikeDislikes.Add(like);
            } else {
                if (likeDislike.IsLiked) {
                    likeDislike.IsLiked = false;
                    likeDislike.IsNone = true;
                    result = false;
                } else {
                    likeDislike.IsLiked = true;
                    likeDislike.IsNone = false;
                }
                likeDislike.IsDisliked = false;
            }

            db.SaveChanges();
            algorithms.ForceAppReviewToLoad(appId);
            Thread.Sleep(1000);

            return Json(new { isDone = result }, "text/html");

        }

        #endregion

        #region Dilsike
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DisLike(long reviewId, long appId) {
            var userId = UserManager.GetLoggedUserId();
            var likeDislike = db.ReviewLikeDislikes.FirstOrDefault(n => n.ReviewID == reviewId && n.UserID == userId);
            var result = true;

            if (likeDislike == null) {
                var like = new ReviewLikeDislike();
                like.IsLiked = true;
                like.IsDisliked = false;
                like.IsNone = false;
                like.UserID = userId;
                like.ReviewID = reviewId;
                db.ReviewLikeDislikes.Add(like);
            } else {
                likeDislike.IsLiked = false;
                if (likeDislike.IsDisliked) {
                    likeDislike.IsDisliked = false;
                    likeDislike.IsNone = true;
                    result = false;
                } else {
                    likeDislike.IsDisliked = true;
                    likeDislike.IsNone = false;
                }
            }

            db.SaveChanges();
            Thread.Sleep(1000);
            algorithms.ForceAppReviewToLoad(appId);
            return Json(new { isDone = result }, "text/html");
        }

        #endregion

        #region Edit or modify record

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Review review) {
            var viewOf = ViewTapping(ViewStates.EditPost, review);
            AddNecessaryFields(review);
            if (ModelState.IsValid) {
                db.Entry(review).State = EntityState.Modified;
                var state = SaveDatabase(ViewStates.Edit, review);
                if (state) {
                    algorithms.AfterReviewIsSavedFixRatingNReviewCountInApp(review, false, db);
                    algorithms.ForceAppReviewToLoad(review.AppID);
                    return Json(new { isDone = true, msg = "Successful." }, JsonRequestBehavior.AllowGet); // return true;
                }
            }
            return Json(new { isDone = false, msg = "failed." }, JsonRequestBehavior.AllowGet); // return true;
        }

        #endregion

        #region Removing output cache

        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        #endregion

        #region Enums

        internal enum ViewStates {
            Index,
            Create,
            CreatePost,
            Edit,
            EditPost,
            Details,
            Delete,
            DeletePost,
            ReviewEdit
        }

        #endregion

        #region Developer Comments - Alim Ul karim

        // Generated by Alim Ul Karim on behalf of Developers Organism.
        // Find us developers-organism.com
        // https://www.facebook.com/DevelopersOrganism
        // mailto:alim@developers-organism.com		

        #endregion

        #region Constants

        private const string _deletedError =
            "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";

        private const string _deletedSaved = "Removed successfully.";
        private const string _editedSaved = "Modified successfully.";

        private const string _editedError =
            "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";

        private const string _createdError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        private const string _createdSaved = "Transaction is successfully added to the database.";
        private const string _controllerName = "Reviews";

        /// Constant value for where the controller is actually visible.
        private const string _controllerVisibleUrl = "";

        #endregion

        #region DropDowns Generate

        #endregion

        #region Index Find - Commented

        /*
        public ActionResult Index(System.Int64 id) {
            var reviews = db.Reviews.Include(r => r.App).Include(r => r.User).Where(n=> n. == id);
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(reviews.ToList());
        }
		*/

        #endregion

        #region Create or Add

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetReviewForm(long AppID) {
            if (!UserManager.IsAuthenticated()) {
                return PartialView("_LoginUrlPage");
            }
            var userId = UserManager.GetLoggedUserId();
            var isSameUser = db.Apps.Any(n => n.AppID == AppID && n.PostedByUserID == userId);
            if (isSameUser) {
                return View("ReviewOwnApp");
            }
            var review = algorithms.GetUserReviewedApp(AppID, db);
            if (review == null) {
                // not ever reviewed.
                var viewOf = ViewTapping(ViewStates.Create);
                review = new Review();
                review.AppID = AppID;
                review.Rating = 0;
                return View("Write", review);
            }
            // already reviewed the then send edit form
            return View("ReviewEdit", review);
        }

        /*
        public ActionResult Create(System.Int64 id) {        
            GetDropDowns(id); // Generate hidden.
            bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }
        */

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Write(Review review) {
            var viewOf = ViewTapping(ViewStates.CreatePost, review);
            //GetDropDowns(review);
            AddNecessaryFields(review);
            ModelState.Remove("CreatedDate");
            review.CreatedDate = DateTime.Now;
            if (ModelState.IsValid) {
                db.Reviews.Add(review);
                var state = SaveDatabase(ViewStates.Create, review);
                if (state) {
                    algorithms.AfterReviewIsSavedFixRatingNReviewCountInApp(review, true, db);
                    algorithms.ForceAppReviewToLoad(review.AppID);
                    AppVar.SetSavedStatus(ViewBag, _createdSaved); // Saved Successfully.          
                }

                return Json(new { isDone = true, msg = "Successful." }, JsonRequestBehavior.AllowGet); // return true;
            }

            return Json(new { isDone = false, msg = "failed." }, JsonRequestBehavior.AllowGet); // return true;
        }

        private void AddNecessaryFields(Review review) {
            review.UserID = UserManager.GetLoggedUserId();


            if (review.Comments != null) {
                review.Comment1 = review.Comments.GetStringCutOff(100);
                review.Comment2 = review.Comments.GetStringCutOff(100, 500);
            }
        }

        #endregion

        //#region Delete or remove record

        //[Authorize]
        //public ActionResult Delete(long id) {
        //    var review = db.Reviews.Find(id);
        //    var viewOf = ViewTapping(ViewStates.Delete, review);
        //    return View(review);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult DeleteConfirmed(long id) {
        //    var review = db.Reviews.Find(id);
        //    var viewOf = ViewTapping(ViewStates.DeletePost, review);
        //    db.Reviews.Remove(review);
        //    var state = SaveDatabase(ViewStates.Delete, review);
        //    if (!state) {
        //        AppVar.SetErrorStatus(ViewBag, _deletedError); // Failed to Save
        //        return View(review);
        //    }

        //    return RedirectToAction("Index");
        //}

        //#endregion

      
    }
}