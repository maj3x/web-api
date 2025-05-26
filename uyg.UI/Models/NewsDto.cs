using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace uyg.UI.Models
{
    public class NewsDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }
        
        public int CategoryId { get; set; }
        
        [Required]
        public string CategoryName { get; set; }
        
        [Required]
        public string AuthorId { get; set; }
        
        [Required]
        public string AuthorName { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        
        public bool IsPublished { get; set; }
        
        public int ViewCount { get; set; }
        
        public List<string> Tags { get; set; } = new List<string>();
    }
} 