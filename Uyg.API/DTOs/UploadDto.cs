using System.ComponentModel.DataAnnotations;

namespace Uyg.API.DTOs
{
    public class UploadDto
    {
        [Required]
        public string PicData { get; set; } = string.Empty;

        [Required]
        public string PicExt { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
