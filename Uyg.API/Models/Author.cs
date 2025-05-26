using System.ComponentModel.DataAnnotations;

namespace Uyg.API.Models
{
    public class Author : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        public string? PhotoUrl { get; set; }

        // Navigation property
        public virtual ICollection<News> News { get; set; } = new List<News>();
    }
} 