namespace WeReviewApp.Models.DesignPattern.Interfaces {
    interface IDevUserRole {
        long Id { get; set; }

        string Name { get; set; }
        byte PriorityLevel { get; set; }
    }
}
