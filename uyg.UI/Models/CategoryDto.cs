using System.ComponentModel.DataAnnotations;

namespace uyg.UI.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        public int NewsCount { get; set; }
    }
} 