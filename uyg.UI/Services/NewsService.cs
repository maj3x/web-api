using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using uyg.UI.Models;
using Microsoft.Extensions.Configuration;

namespace uyg.UI.Services
{
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public NewsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new ArgumentNullException("ApiSettings:BaseUrl");
        }

        public async Task<List<NewsDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<NewsDto>>>($"{_baseUrl}/News");
            return response?.Data ?? new List<NewsDto>();
        }

        public async Task<NewsDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<NewsDto>>($"{_baseUrl}/News/{id}");
            return response?.Data;
        }

        public async Task<bool> CreateAsync(Uyg.API.DTOs.NewsCreateDto newsDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/News", newsDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Uyg.API.DTOs.NewsUpdateDto newsDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/News/{newsDto.Id}", newsDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/News/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<NewsDto>> GetNewsByCategoryAsync(int categoryId, int page = 1)
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<NewsDto>>>($"{_baseUrl}/News/category/{categoryId}?page={page}");
            return response?.Data ?? new List<NewsDto>();
        }

        public async Task<List<NewsDto>> SearchNewsAsync(string query)
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<NewsDto>>>($"{_baseUrl}/News/search?query={query}");
            return response?.Data ?? new List<NewsDto>();
        }
    }
} 