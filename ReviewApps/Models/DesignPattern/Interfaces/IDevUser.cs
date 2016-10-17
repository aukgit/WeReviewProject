namespace ReviewApps.Models.DesignPattern.Interfaces {
    interface IDevUser {
        long UserID { get; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
