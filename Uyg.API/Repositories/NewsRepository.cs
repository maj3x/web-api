using Microsoft.EntityFrameworkCore;
using Uyg.API.Data;
using Uyg.API.Models;
using System.Linq.Expressions;

namespace Uyg.API.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<News>> GetLatestNewsAsync(int count = 10)
        {
            return await _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .Take(count)
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.TagList)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsByCategoryAsync(int categoryId, int page = 1, int pageSize = 10)
        {
            return await _context.News
                .Where(n => n.CategoryId == categoryId && n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.TagList)
                .ToListAsync();
        }

        public async Task<List<News>> SearchNewsAsync(string searchTerm)
        {
            return await _context.News
                .Where(n => n.IsPublished && 
                    (n.Title.Contains(searchTerm) || 
                     n.Content.Contains(searchTerm) || 
                     n.TagList.Any(t => t.Name.Contains(searchTerm))))
                .OrderByDescending(n => n.PublishedAt)
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.TagList)
                .ToListAsync();
        }

        public async Task IncrementViewCountAsync(int newsId)
        {
            var news = await _context.News.FindAsync(newsId);
            if (news != null)
            {
                news.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }

        public override async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.TagList)
                .ToListAsync();
        }

        public override async Task<News?> GetByIdAsync(int id)
        {
            return await _context.News
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.TagList)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    Slug = tagName.ToLower().Replace(" ", "-")
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return tag;
        }

        public override async Task<News> AddAsync(News news)
        {
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public override async Task<News> UpdateAsync(News news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<IEnumerable<News>> GetByCategoryAsync(int categoryId)
        {
            return await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Include(n => n.TagList)
                .Where(n => n.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<News>> GetByAuthorAsync(string authorId)
        {
            return await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Include(n => n.TagList)
                .Where(n => n.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<News>> GetByTagAsync(string tagName)
        {
            return await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Include(n => n.TagList)
                .Where(n => n.TagList.Any(t => t.Name == tagName))
                .ToListAsync();
        }

        public override async Task<News> Update(News news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public override async Task Delete(News news)
        {
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }

        public override async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public override async Task<News?> FindAsync(Expression<Func<News, bool>> predicate)
        {
            return await _context.News
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Include(n => n.TagList)
                .FirstOrDefaultAsync(predicate);
        }
    }
} 