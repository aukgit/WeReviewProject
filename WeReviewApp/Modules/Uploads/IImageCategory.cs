namespace WeReviewApp.Modules.Uploads {
    public interface IImageCategory {
        string CategoryName { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}