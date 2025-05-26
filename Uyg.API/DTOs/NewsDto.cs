using System.ComponentModel.DataAnnotations;
using Uyg.API.Models;

namespace Uyg.API.DTOs
{
    public class NewsDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string Summary { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        [Required]
        public string AuthorName { get; set; } = string.Empty;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }
        public int ViewCount { get; set; }

        [Required]
        public List<Tag> TagList { get; set; } = new();
    }
} 