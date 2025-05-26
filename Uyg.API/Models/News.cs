using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uyg.API.Models
{
    public class News : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(500)]
        public string Summary { get; set; }

        [MaxLength(200)]
        public string ImageUrl { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; } = false;
        public int ViewCount { get; set; } = 0;

        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Media> Media { get; set; } = new List<Media>();
        public ICollection<Tag> TagList { get; set; } = new List<Tag>();
    }
} 