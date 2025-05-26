using Uyg.API.DTOs;

namespace uyg.UI.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CategoryDto categoryDto);
        Task<bool> UpdateAsync(CategoryDto categoryDto);
        Task<bool> DeleteAsync(int id);
    }
} 