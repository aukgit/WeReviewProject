using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeReviewApp.Models.POCO.IdentityCustomization;

namespace WeReviewApp.Common.ResultsType {
    public class NavigationMessage {
        public int NavigationID { get; set; }
        public List<NavigationItem> NavigationItems { get; set; }
        public List<NavigationItem> NavigationItemsFailed { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}