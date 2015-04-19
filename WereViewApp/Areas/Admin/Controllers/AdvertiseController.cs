﻿using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Controllers;
using WereViewApp.Models.EntityModel;
using WereViewApp.Modules.Uploads;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Areas.Admin.Controllers {
    public class AdvertiseController : AdvanceController {

        #region Developer Comments - Alim Ul karim
        // Generated by Alim Ul Karim on behalf of Developers Organism.
        // Find us developers-organism.com
        // https://www.facebook.com/DevelopersOrganism
        // mailto:alim@developers-organism.com		
        #endregion

        #region Constants

        const string _deletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        const string _deletedSaved = "Removed successfully.";
        const string _editedSaved = "Modified successfully.";
        const string _editedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        const string _createdError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        const string _createdSaved = "Transaction is successfully added to the database.";
        const string _controllerName = "Advertise";
        ///Constant value for where the controller is actually visible.
        const string _controllerVisibleUrl = "";

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
            DeletePost
        }

        #endregion

        #region Constructors

        public AdvertiseController()
            : base(true) {
            ViewBag.controller = _controllerName;
        }

        #endregion

        #region View tapping
        /// <summary>
        /// Always tap once before going into the view.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="Gallery">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        bool ViewTapping(ViewStates view, Gallery gallery = null) {
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
        /// Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="Gallery">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        bool SaveDatabase(ViewStates view, Gallery gallery = null) {
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
                var changes = db.SaveChanges(gallery);
                if (changes > 0) {
                    return true;
                }
            } catch (Exception ex) {
                throw new Exception("Message : " + ex.Message.ToString() + " Inner Message : " + ex.InnerException.Message.ToString());
            }
            return false;
        }
        #endregion

        #region DropDowns Generate

        public void GetDropDowns(Gallery gallery = null) {
            if (gallery != null) {
                ViewBag.GalleryCategoryID = new SelectList(db.GalleryCategories.Where(n => n.IsAdvertise).ToList(), "GalleryCategoryID", "CategoryName", gallery.GalleryCategoryID);
            } else {
                ViewBag.GalleryCategoryID = new SelectList(db.GalleryCategories.Where(n => n.IsAdvertise).ToList(), "GalleryCategoryID", "CategoryName");
            }

        }

        public void GetDropDowns(System.Guid id) {
            ViewBag.GalleryCategoryID = new SelectList(db.GalleryCategories, "GalleryCategoryID", "CategoryName");
        }
        #endregion

        #region Index
        public ActionResult Index() {
            var galleries = db.Galleries.Include(g => g.GalleryCategory).Where(n => n.GalleryCategory.IsAdvertise);
            bool viewOf = ViewTapping(ViewStates.Index);
            return View(galleries.ToList());
        }
        #endregion

        #region Index Find - Commented
        /*
        public ActionResult Index(System.Guid id) {
            var galleries = db.Galleries.Include(g => g.GalleryCategory).Where(n=> n. == id);
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(galleries.ToList());
        }
		*/
        #endregion

        #region Details
        public ActionResult Details(System.Guid id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gallery = db.Galleries.Include(n => n.GalleryCategory).FirstOrDefault(n => n.GalleryID == id);
            if (gallery == null) {
                return HttpNotFound();
            }
            bool viewOf = ViewTapping(ViewStates.Details, gallery);
            return View(gallery);
        }
        #endregion

        #region Create or Add
        public ActionResult Create() {
            GetDropDowns();
            bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }

        /*
        public ActionResult Create(System.Guid id) {        
            GetDropDowns(id); // Generate hidden.
            bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gallery gallery) {
            bool viewOf = ViewTapping(ViewStates.CreatePost, gallery);
            if (gallery.File != null) {
                gallery.Extension = UploadProcessor.GetExtension(gallery.File);
            }
            GetDropDowns(gallery);
            if (ModelState.IsValid) {
                gallery.GalleryID = Guid.NewGuid();
                gallery.UploadGuid = gallery.GalleryID;
                db.Galleries.Add(gallery);
                bool state = SaveDatabase(ViewStates.Create, gallery);
                DoUpload(gallery);
                if (state) {
                    AppVar.SetSavedStatus(ViewBag, _createdSaved); // Saved Successfully.
                } else {
                    AppVar.SetErrorStatus(ViewBag, _createdError); // Failed to save
                }

                return View(gallery);
            }
            AppVar.SetErrorStatus(ViewBag, _createdError); // Failed to Save
            return View(gallery);
        }
        #endregion

  

        #region Edit or modify record
        public ActionResult Edit(System.Guid id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gallery = db.Galleries.Find(id);
            if (gallery == null) {
                return HttpNotFound();
            }
            bool viewOf = ViewTapping(ViewStates.Edit, gallery);
            GetDropDowns(gallery); // Generating drop downs
            return View(gallery);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Gallery gallery) {
            bool viewOf = ViewTapping(ViewStates.EditPost, gallery);
            if (gallery.File != null) {
                gallery.Extension = UploadProcessor.GetExtension(gallery.File);
            }
            if (ModelState.IsValid) {
                db.Entry(gallery).State = EntityState.Modified;
                bool state = SaveDatabase(ViewStates.Edit, gallery);
                DoUpload(gallery);   
                if (state) {
                    AppVar.SetSavedStatus(ViewBag, _editedSaved); // Saved Successfully.
                } else {
                    AppVar.SetErrorStatus(ViewBag, _editedError); // Failed to Save
                }

                return RedirectToAction("Index");
            }

            GetDropDowns(gallery);
            AppVar.SetErrorStatus(ViewBag, _editedError); // Failed to save
            return View(gallery);
        }
        #endregion

        #region Do Upload
        /// <summary>
        /// Upload n then resize
        /// </summary>
        /// <param name="gallery"></param>
        public void DoUpload(Gallery gallery) {

            WereViewStatics.UProcessorAdvertiseImages.UploadFile(gallery.File, gallery.UploadGuid.ToString(), -1, false, true);
            ProcessImageAsyc(gallery);
            
        }

        #region Process image
        public void ProcessImageAsyc(Gallery gallery) {
            new Thread(() => {
          
                using (var db2 = new WereViewAppEntities()) {
                    var category = db2.GalleryCategories.Find(gallery.GalleryCategoryID);
                    if (category != null) {         
                        WereViewStatics.UProcessorAdvertiseImages.ProcessImage(gallery,category);
                        WereViewStatics.UProcessorAdvertiseImages.RemoveTempImage(gallery, true);
                    }
                }
            }).Start();
        }
        #endregion
        #endregion

        #region Delete or remove record


        public ActionResult Delete(Guid id) {

            var gallery = db.Galleries.Include(n=> n.GalleryCategory).FirstOrDefault(n=> n.GalleryID == id);
            bool viewOf = ViewTapping(ViewStates.Delete, gallery);
            return View(gallery);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id) {
            var gallery = db.Galleries.Find(id);
            bool viewOf = ViewTapping(ViewStates.DeletePost, gallery);
            db.Galleries.Remove(gallery);

            bool state = SaveDatabase(ViewStates.Delete, gallery);
            if (!state) {
                AppVar.SetErrorStatus(ViewBag, _deletedError); // Failed to Save

                return View(gallery);
            }
            WereViewStatics.UProcessorAdvertiseImages.RemoveTempImage(gallery, false);

            return RedirectToAction("Index");
        }
        #endregion

        #region Removing output cache
        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }
        #endregion
    }


}
