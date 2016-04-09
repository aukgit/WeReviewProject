using System.Collections.Generic;
using System.Xml.Linq;

namespace WeReviewApp.Modules.Sitemaps {
    public interface ISitemapGenerator {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}