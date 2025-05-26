using System.ComponentModel.DataAnnotations;

namespace Uyg.API.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
        public ICollection<News> News { get; set; }
    }
} 