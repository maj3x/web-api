using System.Collections.Generic;
using System.Threading.Tasks;
using uyg.UI.Models;

namespace uyg.UI.Services
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetAllAsync();
        Task<NewsDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Uyg.API.DTOs.NewsCreateDto newsDto);
        Task<bool> UpdateAsync(Uyg.API.DTOs.NewsUpdateDto newsDto);
        Task<bool> DeleteAsync(int id);
        Task<List<NewsDto>> GetNewsByCategoryAsync(int categoryId, int page = 1);
        Task<List<NewsDto>> SearchNewsAsync(string query);
    }
} 