using System;
using WereViewApp.Models.EntityModel.Derivables;

namespace WereViewApp.Models.EntityModel
{
    public class AppDraft : IApp
    {
        public long AppDraftID { get; set; }
        public string AppName { get; set; }
        public byte PlatformID { get; set; }
        public short CategoryID { get; set; }
        public string Description { get; set; }
        public long PostedByUserID { get; set; }
        public short? ReviewsCount { get; set; }
        public bool? IsVideoExist { get; set; }
        public string YoutubeEmbedLink { get; set; }
        public string WebsiteUrl { get; set; }
        public string StoreUrl { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsPublished { get; set; }
        public double? PlatformVersion { get; set; }
        public Guid UploadGuid { get; set; }
        public long? TotalViewed { get; set; }
        public string Url { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public bool IsMultipleVersion { get; set; }
        public string TagsDisplay { get; set; }
        public string SupportedOSVersions { get; set; }



    }
}
