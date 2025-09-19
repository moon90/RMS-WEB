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
using RMS.Application.DTOs.PermissionDTOs.InputDTOs;
using RMS.Application.DTOs.PermissionDTOs.OutputDTOs;
using RMS.Application.DTOs.MenuDTOs.InputDTOs;
using RMS.Application.DTOs.RoleDTOs.InputDTOs;
using RMS.Application.DTOs.RoleDTOs.OutputDTOs;
using RMS.Application.DTOs.RoleMenuDTOs.OutputDTOs;
using RMS.Application.DTOs.RolePermissionDTOs.OutputDTOs;
using RMS.Application.DTOs.UserDTOs.InputDTOs;
using RMS.Application.DTOs.UserRoleDTOs.OutputDTOs;
using RMS.Domain.Entities;
using RMS.Application.DTOs.CustomerDTOs.OutputDTOs;
using RMS.Application.DTOs.CustomerDTOs.InputDTOs;
using RMS.Application.DTOs.StaffDTOs.OutputDTOs;
using RMS.Application.DTOs.StaffDTOs.InputDTOs;
using RMS.Application.DTOs.InventoryDTOs.OutputDTOs;
using RMS.Application.DTOs.InventoryDTOs.InputDTOs;
using RMS.Application.DTOs.StockTransactionDTOs.OutputDTOs;
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.DTOs.IngredientDTOs.OutputDTOs;
using RMS.Application.DTOs.IngredientDTOs.InputDTOs;
using RMS.Application.DTOs.ProductIngredientDTOs.OutputDTOs;
using RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs;
using RMS.Application.DTOs.Orders;
using RMS.Application.DTOs.DiningTables;
using RMS.Application.DTOs.AuditLogs;
using RMS.Application.DTOs.Promotions;
using RMS.Domain.Enum;
using RMS.Application.DTOs;
using RMS.Application.DTOs.SplitPaymentDTOs;
using RMS.Application.DTOs.AlertDTOs;

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
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture != null ? $"data:image/png;base64,{Convert.ToBase64String(src.ProfilePicture)}" : null))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore()); // Keeps role assignment clean

            CreateMap<User, UserCreateDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Never map password hash back to DTO
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore());


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

            // Customer mappings
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();

            // Staff mappings
            CreateMap<Staff, StaffDto>().ReverseMap();
            CreateMap<CreateStaffDto, Staff>();
            CreateMap<UpdateStaffDto, Staff>();

            // Inventory mappings
            CreateMap<Inventory, InventoryDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
            CreateMap<CreateInventoryDto, Inventory>();
            CreateMap<UpdateInventoryDto, Inventory>();

            // StockTransaction mappings
            CreateMap<StockTransaction, StockTransactionDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SupplierName))
                .ForMember(dest => dest.IngredientID, opt => opt.MapFrom(src => src.IngredientID));
            CreateMap<CreateStockTransactionDto, StockTransaction>()
                .ForMember(dest => dest.AdjustmentType, opt => opt.MapFrom(src => src.AdjustmentType))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ForMember(dest => dest.IngredientID, opt => opt.MapFrom(src => src.IngredientID));
            CreateMap<UpdateStockTransactionDto, StockTransaction>();

            // Ingredient mappings
            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SupplierName));
            CreateMap<CreateIngredientDto, Ingredient>();
            CreateMap<UpdateIngredientDto, Ingredient>();

            // ProductIngredient mappings
            CreateMap<ProductIngredient, ProductIngredientDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));
            CreateMap<CreateProductIngredientDto, ProductIngredient>();
            CreateMap<UpdateProductIngredientDto, ProductIngredient>();

            // Order mappings
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()); // OrderDetails are handled separately
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()); // OrderDetails handled separately in handler
            

            // OrderDetail mappings
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product != null ? new ProductDto { Id = src.Product.Id, ProductName = src.Product.ProductName } : null))
                .ReverseMap();
            CreateMap<CreateOrderDetailDto, OrderDetail>();
            CreateMap<UpdateOrderDetailDto, OrderDetail>();

            // DiningTable mappings
            CreateMap<DiningTable, DiningTableDto>().ReverseMap();
            CreateMap<CreateDiningTableDto, DiningTable>();
            CreateMap<UpdateDiningTableDto, DiningTable>();
            CreateMap<UpdateDiningTableStatusDto, DiningTable>()
                .ForMember(dest => dest.TableName, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Ignore BaseEntity.Status
                .ForMember(dest => dest.DiningTableStatus, opt => opt.MapFrom(src => src.DiningTableStatus));

            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SupplierName))
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.ManufacturerName))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.ProductImage != null ? $"data:image/png;base64,{Convert.ToBase64String(src.ProductImage)}" : null))
                .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src => src.ThumbnailImage != null ? $"data:image/png;base64,{Convert.ToBase64String(src.ThumbnailImage)}" : null));
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.ProductImage != null ? ConvertBase64ToBytes(src.ProductImage) : null))
                .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src =>
                    src.ThumbnailImage != null ? ConvertBase64ToBytes(src.ThumbnailImage) : null));
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.ProductImage != null ? ConvertBase64ToBytes(src.ProductImage) : null))
                .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src =>
                    src.ThumbnailImage != null ? ConvertBase64ToBytes(src.ThumbnailImage) : null));

            // AuditLog mappings
            CreateMap<AuditLog, AuditLogDto>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.PerformedAt));

            // Promotion mappings
            CreateMap<Promotion, PromotionDto>().ReverseMap();
            CreateMap<CreatePromotionDto, Promotion>();
            CreateMap<UpdatePromotionDto, Promotion>();

            // Purchase mappings
            CreateMap<Purchase, PurchaseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<CreatePurchaseDto, Purchase>();
            CreateMap<PurchaseDetailDto, PurchaseDetail>().ReverseMap();

            // Sale mappings
            CreateMap<Sale, SaleDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<CreateSaleDto, Sale>();
            CreateMap<SaleDetailDto, SaleDetail>().ReverseMap();

            // UnitConversion mappings
            CreateMap<UnitConversion, UnitConversionDto>()
                .ForMember(dest => dest.FromUnitName, opt => opt.MapFrom(src => src.FromUnit.Name))
                .ForMember(dest => dest.ToUnitName, opt => opt.MapFrom(src => src.ToUnit.Name));
            CreateMap<CreateUnitConversionDto, UnitConversion>();

            // SplitPayment mappings
            CreateMap<SplitPayment, SplitPaymentDto>().ReverseMap();
            CreateMap<CreateSplitPaymentDto, SplitPayment>();

            // Alert mappings
            CreateMap<Alert, AlertDto>().ReverseMap();
            CreateMap<CreateAlertDto, Alert>();
        }
    private byte[]? ConvertBase64ToBytes(string? base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }

            // Remove data URI prefix if present (e.g., "data:image/png;base64,")
            if (base64String.Contains(","))
            {
                base64String = base64String.Substring(base64String.IndexOf(",") + 1);
            }

            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (FormatException)
            {
                // Log or handle invalid Base64 string
                return null;
            }
        }
    }
}
