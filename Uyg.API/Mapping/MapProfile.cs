using AutoMapper;
using Uyg.API.DTOs;
using Uyg.API.Models;

namespace Uyg.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, RegisterDto>().ReverseMap();
            
            CreateMap<News, NewsCreateDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<NewsCreateDto, News>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<News, NewsUpdateDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<NewsUpdateDto, News>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
