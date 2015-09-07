using System.Collections.Generic;
using System.Xml.Linq;

namespace WereViewApp.Modules.Sitemaps {
    public interface ISitemapGenerator {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}