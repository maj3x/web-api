using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uyg.API.Models
{
    public class Media : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FileUrl { get; set; }

        [MaxLength(100)]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        [ForeignKey("News")]
        public int NewsId { get; set; }
        public News News { get; set; }
    }
} 