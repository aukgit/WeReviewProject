using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Helpers;
using System.Web.Mvc;
using DevMvcComponent;
using WeReviewApp.Models.POCO.IdentityCustomization;

namespace WeReviewApp.BusinessLogics.Admin {
    public class NavigationLogics {

 
        #region Save Navigation Order
        public T SaveOrder(NavigationItem[] navigationItems) {
              if (navigationItems == null ||  navigationItems.Length == 0) {
                return Json(new { success = false, error = "Failed due to empty elements send to the server." }, JsonRequestBehavior.AllowGet);
              }

            NavigationItem dbNavigationItem = null;
            var len = navigationItems.Length;
            List<string> navigationItemsNames = new List<string>(len),
                         navigationItemsFailedNames = new List<string>(len); ;
            bool isFailed = false;

            var navigation = 

            foreach (var navItem in navigationItems) {
                try {
                    dbNavigationItem = db.NavigationItems.Find(navItem.NavigationItemID);
                    db.Entry(dbNavigationItem).State = EntityState.Modified;
                    dbNavigationItem.Ordering = navItem.Ordering;
                    if (db.SaveChanges() > -1) {
                        navigationItemsNames.Add(dbNavigationItem.Title);
                    }
                } catch (Exception ex) {
                    Mvc.Error.ByEmail(ex, "SaveOrder()", "", dbNavigationItem);
                    isFailed = true;
                    navigationItemsFailedNames.Add(dbNavigationItem.Title);
                }
            }
            if (isFailed) {
                return Json(new { success = !isFailed, titles = navigationItemsFailedNames }, JsonRequestBehavior.AllowGet);
            } else {
                return Json(new { success = !isFailed, titles = navigationItemsNames }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}