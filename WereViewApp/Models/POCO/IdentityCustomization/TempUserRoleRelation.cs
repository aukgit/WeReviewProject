using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WereViewApp.Models.POCO.IdentityCustomization {
    public class TempUserRoleRelation {
        public long TempUserRoleRelationID { get; set; }
        public long UserID { get; set; }
        public long UserRoleID { get; set; }

    }
}