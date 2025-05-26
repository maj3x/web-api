using System.ComponentModel.DataAnnotations;

namespace uyg.UI.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public string Slug { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public int NewsCount { get; set; }

        public ICollection<News> News { get; set; } = new List<News>();
    }
} 