using System;

namespace ReviewApps.Modules.Uploads {
    public interface IUploadableFile {
        Guid UploadGuid { get; set; }
        //IImageCategory Category { get; }
        byte Sequence { get; set; }
        string Title { get; set; }
        string Subtitle { get; set; }
        string Extension { get; set; }
    }
}