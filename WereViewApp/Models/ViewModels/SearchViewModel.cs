using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WereViewApp.Models.ViewModels {
    public class SearchViewModel {
        public string Url { get; set; }
        public string DisplaySearchText { get; set; }
        public string Category { get; set; }
        public string Platform { get; set; }
        public string PlatformVersion { get; set; }

        public bool isRatingCheck { get; set; }
        public bool isTagsCheck { get; set; }
        public bool isPlatform { get; set; }
    }
}