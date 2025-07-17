using AutoMapper;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.OutputDTOs;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;
using RMS.Domain.Dtos.PermissionDTOs.OutputDTOs;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.OutputDTOs;
using RMS.Domain.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Domain.DTOs.RolePermissionDTOs.OutputDTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Domain.DTOs.UserRoleDTOs.OutputDTOs;
using RMS.Domain.Entities;

namespace RMS.Application.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                    src.UserRoles.Select(ur => ur.Role != null ? ur.Role.RoleName : string.Empty).ToList()))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl)) // New line
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl)) // New line
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore()); // Keeps role assignment clean

            CreateMap<User, UserCreateDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Never map password hash back to DTO
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl)) // New line
                .ReverseMap()
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl)); // New line

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl)) // New line
                .ReverseMap()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl)); // New line


            // Role mappings
            CreateMap<Role, RoleDto>().ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<RoleCreateDto, Role>().ReverseMap();
            CreateMap<RoleUpdateDto, Role>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleID)).ReverseMap();

            // Menu mappings
            CreateMap<Menu, MenuDto>().ReverseMap();
            CreateMap<MenuCreateDto, Menu>().ReverseMap();
            CreateMap<MenuUpdateDto, Menu>().ReverseMap();

            // UserRole mappings
            CreateMap<UserRole, UserRoleDto>().ReverseMap();

            // RolePermission mappings
            CreateMap<RolePermission, RolePermissionDto>().ReverseMap();

            // RoleMenu mappings
            CreateMap<RoleMenu, RoleMenuDto>().ReverseMap();

            // Entity -> DTO
            CreateMap<Permission, PermissionDto>().ReverseMap();

            // CreateDto -> Entity
            CreateMap<PermissionCreateDto, Permission>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap(); // ID is generated

            // UpdateDto -> Entity
            CreateMap<PermissionUpdateDto, Permission>().ReverseMap();
        }
    }
}
