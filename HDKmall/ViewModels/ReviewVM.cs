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
        public bool IsApproved { get; set; }
        public bool IsHidden { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
    }
}
