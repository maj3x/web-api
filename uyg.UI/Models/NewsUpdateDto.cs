using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace uyg.UI.Models
{
    public class NewsUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }

        public bool IsPublished { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
} 