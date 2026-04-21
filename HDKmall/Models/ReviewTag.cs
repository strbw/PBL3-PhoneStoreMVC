using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HDKmall.Models
{
    public class ReviewTag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public ICollection<ReviewTagMapping> ReviewMappings { get; set; } = new List<ReviewTagMapping>();
    }
}
