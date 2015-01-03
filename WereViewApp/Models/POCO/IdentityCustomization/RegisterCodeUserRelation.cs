using System;

namespace WereViewApp.Models.POCO.IdentityCustomization {
    public class RegisterCodeUserRelation {
        public Guid RegisterCodeUserRelationID { get; set; }
        public long UserID { get; set; }
    }
}