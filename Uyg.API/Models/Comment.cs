using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uyg.API.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [ForeignKey("News")]
        public int NewsId { get; set; }
        public News News { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }

        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
} 