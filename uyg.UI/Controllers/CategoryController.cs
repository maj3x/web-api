using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uyg.UI.Services;
using uyg.UI.Models;

namespace uyg.UI.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
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

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var uiCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Slug = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                NewsCount = 0
            };

            return View(uiCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var apiDto = new Uyg.API.DTOs.CategoryDto
                {
                    Name = category.Name,
                    Description = category.Description
                };

                var result = await _categoryService.CreateAsync(apiDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var uiCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Slug = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                NewsCount = 0
            };

            return View(uiCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var apiDto = new Uyg.API.DTOs.CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                var result = await _categoryService.UpdateAsync(apiDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var uiCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Slug = string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                NewsCount = 0
            };

            return View(uiCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
} 