namespace Uyg.API.DTOs
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
        public List<string> Errors { get; set; } = new();
    }
} 