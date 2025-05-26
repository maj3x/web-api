using AutoMapper;
using Uyg.API.DTOs;
using Uyg.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace Uyg.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<News, NewsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForMember(dest => dest.TagList, opt => opt.MapFrom(src => src.TagList));

            CreateMap<NewsCreateDto, News>();
            CreateMap<NewsUpdateDto, News>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, LoginResponseDto>();
        }
    }
} 