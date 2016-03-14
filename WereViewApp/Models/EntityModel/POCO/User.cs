using System.Collections.Generic;
using WereViewApp.Models.DesignPattern.Interfaces;

namespace WereViewApp.Models.EntityModel {
    public class User:IDevUser {
        public User()
        {
            this.Apps = new HashSet<App>();
            this.CellPhones = new HashSet<CellPhone>();
            this.FeaturedImages = new HashSet<FeaturedImage>();
            this.Messages = new HashSet<Message>();
            this.Messages1 = new HashSet<Message>();
            this.MessageSeens = new HashSet<MessageSeen>();
            this.MessageSeens1 = new HashSet<MessageSeen>();
            this.Notifications = new HashSet<Notification>();
            this.NotificationSeens = new HashSet<NotificationSeen>();
            this.Reviews = new HashSet<Review>();
            this.ReviewLikeDislikes = new HashSet<ReviewLikeDislike>();
            this.Subscribes = new HashSet<Subscribe>();
            this.Subscribes1 = new HashSet<Subscribe>();
            this.UserPoints = new HashSet<UserPoint>();
        }

        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }

        public long TotalEarnedPoints { get; set; }
        public long? DefaultCellPhoneID { get; set; }
        public long SubscriberCount { get; set; }
        public System.Guid UploadGuid { get; set; }
        public bool HasPicture { get; set; }
        public virtual ICollection<App> Apps { get; set; }
        public virtual ICollection<CellPhone> CellPhones { get; set; }
        public virtual CellPhone CellPhone { get; set; }
        
        public virtual ICollection<FeaturedImage> FeaturedImages { get; set; }
        
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> Messages1 { get; set; }
        public virtual ICollection<MessageSeen> MessageSeens { get; set; }
        public virtual ICollection<MessageSeen> MessageSeens1 { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        
        public virtual ICollection<NotificationSeen> NotificationSeens { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ReviewLikeDislike> ReviewLikeDislikes { get; set; }
        public virtual ICollection<Subscribe> Subscribes { get; set; }
        
        public virtual ICollection<Subscribe> Subscribes1 { get; set; }
        public virtual ICollection<UserPoint> UserPoints { get; set; }
    }
}
