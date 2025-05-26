using System.ComponentModel.DataAnnotations;

namespace Uyg.API.DTOs
{
    public class UserDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
