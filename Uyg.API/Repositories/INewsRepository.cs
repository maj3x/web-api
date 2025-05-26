using Uyg.API.Models;

namespace Uyg.API.Repositories
{
    public interface INewsRepository : IRepository<News>
    {
        Task<List<News>> GetLatestNewsAsync(int count = 10);
        Task<List<News>> GetNewsByCategoryAsync(int categoryId, int page = 1, int pageSize = 10);
        Task<List<News>> SearchNewsAsync(string searchTerm);
        Task IncrementViewCountAsync(int newsId);
        Task<Tag> GetOrCreateTagAsync(string tagName);
        Task<IEnumerable<News>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<News>> GetByAuthorAsync(string authorId);
        Task<IEnumerable<News>> GetByTagAsync(string tagName);
    }
} 