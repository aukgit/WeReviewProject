using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereViewApp.Models.EntityModel.Derivables {
    public interface IApp {
        string AppName { get; set; }
        byte PlatformID { get; set; }
        short CategoryID { get; set; }
        string Description { get; set; }
        string YoutubeEmbedLink { get; set; }
        string WebSiteURL { get; set; }
        string StoreURL { get; set; }
        Guid UploadGuid { get; set; }       
        string URL { get; set; }
    }
}
