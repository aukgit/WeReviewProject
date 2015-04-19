namespace WereViewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using WereViewApp.Models.EntityModel.Derivables;

    public partial class AppDraft : IApp
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
        public string WebSiteURL { get; set; }
        public string StoreURL { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsPublished { get; set; }
        public double? PlatformVersion { get; set; }
        public Guid UploadGuid { get; set; }
        public long? TotalViewed { get; set; }
        public string URL { get; set; }

        public DateTime ReleaseDate { get; set; }






    }
}
