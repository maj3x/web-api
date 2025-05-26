using System.ComponentModel.DataAnnotations;

namespace Uyg.API.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
} 