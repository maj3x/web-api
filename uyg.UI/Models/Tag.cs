using System.ComponentModel.DataAnnotations;

namespace uyg.UI.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        public ICollection<News> News { get; set; } = new List<News>();
    }
} 