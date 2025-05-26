using System.ComponentModel.DataAnnotations;

namespace uyg.UI.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string Summary { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<Tag> TagList { get; set; } = new();

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedAt { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsPublished { get; set; } = false;

        public int ViewCount { get; set; } = 0;

        public string AuthorId { get; set; } = string.Empty;
        public AppUser? Author { get; set; }
    }
} 