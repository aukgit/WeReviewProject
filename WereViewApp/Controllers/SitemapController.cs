#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Modules.Sitemaps;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {
    public class SitemapController : Controller {
        // GET: Sitemap
        public ActionResult Index() {
            var modifiedDate = DateTime.Now;
            var appUrl = AppVar.Url;

            var sitemapItems = new List<SitemapItem>(300) {
                new SitemapItem(appUrl, modifiedDate, SitemapChangeFrequency.Daily, 1),
                new SitemapItem(appUrl + "/Profile", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/Apps", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/App/Post", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/App/Drafts", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/Apps/Category", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/Platforms", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/AboutUs", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/Report", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/Search", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/Sitemap", modifiedDate, SitemapChangeFrequency.Daily)
                //new SitemapItem(appUrl+"/Sitemap.xml",modifiedDate, SitemapChangeFrequency.Daily),
            };
            var algorithms = new Algorithms();
            using (var db = new WereViewAppEntities()) {
                var max = db.FeaturedImages.Count();
                var homePageGalleryApps = algorithms.GetHomePageGalleryImages(db, max);

                sitemapItems.AddRange(homePageGalleryApps.Select(app => new SitemapItem(app.GetAppUrl(), modifiedDate)));

                var latestApps = algorithms.GetLatestApps(db, 50);
                sitemapItems.AddRange(latestApps.Select(app => new SitemapItem(app.GetAppUrl(), modifiedDate)));

                var categories = WereViewStatics.AppCategoriesCache;
                sitemapItems.AddRange(
                    categories.Select(
                        category => new SitemapItem(appUrl + "/Apps/Category/" + category.CategoryName, modifiedDate)));
            }

            return new SitemapResult(sitemapItems);
        }
    }
}