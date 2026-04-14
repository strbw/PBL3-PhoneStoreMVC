using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HDKmall.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? SessionId { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
