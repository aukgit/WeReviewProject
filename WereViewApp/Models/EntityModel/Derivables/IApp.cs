using System;

namespace WeReviewApp.Models.EntityModel.Derivables {
    public interface IApp {
        string AppName { get; set; }
        byte PlatformID { get; set; }
        short CategoryID { get; set; }
        string Description { get; set; }
        string YoutubeEmbedLink { get; set; }
        string WebsiteUrl { get; set; }
        string StoreUrl { get; set; }
        Guid UploadGuid { get; set; }       
        string Url { get; set; }
    }
}
