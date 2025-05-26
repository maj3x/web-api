using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uyg.API.DTOs;
using Uyg.API.Models;
using Uyg.API.Repositories;
using AutoMapper;

namespace Uyg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public NewsController(INewsRepository newsRepository, IWebHostEnvironment environment, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _environment = environment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var news = await _newsRepository.GetAllAsync();
                var newsDtos = _mapper.Map<List<NewsDto>>(news);
                return Ok(new ResponseDto<List<NewsDto>>
                {
                    Success = true,
                    Data = newsDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<List<NewsDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] int count = 10)
        {
            try
            {
                var news = await _newsRepository.GetLatestNewsAsync(count);
                var newsDtos = _mapper.Map<List<NewsDto>>(news);
                return Ok(new ResponseDto<List<NewsDto>>
                {
                    Success = true,
                    Data = newsDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<List<NewsDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving latest news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var news = await _newsRepository.GetNewsByCategoryAsync(categoryId, page, pageSize);
                var newsDtos = _mapper.Map<List<NewsDto>>(news);
                return Ok(new ResponseDto<List<NewsDto>>
                {
                    Success = true,
                    Data = newsDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<List<NewsDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving news by category",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            try
            {
                var news = await _newsRepository.SearchNewsAsync(searchTerm);
                var newsDtos = _mapper.Map<List<NewsDto>>(news);
                return Ok(new ResponseDto<List<NewsDto>>
                {
                    Success = true,
                    Data = newsDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<List<NewsDto>>
                {
                    Success = false,
                    Message = "An error occurred while searching news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var news = await _newsRepository.GetByIdAsync(id);
                if (news == null)
                    return NotFound(new ResponseDto<NewsDto>
                    {
                        Success = false,
                        Message = "News not found"
                    });

                await _newsRepository.IncrementViewCountAsync(id);
                var newsDto = _mapper.Map<NewsDto>(news);
                return Ok(new ResponseDto<NewsDto>
                {
                    Success = true,
                    Data = newsDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<NewsDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NewsCreateDto newsDto)
        {
            try
            {
                var news = new News
                {
                    Title = newsDto.Title,
                    Content = newsDto.Content,
                    Summary = newsDto.Summary,
                    CategoryId = newsDto.CategoryId,
                    AuthorId = User.FindFirst("sub")?.Value,
                    IsPublished = newsDto.IsPublished,
                    PublishedAt = newsDto.IsPublished ? DateTime.UtcNow : null,
                    TagList = newsDto.TagList
                };

                if (newsDto.Image != null)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(newsDto.Image.FileName)}";
                    var filePath = Path.Combine(_environment.WebRootPath, "Files", "NewsImages", fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newsDto.Image.CopyToAsync(stream);
                    }

                    news.ImageUrl = $"/Files/NewsImages/{fileName}";
                }

                await _newsRepository.AddAsync(news);
                var createdNewsDto = _mapper.Map<NewsDto>(news);
                return CreatedAtAction(nameof(GetById), new { id = news.Id }, new ResponseDto<NewsDto>
                {
                    Success = true,
                    Data = createdNewsDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<NewsDto>
                {
                    Success = false,
                    Message = "An error occurred while creating news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] NewsUpdateDto newsDto)
        {
            try
            {
                var news = await _newsRepository.GetByIdAsync(id);
                if (news == null)
                    return NotFound(new ResponseDto<NewsDto>
                    {
                        Success = false,
                        Message = "News not found"
                    });

                news.Title = newsDto.Title;
                news.Content = newsDto.Content;
                news.Summary = newsDto.Summary;
                news.CategoryId = newsDto.CategoryId;
                news.TagList = newsDto.TagList;

                if (newsDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(news.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(_environment.WebRootPath, "Files", "NewsImages", 
                            Path.GetFileName(news.ImageUrl));
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(newsDto.Image.FileName)}";
                    var filePath = Path.Combine(_environment.WebRootPath, "Files", "NewsImages", fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newsDto.Image.CopyToAsync(stream);
                    }

                    news.ImageUrl = $"/Files/NewsImages/{fileName}";
                }

                if (newsDto.IsPublished && !news.IsPublished)
                {
                    news.IsPublished = true;
                    news.PublishedAt = DateTime.UtcNow;
                }

                await _newsRepository.Update(news);
                var updatedNewsDto = _mapper.Map<NewsDto>(news);
                return Ok(new ResponseDto<NewsDto>
                {
                    Success = true,
                    Data = updatedNewsDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<NewsDto>
                {
                    Success = false,
                    Message = "An error occurred while updating news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var news = await _newsRepository.GetByIdAsync(id);
                if (news == null)
                    return NotFound(new ResponseDto<NewsDto>
                    {
                        Success = false,
                        Message = "News not found"
                    });

                if (!string.IsNullOrEmpty(news.ImageUrl))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "Files", "NewsImages", 
                        Path.GetFileName(news.ImageUrl));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                await _newsRepository.Delete(news);
                return Ok(new ResponseDto<NewsDto>
                {
                    Success = true,
                    Message = "News deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<NewsDto>
                {
                    Success = false,
                    Message = "An error occurred while deleting news",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
} 