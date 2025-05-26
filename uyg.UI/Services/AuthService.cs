using System.Net.Http.Json;
using System.Text.Json;
using Uyg.API.DTOs;

namespace uyg.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _baseUrl = configuration["ApiBaseURL"] ?? throw new ArgumentNullException(nameof(configuration), "ApiBaseURL is not configured");
        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/User/login", loginDto);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    using var jsonDoc = JsonDocument.Parse(responseContent);
                    var root = jsonDoc.RootElement;
                    
                    var loginResponse = new LoginResponseDto
                    {
                        Token = root.GetProperty("token").GetString(),
                        User = new UserDto
                        {
                            Id = root.GetProperty("user").GetProperty("id").GetString(),
                            UserName = root.GetProperty("user").GetProperty("userName").GetString(),
                            Email = root.GetProperty("user").GetProperty("email").GetString(),
                            FullName = root.GetProperty("user").GetProperty("fullName").GetString(),
                            Role = root.GetProperty("user").GetProperty("roles")[0].GetString()
                        }
                    };

                    return new ResponseDto<LoginResponseDto>
                    {
                        Success = true,
                        Data = loginResponse
                    };
                }

                return new ResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = $"Login failed. Status: {response.StatusCode}, Response: {responseContent}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDto<RegisterResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("User/register", registerDto);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseDto<RegisterResponseDto>>();
                    return result;
                }

                return new ResponseDto<RegisterResponseDto>
                {
                    Success = false,
                    Message = "Registration failed. Please try again."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<RegisterResponseDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public Task LogoutAsync()
        {
            // Implement logout logic here
            return Task.CompletedTask;
        }
    }
} 