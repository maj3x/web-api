using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using uyg.UI.Models;
using uyg.UI.Services;

namespace uyg.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _configuration;

        public HomeController(
            INewsService newsService, 
            ICategoryService categoryService,
            IConfiguration configuration)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                LatestNews = await _newsService.GetAllAsync(),
                Categories = (await _categoryService.GetAllAsync()).Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Slug = string.Empty,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    NewsCount = 0
                })
            };
            return View(viewModel);
        }

        [Route("Categories")]
        public async Task<IActionResult> Categories()
        {
            var categories = (await _categoryService.GetAllAsync()).Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Slug = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                NewsCount = 0
            });
            return View(categories);
        }

        [Route("Products/{id}")]
        [Route("Products")]
        public async Task<IActionResult> Products(int id = 0)
        {
            var viewModel = new HomeViewModel
            {
                LatestNews = id > 0 
                    ? await _newsService.GetNewsByCategoryAsync(id)
                    : await _newsService.GetAllAsync(),
                Categories = (await _categoryService.GetAllAsync()).Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Slug = string.Empty,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    NewsCount = 0
                })
            };
            return View(viewModel);
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class HomeViewModel
    {
        public IEnumerable<NewsDto> LatestNews { get; set; } = new List<NewsDto>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
