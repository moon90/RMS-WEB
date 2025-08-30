using AutoMapper;
using RMS.Application.DTOs.ManufacturerDTOs.InputDTOs;
using RMS.Application.DTOs.ManufacturerDTOs.OutputDTOs;
using RMS.Application.DTOs.MenuDTOs.OutputDTOs;
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Application.DTOs.ProductDTOs.OutputDTOs;
using RMS.Application.DTOs.SupplierDTOs.InputDTOs;
using RMS.Application.DTOs.SupplierDTOs.OutputDTOs;
using RMS.Application.DTOs.UnitDTOs.InputDTOs;
using RMS.Application.DTOs.UnitDTOs.OutputDTOs;
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
            CreateMap<Menu, MenuDto>()
                .ForMember(dest => dest.MenuID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ControllerName, opt => opt.MapFrom(src => src.ControllerName))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.ActionName))
                .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.ModuleName))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuID));
            CreateMap<MenuCreateDto, Menu>().ReverseMap();
            CreateMap<MenuUpdateDto, Menu>().ReverseMap();

            // UserRole mappings
            CreateMap<UserRole, UserRoleDto>().ReverseMap();

            // RolePermission mappings
            CreateMap<RolePermission, RolePermissionDto>().ReverseMap();

            // RoleMenu mappings
            CreateMap<RoleMenu, RoleMenuDto>()
                .ForMember(dest => dest.RoleMenuID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.MenuID, opt => opt.MapFrom(src => src.MenuID))
                .ForMember(dest => dest.CanView, opt => opt.MapFrom(src => src.CanView))
                .ForMember(dest => dest.CanAdd, opt => opt.MapFrom(src => src.CanAdd))
                .ForMember(dest => dest.CanEdit, opt => opt.MapFrom(src => src.CanEdit))
                .ForMember(dest => dest.CanDelete, opt => opt.MapFrom(src => src.CanDelete))
                .ForMember(dest => dest.menuName, opt => opt.MapFrom(src => src.Menu.MenuName))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.Menu.DisplayOrder)) // Added
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleMenuID))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.MenuID, opt => opt.MapFrom(src => src.MenuID));

            // Menu to UserMenuPermissionDto mapping
            CreateMap<Menu, UserMenuPermissionDto>()
                .ForMember(dest => dest.MenuID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.MenuName))
                .ForMember(dest => dest.ParentID, opt => opt.MapFrom(src => src.ParentID))
                .ForMember(dest => dest.MenuPath, opt => opt.MapFrom(src => src.MenuPath))
                .ForMember(dest => dest.MenuIcon, opt => opt.MapFrom(src => src.MenuIcon))
                .ForMember(dest => dest.ControllerName, opt => opt.MapFrom(src => src.ControllerName))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.ActionName))
                .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.ModuleName))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
                .ForMember(dest => dest.CanView, opt => opt.Ignore())
                .ForMember(dest => dest.CanAdd, opt => opt.Ignore())
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore())
                .ForMember(dest => dest.CanDelete, opt => opt.Ignore());

            // Entity -> DTO
            CreateMap<Permission, PermissionDto>().ReverseMap();

            // CreateDto -> Entity
            CreateMap<PermissionCreateDto, Permission>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap(); // ID is generated

            // UpdateDto -> Entity
            CreateMap<PermissionUpdateDto, Permission>().ReverseMap();

            // Unit mappings
            CreateMap<Unit, UnitDto>().ReverseMap();
            CreateMap<CreateUnitDto, Unit>();
            CreateMap<UpdateUnitDto, Unit>();

            // Supplier mappings
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();

            // Manufacturer mappings
            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
            CreateMap<CreateManufacturerDto, Manufacturer>();
            CreateMap<UpdateManufacturerDto, Manufacturer>();

            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SupplierName))
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.ManufacturerName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl));
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl));
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl));
        }
    }
}
