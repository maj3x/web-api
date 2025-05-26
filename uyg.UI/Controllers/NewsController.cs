using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uyg.UI.Models;
using uyg.UI.Services;

namespace uyg.UI.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetAllAsync();
            return View(news);
        }

        public async Task<IActionResult> Details(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateDto newsDto)
        {
            if (ModelState.IsValid)
            {
                var apiDto = new Uyg.API.DTOs.NewsCreateDto
                {
                    Title = newsDto.Title,
                    Content = newsDto.Content,
                    CategoryId = newsDto.CategoryId,
                    ImageUrl = newsDto.ImageUrl,
                    IsPublished = newsDto.IsPublished
                };

                var result = await _newsService.CreateAsync(apiDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(newsDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            var updateDto = new NewsUpdateDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                CategoryId = news.CategoryId,
                ImageUrl = news.ImageUrl,
                IsPublished = news.IsPublished
            };

            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsUpdateDto newsDto)
        {
            if (id != newsDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var apiDto = new Uyg.API.DTOs.NewsUpdateDto
                {
                    Id = newsDto.Id,
                    Title = newsDto.Title,
                    Content = newsDto.Content,
                    CategoryId = newsDto.CategoryId,
                    ImageUrl = newsDto.ImageUrl,
                    IsPublished = newsDto.IsPublished
                };

                var result = await _newsService.UpdateAsync(apiDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(newsDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _newsService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        public async Task<IActionResult> Category(int id, int page = 1)
        {
            var news = await _newsService.GetNewsByCategoryAsync(id, page);
            return View("Index", news);
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction(nameof(Index));
            }

            var news = await _newsService.SearchNewsAsync(query);
            return View("Index", news);
        }
    }
} 