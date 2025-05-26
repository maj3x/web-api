using Uyg.API.DTOs;

namespace uyg.UI.Services
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ResponseDto<RegisterResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task LogoutAsync();
    }
} 