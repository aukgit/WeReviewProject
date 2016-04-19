using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DevMvcComponent;
using WeReviewApp.BusinessLogics.Component;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.IdentityCustomization;

namespace WeReviewApp.BusinessLogics.Admin {
    public class NavigationLogics : BaseLogicComponent<ApplicationDbContext> {
        public NavigationLogics(ApplicationDbContext db)
            : base(db) {
            this.db = db;
        }

        #region Save Navigation Order

        /// <summary>
        ///     Finds the navigation and puts -1 to the existing ordering value.
        ///     Then finally marked the old and new one as changed.
        /// </summary>
        /// <param name="changed"></param>
        /// <param name="cachedItems"></param>
        /// <returns>Returns true if already same order exist.</returns>
        private bool PlaceOrderToNegativeOnExistingOrderAndMarkAsChanged(NavigationItem changed,
            List<NavigationItem> cachedItems) {
            var sameOrderItem = cachedItems.FirstOrDefault(n =>
                                                           n.Ordering == changed.Ordering &&
                                                           n.NavigationItemID != changed.NavigationItemID);
            if (sameOrderItem != null) {
                sameOrderItem.Ordering = -1;
                MarkpNavigationAsChanged(sameOrderItem);
                return true;
            }
            return false;
        }

        private void MarkpNavigationAsChanged(NavigationItem changed) {
            db.Entry(changed).State = EntityState.Modified;
        }

        private void MarkpNavigationAsAdded(NavigationItem add) {
            db.Entry(add).State = EntityState.Added;
        }

        public bool SaveOrder(NavigationItem[] navigationItems) {
            if (navigationItems == null || navigationItems.Length == 0) {
                return true;
            }

            NavigationItem dbNavigationItem = null;

            var firstItem = navigationItems.FirstOrDefault();
            var navigationItemsList =
                db.NavigationItems.Where(n => n.NavigationID == firstItem.NavigationID)
                  .OrderBy(n => n.Ordering)
                  .ToList();

            foreach (var changedNavItem in navigationItems) {
                try {
                    var navItem =
                        navigationItemsList.FirstOrDefault(n => n.NavigationItemID == changedNavItem.NavigationItemID);
                    navItem.Ordering = changedNavItem.Ordering;
                    PlaceOrderToNegativeOnExistingOrderAndMarkAsChanged(navItem, navigationItemsList);
                } catch (Exception ex) {
                    Mvc.Error.ByEmail(ex, "SaveOrder()", "", dbNavigationItem);
                }
            }
            var maxOrder = navigationItemsList.Max(n => n.Ordering);
			var orderChangingItems = navigationItemsList.Where(n=> n.Ordering <= 0).OrderByDescending(n=> n.NavigationItemID);
            foreach (var changedNavItem in orderChangingItems) {
                changedNavItem.Ordering = ++maxOrder;
            }
            return db.SaveChanges() > -1;
        }

        #endregion
    }
}