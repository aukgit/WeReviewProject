using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace WeReviewApp.Modules.Sitemaps {
    /// <summary>
    ///     Generates an XML sitemap from a collection of <see cref="ISitemapItem" />
    /// </summary>
    public class SitemapResult : ActionResult {
        private readonly ISitemapGenerator generator;
        private readonly IEnumerable<ISitemapItem> items;

        public SitemapResult(IEnumerable<ISitemapItem> items) : this(items, new SitemapGenerator()) {
        }

        public SitemapResult(IEnumerable<ISitemapItem> items, ISitemapGenerator generator) {
            this.items = items;
            this.generator = generator;
        }

        public override void ExecuteResult(ControllerContext context) {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            using (var writer = new XmlTextWriter(response.Output)) {
                writer.Formatting = Formatting.Indented;
                var sitemap = generator.GenerateSiteMap(items);

                sitemap.WriteTo(writer);
            }
        }
    }
}