using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int? VariantId { get; set; }
        public ProductVariant? Variant { get; set; }
        public int Quantity { get; set; }
    }
}
