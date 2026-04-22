namespace HDKmall.ViewModels
{
    public class ReviewVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Tags { get; set; }      // JSON array string  
        public string? ImageUrl { get; set; }    // Cloudinary URL
        public string Status { get; set; } = "Pending"; // Pending/Approved/Hidden
    }
}
