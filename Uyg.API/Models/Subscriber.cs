using System.ComponentModel.DataAnnotations;

namespace Uyg.API.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsVerified { get; set; } = false;

        public DateTime? LastEmailSent { get; set; }
        public string VerificationToken { get; set; }
    }
} 