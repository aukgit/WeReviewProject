using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageResizer.Util;
using WereViewApp.Models.EntityModel;
using WereViewApp.Modules.Sitemaps;
using WereViewApp.WereViewAppCommon;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;

namespace WereViewApp.Controllers {
    public class SitemapController : Controller {
        // GET: Sitemap
        public ActionResult Index() {
            var modifiedDate = DateTime.Now;
            var appUrl = AppVar.Url;

            var sitemapItems = new List<SitemapItem>(300)
            {
                new SitemapItem(appUrl , modifiedDate, SitemapChangeFrequency.Daily,1),
                new SitemapItem(appUrl+"/Profile",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Apps",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/App/Post",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/App/Drafts",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Category",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Platforms",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/AboutUs",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Report",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Search",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Sitemap",modifiedDate, SitemapChangeFrequency.Daily),
                new SitemapItem(appUrl+"/Sitemap.xml",modifiedDate, SitemapChangeFrequency.Daily),
            
            };
            Algorithms algorithms = new Algorithms();
            using (var db = new WereViewAppEntities()) {
                var max = db.FeaturedImages.Count();
                var homePageGalleryApps = algorithms.GetHomePageGalleryImages(db, max);

                sitemapItems.AddRange(homePageGalleryApps.Select(app => new SitemapItem(app.GetAppUrl(), modifiedDate)));

                var latestApps = algorithms.GetLatestApps(db, 50);
                sitemapItems.AddRange(latestApps.Select(app => new SitemapItem(app.GetAppUrl(), modifiedDate)));
            }

            return new SitemapResult(sitemapItems);
        }
    }
}