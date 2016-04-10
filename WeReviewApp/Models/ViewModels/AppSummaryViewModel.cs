using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeReviewApp.Models.ViewModels {
    public class AppSummaryViewModel {
        public int TotalApps { get; set; }
        public int TotalDeveloper { get; set; }
        public int LastWeeksApps { get; set; }
        public int LastMonthsApps { get; set; }
    }
}