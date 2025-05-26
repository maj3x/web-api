using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Uyg.API.Models;

namespace Uyg.API.DTOs
{
    public class NewsCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Summary { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<Tag> TagList { get; set; } = new();

        public bool IsPublished { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
} 