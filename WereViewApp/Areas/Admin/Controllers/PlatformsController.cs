﻿using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Controllers;
using WereViewApp.Models.EntityModel;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Areas.Admin.Controllers
{
    public class PlatformsController : AdvanceController {

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
		const string _controllerName = "Platforms";
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
		
		public PlatformsController(): base(true){
			ViewBag.controller = _controllerName;
		} 

		#endregion
		
		#region View tapping
		/// <summary>
        /// Always tap once before going into the view.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="Platform">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
		bool ViewTapping(ViewStates view, Platform platform = null){
			switch (view){
				case ViewStates.Index:
					break;
				case ViewStates.Create:
					break;
				case ViewStates.CreatePost: // before saving it
                    WereViewStatics.RefreshCaches();
					break;
				case ViewStates.Edit:
					break;
				case ViewStates.Details:
					break;
				case ViewStates.EditPost: // before saving it
                    WereViewStatics.RefreshCaches();
					break;
				case ViewStates.Delete:
                    WereViewStatics.RefreshCaches();
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
        /// <param name="Platform">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
		bool SaveDatabase(ViewStates view, Platform platform = null){
			// working those at HttpPost time.
			switch (view){
				case ViewStates.Create:
					break;
				case ViewStates.Edit:
					break;
				case ViewStates.Delete:
					break;
			}

			try	{
				var changes = db.SaveChanges(platform);
				if(changes > 0){
					return true;
				}
			} catch (Exception ex){
				 throw new Exception("Message : " + ex.Message.ToString() + " Inner Message : " + ex.InnerException.Message.ToString());
			}
			return false;
		}
		#endregion

		#region DropDowns Generate

		public void GetDropDowns(Platform platform = null){
			
		}

		public void GetDropDowns(System.Byte id){			
		}
		#endregion

		#region Index
        public ActionResult Index() { 
        
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(db.Platforms.ToList());
        }
		#endregion

		#region Index Find - Commented
		/*
        public ActionResult Index(System.Byte id) {
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(db.Platforms.Where(n=> n. == id).ToList());
        }
		*/
		#endregion

		#region Details
        public ActionResult Details(System.Byte id) {
        
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var platform = db.Platforms.Find(id);
            if (platform == null)
            {
                return HttpNotFound();
            }
			bool viewOf = ViewTapping(ViewStates.Details, platform);
            return View(platform);
        }
		#endregion

		#region Create or Add
        public ActionResult Create() {        
			GetDropDowns();
			bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }

		/*
		public ActionResult Create(System.Byte id) {        
			GetDropDowns(id); // Generate hidden.
			bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }
		*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Platform platform) {
			bool viewOf = ViewTapping(ViewStates.CreatePost, platform);
			GetDropDowns(platform);
            if (ModelState.IsValid) {            
                db.Platforms.Add(platform);
                bool state = SaveDatabase(ViewStates.Create, platform);
				if (state) {					
					AppVar.SetSavedStatus(ViewBag,_createdSaved); // Saved Successfully.
				} else {					
					AppVar.SetErrorStatus(ViewBag,_createdError); // Failed to save
				}
				
                return View(platform);
            }			
			AppVar.SetErrorStatus(ViewBag,_createdError); // Failed to Save
            return View(platform);
        }
		#endregion

        #region Edit or modify record
        public ActionResult Edit(System.Byte id) {
        
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var platform = db.Platforms.Find(id);
            if (platform == null)
            {
                return HttpNotFound();
            }
			bool viewOf = ViewTapping(ViewStates.Edit, platform);
			GetDropDowns(platform); // Generating drop downs
            return View(platform);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Platform platform) {
			bool viewOf = ViewTapping(ViewStates.EditPost, platform);
            if (ModelState.IsValid)
            {
                db.Entry(platform).State = EntityState.Modified;
                bool state = SaveDatabase(ViewStates.Edit, platform);
				if (state) {					
					AppVar.SetSavedStatus(ViewBag, _editedSaved); // Saved Successfully.
				} else {					
					AppVar.SetErrorStatus(ViewBag, _editedError); // Failed to Save
				}
				
                return RedirectToAction("Index");
            }

        	GetDropDowns(platform);
            AppVar.SetErrorStatus(ViewBag, _editedError); // Failed to save
            return View(platform);
        }
		#endregion

		#region Delete or remove record

		
        public ActionResult Delete(byte id) {
        
            var platform = db.Platforms.Find(id);
            bool viewOf = ViewTapping(ViewStates.Delete, platform);
			return View(platform);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]		
        public ActionResult DeleteConfirmed(byte id) {
            var platform = db.Platforms.Find(id);
			bool viewOf = ViewTapping(ViewStates.DeletePost, platform);
            db.Platforms.Remove(platform);
            bool state = SaveDatabase(ViewStates.Delete, platform);
			if (!state) {			
				AppVar.SetErrorStatus(ViewBag, _deletedError); // Failed to Save
				return View(platform);
			}
			
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
