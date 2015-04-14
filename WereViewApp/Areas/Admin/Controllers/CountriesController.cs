using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.Cache;

namespace WereViewApp.Areas.Admin.Controllers {
    public class CountriesController : Controller {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index() {
            return View(CachedQueriedData.GetCountries().ToList());
        }

        public ActionResult Edit(System.Int32 id) {
            var zones = CachedQueriedData.GetTimezones(id);
            ViewBag.Timezone = new SelectList(db.UserTimeZones.ToList(), "UserTimeZoneID", "Display");
            ViewBag.CountryID = id;
            ViewBag.CountryName = db.Countries.Find(id).DisplayCountryName + " - " + db.Countries.Find(id).Alpha2Code;
            return View(zones);
        }

        public ActionResult Delete(System.Int32 id) {
            var timezone = db.UserTimeZones.Find(id);
            db.UserTimeZones.Remove(timezone);
            db.SaveChanges();
            return RedirectToActionPermanent("Edit", new { id = id });
        }

        [HttpPost]
        public ActionResult Edit(int CountryID, int Timezone, bool hasMultiple) {
            var country = db.Countries.Find(CountryID);

            var foundTimeZone = db.UserTimeZones.Find(Timezone);
            if (foundTimeZone != null) {
                var addRelation = new CountryTimezoneRelation() {
                    CountryID = country.CountryID,
                    UserTimeZoneID = foundTimeZone.UserTimeZoneID
                };
                var anyExist = db.CountryTimezoneRelations.Any(n => n.UserTimeZoneID == addRelation.UserTimeZoneID && n.CountryID == addRelation.CountryID);

                if (!anyExist) {
                    //not exist then add
                    db.CountryTimezoneRelations.Add(addRelation);
                    country.RelatedTimeZoneID = addRelation.UserTimeZoneID;
                }

                country.IsSingleTimeZone = !hasMultiple;
                country.RelatedTimeZoneID = addRelation.UserTimeZoneID;
                db.SaveChanges();
            }
            var zones = CachedQueriedData.GetTimezones(CountryID);
            ViewBag.Timezone = new SelectList(db.UserTimeZones.ToList(), "UserTimeZoneID", "Display");
            ViewBag.CountryID = CountryID;
            ViewBag.CountryName = db.Countries.Find(CountryID).DisplayCountryName + " - " + db.Countries.Find(CountryID).Alpha2Code;

            return View(zones);
        }


        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
