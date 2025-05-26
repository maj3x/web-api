using Microsoft.AspNetCore.Mvc;
using Uyg.API.DTOs;
using Uyg.API.Models;
using Uyg.API.Repositories;
using AutoMapper;

namespace Uyg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<List<CategoryDto>>>> GetAll()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                return Ok(new ResponseDto<List<CategoryDto>>
                {
                    Success = true,
                    Data = categoryDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<List<CategoryDto>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving categories: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> GetById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ResponseDto<CategoryDto>
                    {
                        Success = false,
                        Message = $"Category with ID {id} not found"
                    });
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(new ResponseDto<CategoryDto>
                {
                    Success = true,
                    Data = categoryDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the category: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> Create(CategoryDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveChangesAsync();

                var createdCategoryDto = _mapper.Map<CategoryDto>(category);
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, new ResponseDto<CategoryDto>
                {
                    Success = true,
                    Data = createdCategoryDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = $"An error occurred while creating the category: {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> Update(int id, CategoryDto categoryDto)
        {
            try
            {
                if (id != categoryDto.Id)
                {
                    return BadRequest(new ResponseDto<CategoryDto>
                    {
                        Success = false,
                        Message = "Category ID mismatch"
                    });
                }

                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ResponseDto<CategoryDto>
                    {
                        Success = false,
                        Message = $"Category with ID {id} not found"
                    });
                }

                _mapper.Map(categoryDto, category);
                _categoryRepository.Update(category);
                await _categoryRepository.SaveChangesAsync();

                var updatedCategoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(new ResponseDto<CategoryDto>
                {
                    Success = true,
                    Data = updatedCategoryDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = $"An error occurred while updating the category: {ex.Message}"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<bool>>> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ResponseDto<bool>
                    {
                        Success = false,
                        Message = $"Category with ID {id} not found"
                    });
                }

                _categoryRepository.Delete(category);
                await _categoryRepository.SaveChangesAsync();

                return Ok(new ResponseDto<bool>
                {
                    Success = true,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting the category: {ex.Message}"
                });
            }
        }
    }
} 