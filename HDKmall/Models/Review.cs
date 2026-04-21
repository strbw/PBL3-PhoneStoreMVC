using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace HDKmall.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false;
        public bool IsHidden { get; set; } = false;
        public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();
        public ICollection<ReviewTagMapping> TagMappings { get; set; } = new List<ReviewTagMapping>();
    }
}
