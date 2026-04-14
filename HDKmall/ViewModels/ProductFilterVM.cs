namespace HDKmall.ViewModels
{
    public class ProductFilterVM
    {
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SearchQuery { get; set; }
        public string SortBy { get; set; } = "newest"; // newest, price-low, price-high, rating
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
