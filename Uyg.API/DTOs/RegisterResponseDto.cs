namespace Uyg.API.DTOs
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
    }
} 