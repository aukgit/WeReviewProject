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
    [OutputCache(CacheProfile = "Day", VaryByCustom = "none")]
    public class SitemapController : Controller {
        // GET: Sitemap
        public ActionResult Index() {
            var modifiedDate = DateTime.Now;
            var appUrl = AppVar.Url;

            var sitemapItems = new List<SitemapItem>(900) {
                new SitemapItem(appUrl, modifiedDate, SitemapChangeFrequency.Daily, 1),
                new SitemapItem(appUrl + "/profiles", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/apps", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/app/post", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/app/drafts", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/apps/category", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/platforms", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/about", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/report", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/search", modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl + "/sitemap", modifiedDate, SitemapChangeFrequency.Daily)
                //new SitemapItem(appUrl+"/Sitemap.xml",modifiedDate, SitemapChangeFrequency.Daily),
            };
            var algorithms = new Algorithms.Algorithms();
            using (var db = new WereViewAppEntities()) {
                var max = db.FeaturedImages.Count();
                var homePageGalleryApps = algorithms.GetHomePageGalleryImages(db, max);

                sitemapItems.AddRange(
                    homePageGalleryApps.Select(app => new SitemapItem(app.GetAbsoluteUrl(), modifiedDate)));

                var latestApps = algorithms.GetLatestApps(db, 50);
                if (latestApps != null) {
                    sitemapItems.AddRange(latestApps.Select(app => new SitemapItem(app.GetAbsoluteUrl(), modifiedDate)));
                }

                var topApps = algorithms.GetTopRatedApps(db, 50);
                if (topApps != null) {
                    sitemapItems.AddRange(topApps.Select(app => new SitemapItem(app.GetAbsoluteUrl(), modifiedDate)));
                }

                var categories = WereViewStatics.AppCategoriesCache;
                sitemapItems.AddRange(
                    categories.Select(
                        category => new SitemapItem(appUrl + "/apps/category/" + category.Slug, modifiedDate)));

                var top30Developers = algorithms.GetTopDevelopers(db);
                sitemapItems.AddRange(
                    top30Developers.Select(
                        developer => new SitemapItem(appUrl + "/profiles/" + developer, modifiedDate)));
            }

            return new SitemapResult(sitemapItems);
        }
    }
}