using AutoMapper;
using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using RMS.Application.DTOs.CategoryDTOs.OutputDTOs;
using RMS.Domain.Entities;

namespace RMS.Application.AutoMappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}
