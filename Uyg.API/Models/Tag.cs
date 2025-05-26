using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uyg.API.Models
{
    public class Tag : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; }

        public ICollection<News> News { get; set; } = new List<News>();
    }
} 