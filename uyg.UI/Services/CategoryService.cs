using System.Net.Http.Json;
using Uyg.API.DTOs;
using Microsoft.Extensions.Configuration;

namespace uyg.UI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CategoryService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new ArgumentNullException("ApiSettings:BaseUrl");
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<CategoryDto>>>($"{_baseUrl}/api/categories");
            return response?.Data ?? new List<CategoryDto>();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<CategoryDto>>($"{_baseUrl}/api/categories/{id}");
            return response?.Data;
        }

        public async Task<bool> CreateAsync(CategoryDto categoryDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/categories", categoryDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(CategoryDto categoryDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/categories/{categoryDto.Id}", categoryDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/categories/{id}");
            return response.IsSuccessStatusCode;
        }
    }
} 