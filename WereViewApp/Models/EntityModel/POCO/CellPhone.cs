using System.Collections.Generic;

namespace WereViewApp.Models.EntityModel {
    public partial class CellPhone {
        public CellPhone() {
            Users = new HashSet<User>();
        }

        public long CellPhoneID { get; set; }
        public long UserID { get; set; }
        public byte PlatformID { get; set; }
        public double PlatformVersion { get; set; }

        public virtual Platform Platform { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}