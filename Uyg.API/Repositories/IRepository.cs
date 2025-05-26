using System.Linq.Expressions;

namespace Uyg.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
} 