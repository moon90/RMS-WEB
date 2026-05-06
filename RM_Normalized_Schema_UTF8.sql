USE [master]
GO
/****** Object:  Database [RMSDB]    Script Date: 08/01/2026 01:58:44 ******/
CREATE DATABASE [RMSDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RMSDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RMSDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RMSDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RMSDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RMSDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RMSDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RMSDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RMSDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RMSDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RMSDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RMSDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RMSDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RMSDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RMSDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RMSDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RMSDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RMSDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RMSDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RMSDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RMSDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RMSDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RMSDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RMSDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RMSDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RMSDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RMSDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RMSDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [RMSDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RMSDB] SET RECOVERY FULL 
GO
ALTER DATABASE [RMSDB] SET  MULTI_USER 
GO
ALTER DATABASE [RMSDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RMSDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RMSDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RMSDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RMSDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RMSDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RMSDB] SET QUERY_STORE = OFF
GO
USE [RMSDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Alerts]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alerts](
	[AlertId] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](250) NOT NULL,
	[Type] [int] NOT NULL,
	[IsAcknowledged] [bit] NOT NULL,
	[AlertDate] [datetime2](7) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Alerts] PRIMARY KEY CLUSTERED 
(
	[AlertId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](max) NOT NULL,
	[EntityType] [nvarchar](max) NOT NULL,
	[EntityId] [nvarchar](max) NOT NULL,
	[PerformedBy] [nvarchar](max) NOT NULL,
	[PerformedAt] [datetime2](7) NOT NULL,
	[Details] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](100) NOT NULL,
	[CustomerPhone] [nvarchar](20) NULL,
	[CustomerEmail] [nvarchar](100) NULL,
	[Address] [nvarchar](250) NULL,
	[DriverName] [nvarchar](100) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiningTables]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiningTables](
	[TableID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](100) NOT NULL,
	[DiningTableStatus] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_DiningTables] PRIMARY KEY CLUSTERED 
(
	[TableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredients]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredients](
	[IngredientID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[QuantityAvailable] [decimal](10, 2) NOT NULL,
	[UnitID] [int] NOT NULL,
	[ReorderLevel] [decimal](10, 2) NOT NULL,
	[ReorderQuantity] [decimal](10, 2) NOT NULL,
	[SupplierID] [int] NULL,
	[ExpireDate] [datetime2](7) NULL,
	[Remarks] [nvarchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Ingredients] PRIMARY KEY CLUSTERED 
(
	[IngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[InventoryID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[InitialStock] [int] NOT NULL,
	[CurrentStock] [int] NOT NULL,
	[MinStockLevel] [int] NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[InventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturers]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ManufacturerName] [nvarchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Manufacturers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](100) NOT NULL,
	[ParentID] [int] NULL,
	[MenuPath] [nvarchar](200) NOT NULL,
	[MenuIcon] [nvarchar](100) NOT NULL,
	[ControllerName] [nvarchar](100) NOT NULL,
	[ActionName] [nvarchar](100) NOT NULL,
	[ModuleName] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DiscountPrice] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PromotionDetailID] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [date] NULL,
	[OrderTime] [varchar](15) NOT NULL,
	[TableName] [varchar](50) NOT NULL,
	[WaiterName] [varchar](50) NOT NULL,
	[OrderStatus] [varchar](20) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL,
	[DiscountPercentage] [decimal](10, 2) NOT NULL,
	[PromotionID] [int] NULL,
	[Received] [decimal](18, 2) NOT NULL,
	[ChangeAmount] [decimal](18, 2) NOT NULL,
	[DriverID] [int] NULL,
	[CustomerID] [int] NULL,
	[PaymentStatus] [varchar](50) NULL,
	[PaymentMethod] [varchar](50) NULL,
	[AmountPaid] [decimal](18, 2) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [nvarchar](100) NOT NULL,
	[PermissionKey] [nvarchar](100) NOT NULL,
	[ControllerName] [nvarchar](max) NULL,
	[ActionName] [nvarchar](max) NULL,
	[ModuleName] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductIngredients]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductIngredients](
	[ProductIngredientID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[IngredientID] [int] NOT NULL,
	[Quantity] [decimal](10, 2) NOT NULL,
	[UnitID] [int] NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ProductIngredients] PRIMARY KEY CLUSTERED 
(
	[ProductIngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[ProductPrice] [decimal](18, 2) NOT NULL,
	[CostPrice] [decimal](18, 2) NULL,
	[ProductBarcode] [nvarchar](100) NULL,
	[ProductImage] [varbinary](max) NULL,
	[ThumbnailImage] [varbinary](max) NULL,
	[CategoryID] [int] NULL,
	[SupplierID] [int] NULL,
	[ManufacturerID] [int] NULL,
	[ExpireDate] [date] NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotions]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotions](
	[PromotionID] [int] IDENTITY(1,1) NOT NULL,
	[CouponCode] [nvarchar](50) NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL,
	[DiscountPercentage] [decimal](10, 2) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Promotions] PRIMARY KEY CLUSTERED 
(
	[PromotionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseDetails]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseDetails](
	[PurchaseDetailID] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PurchaseDetails] PRIMARY KEY CLUSTERED 
(
	[PurchaseDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchases]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchases](
	[PurchaseID] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseDate] [datetime2](7) NOT NULL,
	[SupplierID] [int] NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Purchases] PRIMARY KEY CLUSTERED 
(
	[PurchaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMenus]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[MenuID] [int] NOT NULL,
	[CanView] [bit] NOT NULL,
	[CanAdd] [bit] NOT NULL,
	[CanEdit] [bit] NOT NULL,
	[CanDelete] [bit] NOT NULL,
	[AssignedAt] [datetime2](7) NOT NULL,
	[AssignedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_RoleMenus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[AssignedAt] [datetime2](7) NOT NULL,
	[AssignedBy] [nvarchar](max) NULL,
	[SortingOrder] [int] NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SaleDetails]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleDetails](
	[SaleDetailID] [int] IDENTITY(1,1) NOT NULL,
	[SaleID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SaleDetails] PRIMARY KEY CLUSTERED 
(
	[SaleDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SaleID] [int] IDENTITY(1,1) NOT NULL,
	[SaleDate] [datetime2](7) NOT NULL,
	[CustomerID] [int] NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[FinalAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[SaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SplitPayments]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SplitPayments](
	[SplitPaymentID] [int] IDENTITY(1,1) NOT NULL,
	[SaleID] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SplitPayments] PRIMARY KEY CLUSTERED 
(
	[SplitPaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffID] [int] IDENTITY(1,1) NOT NULL,
	[StaffName] [nvarchar](100) NULL,
	[StaffPhone] [nvarchar](20) NULL,
	[StaffRole] [nvarchar](100) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockTransactions]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockTransactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[SupplierID] [int] NULL,
	[TransactionType] [nvarchar](10) NOT NULL,
	[Quantity] [int] NOT NULL,
	[TransactionDate] [datetime2](7) NOT NULL,
	[ExpireDate] [datetime2](7) NULL,
	[Remarks] [nvarchar](250) NULL,
	[SaleID] [int] NULL,
	[PurchaseID] [int] NULL,
	[TransactionSource] [nvarchar](50) NULL,
	[AdjustmentType] [nvarchar](max) NULL,
	[Reason] [nvarchar](max) NULL,
	[IngredientID] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_StockTransactions] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [nvarchar](100) NOT NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Units]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ShortCode] [nvarchar](10) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[AssignedAt] [datetime2](7) NOT NULL,
	[AssignedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08/01/2026 01:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[PasswordSalt] [nvarchar](max) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[ProfilePicture] [varbinary](max) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpiry] [datetime2](7) NULL,
	[PasswordResetToken] [nvarchar](max) NULL,
	[PasswordResetTokenExpiry] [datetime2](7) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251215015122_InitialEntry', N'8.0.12')
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] ON 

INSERT [dbo].[AuditLogs] ([Id], [Action], [EntityType], [EntityId], [PerformedBy], [PerformedAt], [Details]) VALUES (1, N'CreateCategory', N'Category', N'CategoryId:1001', N'System', CAST(N'2025-12-28T23:26:22.4481162' AS DateTime2), N'Category ''Combo Pack 1'' created.')
INSERT [dbo].[AuditLogs] ([Id], [Action], [EntityType], [EntityId], [PerformedBy], [PerformedAt], [Details]) VALUES (2, N'UpdateCategoryStatus', N'Category', N'CategoryId:1', N'System', CAST(N'2025-12-28T23:48:00.1215037' AS DateTime2), N'Category ''Appetizers'' status updated to Inactive.')
INSERT [dbo].[AuditLogs] ([Id], [Action], [EntityType], [EntityId], [PerformedBy], [PerformedAt], [Details]) VALUES (3, N'UpdateCategoryStatus', N'Category', N'CategoryId:1', N'System', CAST(N'2025-12-28T23:48:06.1662349' AS DateTime2), N'Category ''Appetizers'' status updated to Active.')
INSERT [dbo].[AuditLogs] ([Id], [Action], [EntityType], [EntityId], [PerformedBy], [PerformedAt], [Details]) VALUES (4, N'CreateCategory', N'Category', N'CategoryId:1002', N'System', CAST(N'2025-12-28T23:48:17.4767190' AS DateTime2), N'Category ''Combo Pack 2'' created.')
SET IDENTITY_INSERT [dbo].[AuditLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Appetizers', 1, N'system', CAST(N'2025-12-15T01:51:21.3498755' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Main Courses', 1, N'system', CAST(N'2025-12-15T01:51:21.3498757' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Desserts', 1, N'system', CAST(N'2025-12-15T01:51:21.3498759' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (4, N'Drinks', 1, N'system', CAST(N'2025-12-15T01:51:21.3498760' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1001, N'Combo Pack 1', 1, N'system', CAST(N'2025-12-28T23:26:22.3732487' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1002, N'Combo Pack 2', 1, N'system', CAST(N'2025-12-28T23:48:17.4669461' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [Address], [DriverName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'John Doe', N'123-456-7890', N'john.doe@example.com', N'123 Main St, Anytown', N'John Doe', 1, N'system', CAST(N'2025-12-15T01:51:21.3503461' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Customers] ([CustomerID], [CustomerName], [CustomerPhone], [CustomerEmail], [Address], [DriverName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Jane Smith', N'098-765-4321', N'jane.smith@example.com', N'456 Oak Ave, Somewhere', N'Jane Smith', 1, N'system', CAST(N'2025-12-15T01:51:21.3503463' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[DiningTables] ON 

INSERT [dbo].[DiningTables] ([TableID], [TableName], [DiningTableStatus], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Table 1', N'Available', 1, N'system', CAST(N'2025-12-15T01:51:21.3523598' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[DiningTables] ([TableID], [TableName], [DiningTableStatus], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Table 2', N'Available', 1, N'system', CAST(N'2025-12-15T01:51:21.3523600' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[DiningTables] ([TableID], [TableName], [DiningTableStatus], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Table 3', N'Available', 1, N'system', CAST(N'2025-12-15T01:51:21.3523641' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[DiningTables] ([TableID], [TableName], [DiningTableStatus], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (4, N'Table 4', N'Available', 1, N'system', CAST(N'2025-12-15T01:51:21.3523643' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[DiningTables] ([TableID], [TableName], [DiningTableStatus], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (5, N'Table 5', N'Available', 1, N'system', CAST(N'2025-12-15T01:51:21.3523644' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[DiningTables] OFF
GO
SET IDENTITY_INSERT [dbo].[Ingredients] ON 

INSERT [dbo].[Ingredients] ([IngredientID], [Name], [QuantityAvailable], [UnitID], [ReorderLevel], [ReorderQuantity], [SupplierID], [ExpireDate], [Remarks], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Flour', CAST(50.00 AS Decimal(10, 2)), 1, CAST(10.00 AS Decimal(10, 2)), CAST(20.00 AS Decimal(10, 2)), 1, CAST(N'2026-12-15T01:51:21.3511634' AS DateTime2), N'All-purpose flour', 1, N'system', CAST(N'2025-12-15T01:51:21.3511637' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Ingredients] ([IngredientID], [Name], [QuantityAvailable], [UnitID], [ReorderLevel], [ReorderQuantity], [SupplierID], [ExpireDate], [Remarks], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Sugar', CAST(25.00 AS Decimal(10, 2)), 1, CAST(5.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), 1, CAST(N'2027-12-15T01:51:21.3511640' AS DateTime2), N'Granulated sugar', 1, N'system', CAST(N'2025-12-15T01:51:21.3511641' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Ingredients] OFF
GO
SET IDENTITY_INSERT [dbo].[Inventory] ON 

INSERT [dbo].[Inventory] ([InventoryID], [ProductID], [InitialStock], [CurrentStock], [MinStockLevel], [LastUpdated], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, 100, 100, 10, CAST(N'2025-12-15T01:51:21.3506273' AS DateTime2), 1, N'system', CAST(N'2025-12-15T01:51:21.3506274' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Inventory] ([InventoryID], [ProductID], [InitialStock], [CurrentStock], [MinStockLevel], [LastUpdated], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 2, 50, 0, 5, CAST(N'2025-12-15T01:51:21.3506276' AS DateTime2), 1, N'system', CAST(N'2025-12-15T01:51:21.3506276' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Inventory] OFF
GO
SET IDENTITY_INSERT [dbo].[Manufacturers] ON 

INSERT [dbo].[Manufacturers] ([Id], [ManufacturerName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Manufacturer X', 1, N'system', CAST(N'2025-12-15T01:51:21.3499841' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Manufacturers] ([Id], [ManufacturerName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Manufacturer Y', 1, N'system', CAST(N'2025-12-15T01:51:21.3499842' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Manufacturers] ([Id], [ManufacturerName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Manufacturer Z', 0, N'system', CAST(N'2025-12-15T01:51:21.3499843' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Manufacturers] OFF
GO
SET IDENTITY_INSERT [dbo].[Menus] ON 

INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Dashboard', NULL, N'/dashboard', N'FaHome', N'Dashboard', N'Index', N'Dashboard', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495317' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'User Management', NULL, N'/users', N'FaUsers', N'Users', N'Index', N'UserManagement', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495320' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'User List', 2, N'/users/list', N'FaListUl', N'Users', N'List', N'UserManagement', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495324' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (4, N'User Add', 2, N'/users/add', N'FaPlus', N'Users', N'Add', N'UserManagement', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495327' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (5, N'Roles', 2, N'/roles', N'FaUserShield', N'Roles', N'Index', N'UserManagement', 3, 1, N'system', CAST(N'2025-12-15T01:51:21.3495329' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (6, N'Role List', 5, N'/roles/list', N'FaListUl', N'Roles', N'List', N'UserManagement', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495332' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (7, N'Role Add', 5, N'/roles/add', N'FaPlus', N'Roles', N'Add', N'UserManagement', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495335' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (8, N'User Access Role', 5, N'/roles/access_role', N'FaUserShield', N'Roles', N'AccessRole', N'UserManagement', 3, 1, N'system', CAST(N'2025-12-15T01:51:21.3495338' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (9, N'Role Permissions', 5, N'/roles/role_permissions', N'FaCog', N'Permissions', N'Setup', N'UserManagement', 4, 1, N'system', CAST(N'2025-12-15T01:51:21.3495340' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (10, N'Menu Assignments', 5, N'/roles/menu_assignments', N'FaClipboardList', N'Menus', N'Setup', N'UserManagement', 5, 1, N'system', CAST(N'2025-12-15T01:51:21.3495343' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (11, N'Permissions', 2, N'/permissions', N'FaCog', N'Permissions', N'Index', N'UserManagement', 4, 1, N'system', CAST(N'2025-12-15T01:51:21.3495346' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (12, N'Permission List', 11, N'/permissions/list', N'FaListUl', N'Permissions', N'List', N'UserManagement', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495349' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (13, N'Permission Add', 11, N'/permissions/add', N'FaPlus', N'Permissions', N'Add', N'UserManagement', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495351' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (14, N'Menu Management', NULL, N'/menus', N'FaClipboardList', N'Menus', N'Index', N'MenuManagement', 3, 1, N'system', CAST(N'2025-12-15T01:51:21.3495354' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (15, N'Menu List', 14, N'/menus/list', N'FaListUl', N'Menus', N'List', N'MenuManagement', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495356' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (16, N'Menu Add', 14, N'/menus/add', N'FaPlus', N'Menus', N'Add', N'MenuManagement', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495359' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (17, N'Audit Logs', NULL, N'/audit-logs', N'FaFileAlt', N'AuditLogs', N'Index', N'System', 4, 1, N'system', CAST(N'2025-12-15T01:51:21.3495361' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (18, N'Inventory', NULL, N'/inventory', N'FaBoxOpen', N'Inventory', N'Index', N'Inventory', 5, 1, N'system', CAST(N'2025-12-15T01:51:21.3495363' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (19, N'Kitchen', NULL, N'/kitchen', N'FaUtensils', N'Kitchen', N'Index', N'Kitchen', 6, 1, N'system', CAST(N'2025-12-15T01:51:21.3495365' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (20, N'Category Management', NULL, N'/categories', N'FaClipboardList', N'Categories', N'Index', N'Product Management', 4, 1, N'system', CAST(N'2025-12-15T01:51:21.3495368' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (21, N'Category List', 20, N'/categories/list', N'FaListUl', N'Categories', N'List', N'Product Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495370' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (22, N'Category Add', 20, N'/categories/add', N'FaPlus', N'Categories', N'Add', N'Product Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495373' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (23, N'Unit Management', NULL, N'/units', N'FaRulerCombined', N'Units', N'Index', N'Product Management', 7, 1, N'system', CAST(N'2025-12-15T01:51:21.3495375' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (24, N'Unit List', 23, N'/units/list', N'FaListUl', N'Units', N'List', N'Product Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495378' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (25, N'Unit Add', 23, N'/units/add', N'FaPlus', N'Units', N'Add', N'Product Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495380' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (26, N'Supplier Management', NULL, N'/suppliers', N'FaTruck', N'Suppliers', N'Index', N'Product Management', 8, 1, N'system', CAST(N'2025-12-15T01:51:21.3495382' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (27, N'Supplier List', 26, N'/suppliers/list', N'FaListUl', N'Suppliers', N'List', N'Product Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495385' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (28, N'Supplier Add', 26, N'/suppliers/add', N'FaPlus', N'Suppliers', N'Add', N'Product Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495387' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (29, N'Manufacturer Management', NULL, N'/manufacturers', N'FaIndustry', N'Manufacturers', N'Index', N'Product Management', 9, 1, N'system', CAST(N'2025-12-15T01:51:21.3495389' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (30, N'Manufacturer List', 29, N'/manufacturers/list', N'FaListUl', N'Manufacturers', N'List', N'Product Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495392' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (31, N'Manufacturer Add', 29, N'/manufacturers/add', N'FaPlus', N'Manufacturers', N'Add', N'Product Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495394' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (32, N'Product Management', NULL, N'/products', N'FaBoxOpen', N'Products', N'Index', N'Product Management', 10, 1, N'system', CAST(N'2025-12-15T01:51:21.3495397' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (33, N'Product List', 32, N'/products/list', N'FaListUl', N'Products', N'List', N'Product Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495399' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (34, N'Product Add', 32, N'/products/add', N'FaPlus', N'Products', N'Add', N'Product Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495402' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (35, N'Customer Management', NULL, N'/customers', N'FaUserFriends', N'Customers', N'Index', N'Customer Management', 11, 1, N'system', CAST(N'2025-12-15T01:51:21.3495452' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (36, N'Customer List', 35, N'/customers/list', N'FaListUl', N'Customers', N'List', N'Customer Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495455' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (37, N'Customer Add', 35, N'/customers/add', N'FaPlus', N'Customers', N'Add', N'Customer Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495458' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (38, N'Staff Management', NULL, N'/staff', N'FaUserTie', N'Staff', N'Index', N'Staff Management', 12, 1, N'system', CAST(N'2025-12-15T01:51:21.3495461' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (39, N'Staff List', 38, N'/staff/list', N'FaListUl', N'Staff', N'List', N'Staff Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495463' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (40, N'Staff Add', 38, N'/staff/add', N'FaPlus', N'Staff', N'Add', N'Staff Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495466' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (41, N'Inventory Management', NULL, N'/inventory', N'FaBoxOpen', N'Inventory', N'Index', N'Inventory Management', 13, 1, N'system', CAST(N'2025-12-15T01:51:21.3495468' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (42, N'Inventory List', 41, N'/inventory/list', N'FaListUl', N'Inventory', N'List', N'Inventory Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495471' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (43, N'Alerts', 41, N'/alerts', N'FaBell', N'Alerts', N'Index', N'Inventory Management', 3, 1, N'system', CAST(N'2025-12-15T01:51:21.3495473' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (44, N'Stock Transaction Management', NULL, N'/stock-transactions', N'FaExchangeAlt', N'StockTransactions', N'Index', N'Stock Management', 14, 1, N'system', CAST(N'2025-12-15T01:51:21.3495476' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (45, N'Stock Transaction List', 44, N'/stock-transactions/list', N'FaListUl', N'StockTransactions', N'List', N'Stock Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495480' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (46, N'Stock Transaction Add', 44, N'/stock-transactions/add', N'FaPlus', N'StockTransactions', N'Add', N'Stock Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495482' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (47, N'Ingredient Management', NULL, N'/ingredients', N'FaLeaf', N'Ingredients', N'Index', N'Ingredient Management', 15, 1, N'system', CAST(N'2025-12-15T01:51:21.3495485' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (48, N'Ingredient List', 47, N'/ingredients/list', N'FaListUl', N'Ingredients', N'List', N'Ingredient Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495487' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (49, N'Ingredient Add', 47, N'/ingredients/add', N'FaPlus', N'Ingredients', N'Add', N'Ingredient Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495490' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (50, N'Product Ingredient Management', NULL, N'/product-ingredients', N'FaBlender', N'ProductIngredients', N'Index', N'Product Ingredient Management', 16, 1, N'system', CAST(N'2025-12-15T01:51:21.3495492' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (51, N'Product Ingredient List', 50, N'/product-ingredients/list', N'FaListUl', N'ProductIngredients', N'List', N'Product Ingredient Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495495' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (52, N'Product Ingredient Add', 50, N'/product-ingredients/add', N'FaPlus', N'ProductIngredients', N'Add', N'Product Ingredient Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495497' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (53, N'Order Management', NULL, N'/orders', N'FaShoppingCart', N'Orders', N'Index', N'Order Management', 17, 1, N'system', CAST(N'2025-12-15T01:51:21.3495499' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (54, N'Order List', 53, N'/orders/list', N'FaListUl', N'Orders', N'List', N'Order Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495502' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (55, N'Order Add', 53, N'/orders/add', N'FaPlus', N'Orders', N'Add', N'Order Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495504' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (56, N'Dining Table Management', NULL, N'/dining-tables', N'FaTable', N'DiningTables', N'Index', N'Dining Table Management', 18, 1, N'system', CAST(N'2025-12-15T01:51:21.3495514' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (57, N'Dining Table List', 56, N'/dining-tables/list', N'FaListUl', N'DiningTables', N'List', N'Dining Table Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495516' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (58, N'Dining Table Add', 56, N'/dining-tables/add', N'FaPlus', N'DiningTables', N'Add', N'Dining Table Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495519' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (59, N'Promotions Management', NULL, N'/promotions', N'FaTags', N'Promotions', N'Index', N'Promotions Management', 19, 1, N'system', CAST(N'2025-12-15T01:51:21.3495521' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (60, N'Promotion List', 59, N'/promotions/list', N'FaListUl', N'Promotions', N'List', N'Promotions Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495523' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (61, N'Promotion Add', 59, N'/promotions/add', N'FaPlus', N'Promotions', N'Add', N'Promotions Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495526' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (62, N'Purchase Management', NULL, N'/purchases', N'FaShoppingCart', N'Purchases', N'Index', N'Purchase Management', 20, 1, N'system', CAST(N'2025-12-15T01:51:21.3495528' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (63, N'Purchase List', 62, N'/purchases/list', N'FaListUl', N'Purchases', N'List', N'Purchase Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495530' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (64, N'Purchase Add', 62, N'/purchases/create', N'FaPlus', N'Purchases', N'Create', N'Purchase Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495533' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (65, N'Purchase Edit', 62, N'/purchases/edit', N'FaEdit', N'Purchases', N'Edit', N'Purchase Management', 3, 1, N'system', CAST(N'2025-12-15T01:51:21.3495535' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (66, N'Purchase Detail', 62, N'/purchases/detail', N'FaInfoCircle', N'Purchases', N'Detail', N'Purchase Management', 4, 1, N'system', CAST(N'2025-12-15T01:51:21.3495538' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (67, N'Sales Management', NULL, N'/sales', N'FaChartLine', N'Sales', N'Index', N'Sale Management', 21, 1, N'system', CAST(N'2025-12-15T01:51:21.3495540' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (68, N'Sales List', 67, N'/sales/list', N'FaListUl', N'Sales', N'List', N'Sale Management', 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3495543' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (69, N'Sales Add', 67, N'/sales/add', N'FaPlus', N'Sales', N'Add', N'Sale Management', 2, 1, N'system', CAST(N'2025-12-15T01:51:21.3495545' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (70, N'Sales Edit', 67, N'/sales/edit', N'FaEdit', N'Sales', N'Edit', N'Sale Management', 3, 1, N'system', CAST(N'2025-12-15T01:51:21.3495547' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Menus] ([Id], [MenuName], [ParentID], [MenuPath], [MenuIcon], [ControllerName], [ActionName], [ModuleName], [DisplayOrder], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (71, N'Sale Detail', 67, N'/sales/detail', N'FaInfoCircle', N'Sales', N'Detail', N'Sale Management', 4, 1, N'system', CAST(N'2025-12-15T01:51:21.3495550' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Menus] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price], [DiscountPrice], [Amount], [PromotionDetailID], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, 1, 2, CAST(25.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3523020' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price], [DiscountPrice], [Amount], [PromotionDetailID], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 1, 2, 1, CAST(25.50 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(25.50 AS Decimal(18, 2)), NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3523023' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price], [DiscountPrice], [Amount], [PromotionDetailID], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, 2, 3, 1, CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3523025' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [OrderDate], [OrderTime], [TableName], [WaiterName], [OrderStatus], [OrderType], [Total], [DiscountAmount], [DiscountPercentage], [PromotionID], [Received], [ChangeAmount], [DriverID], [CustomerID], [PaymentStatus], [PaymentMethod], [AmountPaid], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, CAST(N'2025-09-06' AS Date), N'12:30 PM', N'Table 5', N'John Doe', N'Completed', N'DineIn', CAST(75.50 AS Decimal(18, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), NULL, CAST(75.50 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 1, N'system', CAST(N'2025-12-15T01:51:21.3521916' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [OrderTime], [TableName], [WaiterName], [OrderStatus], [OrderType], [Total], [DiscountAmount], [DiscountPercentage], [PromotionID], [Received], [ChangeAmount], [DriverID], [CustomerID], [PaymentStatus], [PaymentMethod], [AmountPaid], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, CAST(N'2025-09-06' AS Date), N'01:00 PM', N'Takeout', N'Jane Smith', N'Pending', N'TakeOut', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 1, N'system', CAST(N'2025-12-15T01:51:21.3521924' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'View Users', N'USER_VIEW', N'Users', N'GetAllUsers', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490574' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Create User', N'USER_CREATE', N'Users', N'CreateUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490576' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Update User', N'USER_UPDATE', N'Users', N'UpdateUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490578' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (4, N'Delete User', N'USER_DELETE', N'Users', N'DeleteUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490579' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (5, N'Assign Role to User', N'USER_ASSIGN_ROLE', N'Users', N'AssignRoleToUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490581' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (6, N'Unassign Role from User', N'USER_UNASSIGN_ROLE', N'Users', N'UnassignRoleFromUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490583' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (7, N'Assign Multiple Roles to User', N'USER_ASSIGN_ROLES', N'Users', N'AssignRolesToUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490584' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (8, N'Unassign Multiple Roles from User', N'USER_UNASSIGN_ROLES', N'Users', N'UnassignRolesFromUser', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490586' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (9, N'Upload User Profile Picture', N'USER_UPLOAD_PROFILE_PICTURE', N'Users', N'UploadProfilePicture', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490588' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (10, N'View User Menu Permissions', N'USER_VIEW_MENU_PERMISSIONS', N'Users', N'GetMenuPermissions', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490589' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (11, N'View Menus', N'MENU_VIEW', N'Menus', N'GetAllMenus', N'Menu Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490591' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (12, N'Create Menu', N'MENU_CREATE', N'Menus', N'CreateMenu', N'Menu Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490592' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (13, N'Update Menu', N'MENU_UPDATE', N'Menus', N'UpdateMenu', N'Menu Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490594' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (14, N'Delete Menu', N'MENU_DELETE', N'Menus', N'DeleteMenu', N'Menu Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490596' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (15, N'Assign Menu to Role', N'MENU_ASSIGN_ROLE', N'Menus', N'AssignMenuToRole', N'Menu Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490597' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (16, N'Unassign Menu from Role', N'MENU_UNASSIGN_ROLE', N'Menus', N'UnassignMenuFromRole', N'Menu Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490599' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (17, N'View Audit Logs', N'AUDIT_LOG_VIEW', N'AuditLog', N'GetAll', N'System', 1, N'system', CAST(N'2025-12-15T01:51:21.3490600' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (18, N'View Permissions', N'PERMISSION_VIEW', N'Permission', N'GetAll', N'Permission Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490602' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (19, N'Create Permission', N'PERMISSION_CREATE', N'Permission', N'Create', N'Permission Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490603' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (20, N'Update Permission', N'PERMISSION_UPDATE', N'Permission', N'Update', N'Permission Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490605' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (21, N'Delete Permission', N'PERMISSION_DELETE', N'Permission', N'Delete', N'Permission Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490607' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (22, N'View Roles', N'ROLE_VIEW', N'Roles', N'GetAllRoles', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490608' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (23, N'Create Role', N'ROLE_CREATE', N'Roles', N'CreateRole', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490610' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (24, N'Update Role', N'ROLE_UPDATE', N'Roles', N'UpdateRole', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490611' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (25, N'Delete Role', N'ROLE_DELETE', N'Roles', N'DeleteRole', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490613' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (26, N'Assign Permission to Role', N'ROLE_ASSIGN_PERMISSION', N'Roles', N'AssignPermissionToRole', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490614' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (27, N'Unassign Permission from Role', N'ROLE_UNASSIGN_PERMISSION', N'Roles', N'UnassignPermissionFromRole', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490616' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (28, N'View Inventory', N'INVENTORY_VIEW', N'Inventory', N'GetAll', N'Inventory Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490618' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (29, N'View Kitchen', N'KITCHEN_VIEW', N'Kitchen', N'GetAll', N'Kitchen Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490663' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (30, N'View Role Menus', N'ROLE_VIEW_MENUS', N'Roles', N'GetRoleMenus', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490665' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (31, N'View Categories', N'CATEGORY_VIEW', N'Categories', N'GetAllCategories', N'Category Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490668' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (32, N'Create Category', N'CATEGORY_CREATE', N'Categories', N'Create', N'Category Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490670' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (33, N'Update Category', N'CATEGORY_UPDATE', N'Categories', N'Update', N'Category Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490671' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (34, N'Delete Category', N'CATEGORY_DELETE', N'Categories', N'Delete', N'Category Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490673' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (35, N'View Units', N'UNIT_VIEW', N'Units', N'GetAllUnits', N'Unit Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490676' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (36, N'Create Unit', N'UNIT_CREATE', N'Units', N'CreateUnit', N'Unit Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490678' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (37, N'Update Unit', N'UNIT_UPDATE', N'Units', N'UpdateUnit', N'Unit Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490679' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (38, N'Delete Unit', N'UNIT_DELETE', N'Units', N'DeleteUnit', N'Unit Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490682' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (39, N'View Suppliers', N'SUPPLIER_VIEW', N'Suppliers', N'GetAllSuppliers', N'Supplier Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490683' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (40, N'Create Supplier', N'SUPPLIER_CREATE', N'Suppliers', N'CreateSupplier', N'Supplier Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490685' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (41, N'Update Supplier', N'SUPPLIER_UPDATE', N'Suppliers', N'UpdateSupplier', N'Supplier Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490687' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (42, N'Delete Supplier', N'SUPPLIER_DELETE', N'Suppliers', N'DeleteSupplier', N'Supplier Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490688' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (43, N'View Manufacturers', N'MANUFACTURER_VIEW', N'Manufacturers', N'GetAllManufacturers', N'Manufacturer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490690' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (44, N'Create Manufacturer', N'MANUFACTURER_CREATE', N'Manufacturers', N'CreateManufacturer', N'Manufacturer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490691' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (45, N'Update Manufacturer', N'MANUFACTURER_UPDATE', N'Manufacturers', N'UpdateManufacturer', N'Manufacturer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490693' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (46, N'Delete Manufacturer', N'MANUFACTURER_DELETE', N'Manufacturers', N'DeleteManufacturer', N'Manufacturer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490694' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (47, N'View Products', N'PRODUCT_VIEW', N'Products', N'GetAllProducts', N'Product Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490696' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (48, N'Create Product', N'PRODUCT_CREATE', N'Products', N'CreateProduct', N'Product Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490697' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (49, N'Update Product', N'PRODUCT_UPDATE', N'Products', N'UpdateProduct', N'Product Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490699' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (50, N'Delete Product', N'PRODUCT_DELETE', N'Products', N'DeleteProduct', N'Product Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490700' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (51, N'View Customers', N'CUSTOMER_VIEW', N'Customers', N'GetAllCustomers', N'Customer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490702' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (52, N'Create Customer', N'CUSTOMER_CREATE', N'Customers', N'CreateCustomer', N'Customer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490703' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (53, N'Update Customer', N'CUSTOMER_UPDATE', N'Customers', N'UpdateCustomer', N'Customer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490705' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (54, N'Delete Customer', N'CUSTOMER_DELETE', N'Customers', N'DeleteCustomer', N'Customer Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490707' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (55, N'View Staff', N'STAFF_VIEW', N'Staff', N'GetAllStaff', N'Staff Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490708' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (56, N'Create Staff', N'STAFF_CREATE', N'Staff', N'CreateStaff', N'Staff Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490710' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (57, N'Update Staff', N'STAFF_UPDATE', N'Staff', N'UpdateStaff', N'Staff Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490711' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (58, N'Delete Staff', N'STAFF_DELETE', N'Staff', N'DeleteStaff', N'Staff Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490713' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (59, N'View Inventory', N'INVENTORY_VIEW', N'Inventory', N'GetAll', N'Inventory Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490714' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (60, N'Create Inventory', N'INVENTORY_CREATE', N'Inventory', N'Create', N'Inventory Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490716' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (61, N'Update Inventory', N'INVENTORY_UPDATE', N'Inventory', N'Update', N'Inventory Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490717' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (62, N'Delete Inventory', N'INVENTORY_DELETE', N'Inventory', N'Delete', N'Inventory Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490719' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (63, N'View Stock Transactions', N'STOCK_TRANSACTION_VIEW', N'StockTransactions', N'GetAll', N'Stock Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490728' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (64, N'Create Stock Transaction', N'STOCK_TRANSACTION_CREATE', N'StockTransactions', N'Create', N'Stock Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490730' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (65, N'Update Stock Transaction', N'STOCK_TRANSACTION_UPDATE', N'StockTransactions', N'Update', N'Stock Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490731' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (66, N'Delete Stock Transaction', N'STOCK_TRANSACTION_DELETE', N'StockTransactions', N'Delete', N'Stock Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490733' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (67, N'View Ingredients', N'INGREDIENT_VIEW', N'Ingredients', N'GetAll', N'Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490734' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (68, N'Create Ingredient', N'INGREDIENT_CREATE', N'Ingredients', N'Create', N'Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490736' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (69, N'Update Ingredient', N'INGREDIENT_UPDATE', N'Ingredients', N'Update', N'Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490737' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (70, N'Delete Ingredient', N'INGREDIENT_DELETE', N'Ingredients', N'Delete', N'Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490739' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (71, N'View Product Ingredients', N'PRODUCT_INGREDIENT_VIEW', N'ProductIngredients', N'GetAll', N'Product Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490740' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (72, N'Create Product Ingredient', N'PRODUCT_INGREDIENT_CREATE', N'ProductIngredients', N'Create', N'Product Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490742' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (73, N'Update Product Ingredient', N'PRODUCT_INGREDIENT_UPDATE', N'ProductIngredients', N'Update', N'Product Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490743' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (74, N'Delete Product Ingredient', N'PRODUCT_INGREDIENT_DELETE', N'ProductIngredients', N'Delete', N'Product Ingredient Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490745' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (75, N'View Alerts', N'ALERT_VIEW', N'Alerts', N'GetAlerts', N'Alert Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490720' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (76, N'View Orders', N'ORDER_VIEW', N'Orders', N'GetAllOrders', N'Order Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490746' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (77, N'Create Order', N'ORDER_CREATE', N'Orders', N'CreateOrder', N'Order Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490748' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (78, N'Update Order', N'ORDER_UPDATE', N'Orders', N'UpdateOrder', N'Order Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490749' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (79, N'Delete Order', N'ORDER_DELETE', N'Orders', N'DeleteOrder', N'Order Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490751' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (80, N'View Dining Tables', N'DINING_TABLE_VIEW', N'DiningTables', N'GetAllDiningTables', N'Dining Table Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490794' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (81, N'Create Dining Table', N'DINING_TABLE_CREATE', N'DiningTables', N'CreateDiningTable', N'Dining Table Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490795' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (82, N'Update Dining Table', N'DINING_TABLE_UPDATE', N'DiningTables', N'UpdateDiningTable', N'Dining Table Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490797' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (83, N'Delete Dining Table', N'DINING_TABLE_DELETE', N'DiningTables', N'DeleteDiningTable', N'Dining Table Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490799' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (84, N'Toggle User Status', N'USER_TOGGLE_STATUS', N'Users', N'UpdateUserStatus', N'User Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490800' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (85, N'Toggle Role Status', N'ROLE_TOGGLE_STATUS', N'Roles', N'UpdateRoleStatus', N'Role Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490666' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (86, N'Toggle Category Status', N'CATEGORY_TOGGLE_STATUS', N'Categories', N'UpdateStatus', N'Category Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490674' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (87, N'View Promotions', N'PROMOTION_VIEW', N'Promotions', N'GetAllPromotions', N'Promotions Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490802' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (88, N'Create Promotion', N'PROMOTION_CREATE', N'Promotions', N'CreatePromotion', N'Promotions Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490803' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (89, N'Update Promotion', N'PROMOTION_UPDATE', N'Promotions', N'UpdatePromotion', N'Promotions Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490805' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (90, N'Delete Promotion', N'PROMOTION_DELETE', N'Promotions', N'DeletePromotion', N'Promotions Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490807' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (91, N'View Purchases', N'PURCHASE_VIEW', N'Purchases', N'GetAllPurchases', N'Purchase Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490808' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (92, N'Create Purchase', N'PURCHASE_CREATE', N'Purchases', N'CreatePurchase', N'Purchase Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490810' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (93, N'Update Purchase', N'PURCHASE_UPDATE', N'Purchases', N'UpdatePurchase', N'Purchase Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490811' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (94, N'Delete Purchase', N'PURCHASE_DELETE', N'Purchases', N'DeletePurchase', N'Purchase Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490813' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (95, N'View Sales', N'SALE_VIEW', N'Sales', N'GetAllSales', N'Sale Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490814' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (96, N'Create Sale', N'SALE_CREATE', N'Sales', N'CreateSale', N'Sale Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490816' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (97, N'Update Sale', N'SALE_UPDATE', N'Sales', N'UpdateSale', N'Sale Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490818' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (98, N'Delete Sale', N'SALE_DELETE', N'Sales', N'DeleteSale', N'Sale Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490819' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Permissions] ([Id], [PermissionName], [PermissionKey], [ControllerName], [ActionName], [ModuleName], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (99, N'Acknowledge Alert', N'ALERT_ACKNOWLEDGE', N'Alerts', N'AcknowledgeAlert', N'Alert Management', 1, N'system', CAST(N'2025-12-15T01:51:21.3490722' AS DateTime2), NULL, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductIngredients] ON 

INSERT [dbo].[ProductIngredients] ([ProductIngredientID], [ProductID], [IngredientID], [Quantity], [UnitID], [Remarks], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, 1, CAST(0.50 AS Decimal(10, 2)), 1, N'Used for bread', 1, N'system', CAST(N'2025-12-15T01:51:21.3514912' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[ProductIngredients] ([ProductIngredientID], [ProductID], [IngredientID], [Quantity], [UnitID], [Remarks], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 1, 2, CAST(0.10 AS Decimal(10, 2)), 1, N'Used for sweetness', 1, N'system', CAST(N'2025-12-15T01:51:21.3514915' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[ProductIngredients] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [ProductName], [ProductPrice], [CostPrice], [ProductBarcode], [ProductImage], [ThumbnailImage], [CategoryID], [SupplierID], [ManufacturerID], [ExpireDate], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Product A', CAST(10.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), N'BARCODE001', NULL, NULL, 1, 1, 1, CAST(N'2026-12-15' AS Date), 1, N'system', CAST(N'2025-12-15T01:51:21.3502118' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Products] ([Id], [ProductName], [ProductPrice], [CostPrice], [ProductBarcode], [ProductImage], [ThumbnailImage], [CategoryID], [SupplierID], [ManufacturerID], [ExpireDate], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Product B', CAST(20.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), N'BARCODE002', NULL, NULL, 2, 2, 2, CAST(N'2027-12-15' AS Date), 1, N'system', CAST(N'2025-12-15T01:51:21.3502121' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Products] ([Id], [ProductName], [ProductPrice], [CostPrice], [ProductBarcode], [ProductImage], [ThumbnailImage], [CategoryID], [SupplierID], [ManufacturerID], [ExpireDate], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Product C', CAST(30.00 AS Decimal(18, 2)), CAST(15.00 AS Decimal(18, 2)), N'BARCODE003', NULL, NULL, 1, 1, 3, CAST(N'2028-12-15' AS Date), 0, N'system', CAST(N'2025-12-15T01:51:21.3502126' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Promotions] ON 

INSERT [dbo].[Promotions] ([PromotionID], [CouponCode], [DiscountAmount], [DiscountPercentage], [Description], [ValidFrom], [ValidTo], [IsActive], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'SUMMER20', CAST(0.00 AS Decimal(10, 2)), CAST(20.00 AS Decimal(10, 2)), N'20% off on all items for summer', CAST(N'2025-12-15T01:51:21.3516900' AS DateTime2), CAST(N'2026-01-15T01:51:21.3516900' AS DateTime2), 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3516902' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Promotions] ([PromotionID], [CouponCode], [DiscountAmount], [DiscountPercentage], [Description], [ValidFrom], [ValidTo], [IsActive], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'HALFOFF', CAST(5.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), N'$5 off on orders over $20', CAST(N'2025-12-15T01:51:21.3516905' AS DateTime2), CAST(N'2025-12-22T01:51:21.3516905' AS DateTime2), 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3516918' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Promotions] OFF
GO
SET IDENTITY_INSERT [dbo].[PurchaseDetails] ON 

INSERT [dbo].[PurchaseDetails] ([PurchaseDetailID], [PurchaseID], [ProductID], [Quantity], [UnitPrice], [TotalAmount], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, 1, 5, CAST(5.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), 1, N'system', CAST(N'2025-12-15T01:51:21.3525624' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[PurchaseDetails] ([PurchaseDetailID], [PurchaseID], [ProductID], [Quantity], [UnitPrice], [TotalAmount], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 1, 2, 3, CAST(10.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 1, N'system', CAST(N'2025-12-15T01:51:21.3525628' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[PurchaseDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Purchases] ON 

INSERT [dbo].[Purchases] ([PurchaseID], [PurchaseDate], [SupplierID], [TotalAmount], [PaymentMethod], [CreatedOn], [CategoryId], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, CAST(N'2025-12-15T01:51:21.3524949' AS DateTime2), 1, CAST(55.00 AS Decimal(18, 2)), N'Credit Card', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3524952' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Purchases] OFF
GO
SET IDENTITY_INSERT [dbo].[RoleMenus] ON 

INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1, 1, 1, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497135' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (2, 1, 2, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497137' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (3, 1, 3, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497139' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (4, 1, 4, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497140' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (5, 1, 5, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497142' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (6, 1, 6, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497143' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (7, 1, 7, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497144' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (8, 1, 8, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497146' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (9, 1, 9, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497147' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (10, 1, 10, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497148' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (11, 1, 11, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497150' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (12, 1, 12, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497152' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (13, 1, 13, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497153' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (14, 1, 14, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497154' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (15, 1, 15, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497155' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (16, 1, 16, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497157' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (17, 1, 17, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497158' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (18, 1, 18, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497159' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (19, 1, 19, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497160' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (20, 2, 1, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497163' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (21, 2, 2, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497164' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (22, 2, 3, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497165' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (23, 2, 5, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497166' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (24, 2, 6, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497167' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (25, 2, 14, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497169' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (26, 2, 15, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497171' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (27, 2, 17, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497174' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (28, 2, 18, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497175' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (29, 2, 19, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497176' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (30, 3, 1, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497178' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (31, 3, 18, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497180' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (32, 3, 19, 1, 0, 0, 0, CAST(N'2025-12-15T01:51:21.3497181' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (33, 1, 20, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497182' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (34, 1, 21, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497183' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (35, 1, 22, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497185' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (36, 1, 23, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497186' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (37, 1, 24, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497187' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (38, 1, 25, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497188' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (39, 1, 26, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497189' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (40, 1, 27, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497191' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (41, 1, 28, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497193' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (45, 1, 29, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497194' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (46, 1, 30, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497195' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (47, 1, 31, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497196' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (48, 1, 32, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497198' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (49, 1, 33, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497199' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (50, 1, 34, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497200' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (51, 1, 35, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497201' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (52, 1, 36, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497202' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (53, 1, 37, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497204' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (54, 1, 38, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497205' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (55, 1, 39, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497206' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (56, 1, 40, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497207' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (57, 1, 43, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497161' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (69, 1, 53, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497208' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (70, 1, 54, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497211' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (71, 1, 55, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497212' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (72, 1, 56, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497213' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (73, 1, 57, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497214' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (74, 1, 58, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497215' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (75, 1, 59, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497216' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (76, 1, 60, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497218' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (77, 1, 61, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497219' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (78, 1, 62, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497220' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (79, 1, 63, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497221' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (80, 1, 64, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497222' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (81, 1, 65, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497224' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (82, 1, 66, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497225' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (83, 1, 67, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497226' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (84, 1, 68, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497227' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (85, 1, 69, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497228' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (86, 1, 70, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497230' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (87, 1, 71, 1, 1, 1, 1, CAST(N'2025-12-15T01:51:21.3497256' AS DateTime2), N'System')
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1001, 1, 47, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1002, 1, 48, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1003, 1, 49, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1004, 1, 50, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1005, 1, 51, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1006, 1, 52, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1007, 1, 46, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1008, 1, 45, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1009, 1, 44, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1010, 1, 42, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[RoleMenus] ([Id], [RoleID], [MenuID], [CanView], [CanAdd], [CanEdit], [CanDelete], [AssignedAt], [AssignedBy]) VALUES (1011, 1, 41, 1, 1, 1, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[RoleMenus] OFF
GO
SET IDENTITY_INSERT [dbo].[RolePermissions] ON 

INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (31, 2, 1, CAST(N'2025-12-15T01:51:21.3491926' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (32, 2, 3, CAST(N'2025-12-15T01:51:21.3491927' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (33, 2, 11, CAST(N'2025-12-15T01:51:21.3491928' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (34, 2, 13, CAST(N'2025-12-15T01:51:21.3491929' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (35, 2, 22, CAST(N'2025-12-15T01:51:21.3491931' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (36, 2, 18, CAST(N'2025-12-15T01:51:21.3491932' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (37, 2, 17, CAST(N'2025-12-15T01:51:21.3491933' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (38, 2, 28, CAST(N'2025-12-15T01:51:21.3491934' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (39, 2, 29, CAST(N'2025-12-15T01:51:21.3491935' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (40, 2, 30, CAST(N'2025-12-15T01:51:21.3491936' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (41, 3, 1, CAST(N'2025-12-15T01:51:21.3491936' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (42, 3, 11, CAST(N'2025-12-15T01:51:21.3491937' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (43, 3, 28, CAST(N'2025-12-15T01:51:21.3491938' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (44, 3, 29, CAST(N'2025-12-15T01:51:21.3491939' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1001, 1, 1, CAST(N'2025-12-28T22:29:05.0817210' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1002, 1, 2, CAST(N'2025-12-28T22:29:05.1250205' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1003, 1, 3, CAST(N'2025-12-28T22:29:05.1260674' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1004, 1, 4, CAST(N'2025-12-28T22:29:05.1261360' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1005, 1, 5, CAST(N'2025-12-28T22:29:05.1262013' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1006, 1, 6, CAST(N'2025-12-28T22:29:05.1262495' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1007, 1, 7, CAST(N'2025-12-28T22:29:05.1262975' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1008, 1, 8, CAST(N'2025-12-28T22:29:05.1263450' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1009, 1, 9, CAST(N'2025-12-28T22:29:05.1263990' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1010, 1, 10, CAST(N'2025-12-28T22:29:05.1264531' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1011, 1, 11, CAST(N'2025-12-28T22:29:05.1265037' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1012, 1, 12, CAST(N'2025-12-28T22:29:05.1265506' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1013, 1, 13, CAST(N'2025-12-28T22:29:05.1266129' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1014, 1, 14, CAST(N'2025-12-28T22:29:05.1266593' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1015, 1, 15, CAST(N'2025-12-28T22:29:05.1267228' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1016, 1, 16, CAST(N'2025-12-28T22:29:05.1267975' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1017, 1, 17, CAST(N'2025-12-28T22:29:05.1268418' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1018, 1, 18, CAST(N'2025-12-28T22:29:05.1268842' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1019, 1, 19, CAST(N'2025-12-28T22:29:05.1269315' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1020, 1, 20, CAST(N'2025-12-28T22:29:05.1269889' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1021, 1, 21, CAST(N'2025-12-28T22:29:05.1270333' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1022, 1, 22, CAST(N'2025-12-28T22:29:05.1270793' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1023, 1, 23, CAST(N'2025-12-28T22:29:05.1271223' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1024, 1, 24, CAST(N'2025-12-28T22:29:05.1271677' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1025, 1, 25, CAST(N'2025-12-28T22:29:05.1272137' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1026, 1, 26, CAST(N'2025-12-28T22:29:05.1272764' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1027, 1, 27, CAST(N'2025-12-28T22:29:05.1273214' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1028, 1, 28, CAST(N'2025-12-28T22:29:05.1273658' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1029, 1, 29, CAST(N'2025-12-28T22:29:05.1274126' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1030, 1, 30, CAST(N'2025-12-28T22:29:05.1274564' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1031, 1, 31, CAST(N'2025-12-28T22:29:05.1275102' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1032, 1, 32, CAST(N'2025-12-28T22:29:05.1275769' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1033, 1, 33, CAST(N'2025-12-28T22:29:05.1276206' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1034, 1, 34, CAST(N'2025-12-28T22:29:05.1276638' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1035, 1, 35, CAST(N'2025-12-28T22:29:05.1277636' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1036, 1, 36, CAST(N'2025-12-28T22:29:05.1278201' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1037, 1, 37, CAST(N'2025-12-28T22:29:05.1278651' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1038, 1, 38, CAST(N'2025-12-28T22:29:05.1279033' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1039, 1, 39, CAST(N'2025-12-28T22:29:05.1279477' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1040, 1, 40, CAST(N'2025-12-28T22:29:05.1279917' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1041, 1, 41, CAST(N'2025-12-28T22:29:05.1280215' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1042, 1, 42, CAST(N'2025-12-28T22:29:05.1280697' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1043, 1, 43, CAST(N'2025-12-28T22:29:05.1281144' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1044, 1, 44, CAST(N'2025-12-28T22:29:05.1282134' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1045, 1, 45, CAST(N'2025-12-28T22:29:05.1282690' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1046, 1, 46, CAST(N'2025-12-28T22:29:05.1283019' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1047, 1, 47, CAST(N'2025-12-28T22:29:05.1283451' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1048, 1, 48, CAST(N'2025-12-28T22:29:05.1283927' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1049, 1, 49, CAST(N'2025-12-28T22:29:05.1284358' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1050, 1, 50, CAST(N'2025-12-28T22:29:05.1285037' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1051, 1, 51, CAST(N'2025-12-28T22:29:05.1285486' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1052, 1, 52, CAST(N'2025-12-28T22:29:05.1286165' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1053, 1, 53, CAST(N'2025-12-28T22:29:05.1286659' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1054, 1, 54, CAST(N'2025-12-28T22:29:05.1287253' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1055, 1, 55, CAST(N'2025-12-28T22:29:05.1287740' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1056, 1, 56, CAST(N'2025-12-28T22:29:05.1288343' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1057, 1, 57, CAST(N'2025-12-28T22:29:05.1288793' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1058, 1, 58, CAST(N'2025-12-28T22:29:05.1289184' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1059, 1, 59, CAST(N'2025-12-28T22:29:05.1289491' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1060, 1, 60, CAST(N'2025-12-28T22:29:05.1289820' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1061, 1, 61, CAST(N'2025-12-28T22:29:05.1290242' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1062, 1, 62, CAST(N'2025-12-28T22:29:05.1290797' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1063, 1, 63, CAST(N'2025-12-28T22:29:05.1291237' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1064, 1, 64, CAST(N'2025-12-28T22:29:05.1291663' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1065, 1, 65, CAST(N'2025-12-28T22:29:05.1292091' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1066, 1, 66, CAST(N'2025-12-28T22:29:05.1292523' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1067, 1, 67, CAST(N'2025-12-28T22:29:05.1292903' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1068, 1, 68, CAST(N'2025-12-28T22:29:05.1293226' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1069, 1, 69, CAST(N'2025-12-28T22:29:05.1293526' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1070, 1, 70, CAST(N'2025-12-28T22:29:05.1293942' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1071, 1, 71, CAST(N'2025-12-28T22:29:05.1294322' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1072, 1, 72, CAST(N'2025-12-28T22:29:05.1294619' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1073, 1, 73, CAST(N'2025-12-28T22:29:05.1295100' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1074, 1, 74, CAST(N'2025-12-28T22:29:05.1295535' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1075, 1, 75, CAST(N'2025-12-28T22:29:05.1295965' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1076, 1, 76, CAST(N'2025-12-28T22:29:05.1296406' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1077, 1, 77, CAST(N'2025-12-28T22:29:05.1296716' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1078, 1, 78, CAST(N'2025-12-28T22:29:05.1297048' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1079, 1, 79, CAST(N'2025-12-28T22:29:05.1297416' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1080, 1, 80, CAST(N'2025-12-28T22:29:05.1297775' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1081, 1, 81, CAST(N'2025-12-28T22:29:05.1298201' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1082, 1, 82, CAST(N'2025-12-28T22:29:05.1298625' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1083, 1, 83, CAST(N'2025-12-28T22:29:05.1299064' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1084, 1, 84, CAST(N'2025-12-28T22:29:05.1299851' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1085, 1, 87, CAST(N'2025-12-28T22:29:05.1300312' AS DateTime2), N'System', NULL)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1086, 1, 88, CAST(N'2025-12-28T22:29:05.1300790' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1087, 1, 89, CAST(N'2025-12-28T22:29:05.1301219' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1088, 1, 90, CAST(N'2025-12-28T22:29:05.1301642' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1089, 1, 91, CAST(N'2025-12-28T22:29:05.1302108' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1090, 1, 92, CAST(N'2025-12-28T22:29:05.1302654' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1091, 1, 93, CAST(N'2025-12-28T22:29:05.1303118' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1092, 1, 94, CAST(N'2025-12-28T22:29:05.1303628' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1093, 1, 95, CAST(N'2025-12-28T22:29:05.1304064' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1094, 1, 96, CAST(N'2025-12-28T22:29:05.1304496' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1095, 1, 97, CAST(N'2025-12-28T22:29:05.1304927' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1096, 1, 98, CAST(N'2025-12-28T22:29:05.1305354' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1097, 1, 99, CAST(N'2025-12-28T22:29:05.1305911' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1098, 1, 86, CAST(N'2025-12-28T22:29:05.1306355' AS DateTime2), N'System', NULL)
INSERT [dbo].[RolePermissions] ([Id], [RoleID], [PermissionID], [AssignedAt], [AssignedBy], [SortingOrder]) VALUES (1099, 1, 85, CAST(N'2025-12-28T22:29:05.1306814' AS DateTime2), N'System', NULL)
SET IDENTITY_INSERT [dbo].[RolePermissions] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Admin', N'Administrator', 1, N'system', CAST(N'2025-12-15T01:51:21.3488906' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Manager', N'Manager', 1, N'system', CAST(N'2025-12-15T01:51:21.3488908' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'User', N'Standard User', 1, N'system', CAST(N'2025-12-15T01:51:21.3488910' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[SaleDetails] ON 

INSERT [dbo].[SaleDetails] ([SaleDetailID], [SaleID], [ProductID], [Quantity], [UnitPrice], [Discount], [TotalAmount], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, 1, 2, CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 1, N'system', CAST(N'2025-12-15T01:51:21.3527537' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[SaleDetails] ([SaleDetailID], [SaleID], [ProductID], [Quantity], [UnitPrice], [Discount], [TotalAmount], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 1, 2, 1, CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 1, N'system', CAST(N'2025-12-15T01:51:21.3527540' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[SaleDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Sales] ON 

INSERT [dbo].[Sales] ([SaleID], [SaleDate], [CustomerID], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [CreatedOn], [CategoryId], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, CAST(N'2025-12-15T01:51:21.3526888' AS DateTime2), 1, CAST(40.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), N'Cash', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 1, 1, N'system', CAST(N'2025-12-15T01:51:21.3526893' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Sales] OFF
GO
SET IDENTITY_INSERT [dbo].[SplitPayments] ON 

INSERT [dbo].[SplitPayments] ([SplitPaymentID], [SaleID], [Amount], [PaymentMethod], [CreatedOn], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, CAST(20.00 AS Decimal(18, 2)), N'Cash', CAST(N'2025-12-15T01:51:21.3528234' AS DateTime2), 1, N'system', CAST(N'2025-12-15T01:51:21.3528232' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[SplitPayments] ([SplitPaymentID], [SaleID], [Amount], [PaymentMethod], [CreatedOn], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 1, CAST(20.00 AS Decimal(18, 2)), N'Card', CAST(N'2025-12-15T01:51:21.3528260' AS DateTime2), 1, N'system', CAST(N'2025-12-15T01:51:21.3528259' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[SplitPayments] OFF
GO
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([StaffID], [StaffName], [StaffPhone], [StaffRole], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Alice Johnson', N'111-222-3333', N'Manager', 1, N'system', CAST(N'2025-12-15T01:51:21.3504532' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Staff] ([StaffID], [StaffName], [StaffPhone], [StaffRole], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Bob Williams', N'444-555-6666', N'Chef', 1, N'system', CAST(N'2025-12-15T01:51:21.3504534' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Staff] OFF
GO
SET IDENTITY_INSERT [dbo].[StockTransactions] ON 

INSERT [dbo].[StockTransactions] ([TransactionID], [ProductID], [SupplierID], [TransactionType], [Quantity], [TransactionDate], [ExpireDate], [Remarks], [SaleID], [PurchaseID], [TransactionSource], [AdjustmentType], [Reason], [IngredientID], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, 1, NULL, N'IN', 10, CAST(N'2025-12-15T01:51:21.3509323' AS DateTime2), CAST(N'2026-06-15T01:51:21.3509324' AS DateTime2), N'Initial stock received', NULL, NULL, N'Purchase', NULL, NULL, NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3509329' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[StockTransactions] ([TransactionID], [ProductID], [SupplierID], [TransactionType], [Quantity], [TransactionDate], [ExpireDate], [Remarks], [SaleID], [PurchaseID], [TransactionSource], [AdjustmentType], [Reason], [IngredientID], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, 2, 1, N'OUT', 5, CAST(N'2025-12-15T01:51:21.3509331' AS DateTime2), CAST(N'2025-12-15T01:51:53.5033333' AS DateTime2), N'Sold to customer', 1, NULL, N'Sale', NULL, NULL, NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3509332' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[StockTransactions] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([Id], [SupplierName], [ContactPerson], [Phone], [Email], [Address], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Supplier A', N'Person A', N'1234567890', N'supplier.a@example.com', N'Address A', 1, N'system', CAST(N'2025-12-15T01:51:21.3499539' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Suppliers] ([Id], [SupplierName], [ContactPerson], [Phone], [Email], [Address], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Supplier B', N'Person B', N'0987654321', N'supplier.b@example.com', N'Address B', 1, N'system', CAST(N'2025-12-15T01:51:21.3499542' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Suppliers] ([Id], [SupplierName], [ContactPerson], [Phone], [Email], [Address], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Supplier C', N'Person C', N'1122334455', N'supplier.c@example.com', N'Address C', 0, N'system', CAST(N'2025-12-15T01:51:21.3499544' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
SET IDENTITY_INSERT [dbo].[Units] ON 

INSERT [dbo].[Units] ([Id], [Name], [ShortCode], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'Pieces', N'pcs', 1, N'system', CAST(N'2025-12-15T01:51:21.3499103' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Units] ([Id], [Name], [ShortCode], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'Kilograms', N'kg', 1, N'system', CAST(N'2025-12-15T01:51:21.3499105' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Units] ([Id], [Name], [ShortCode], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'Liters', N'l', 1, N'system', CAST(N'2025-12-15T01:51:21.3499106' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Units] ([Id], [Name], [ShortCode], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (4, N'Grams', N'g', 1, N'system', CAST(N'2025-12-15T01:51:21.3499107' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Units] ([Id], [Name], [ShortCode], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (5, N'Milliliters', N'ml', 1, N'system', CAST(N'2025-12-15T01:51:21.3499108' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Units] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([Id], [UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (1, 1, 1, CAST(N'2025-12-15T01:51:21.3490216' AS DateTime2), N'System')
INSERT [dbo].[UserRoles] ([Id], [UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (2, 2, 2, CAST(N'2025-12-15T01:51:21.3490218' AS DateTime2), N'System')
INSERT [dbo].[UserRoles] ([Id], [UserID], [RoleID], [AssignedAt], [AssignedBy]) VALUES (3, 3, 3, CAST(N'2025-12-15T01:51:21.3490219' AS DateTime2), N'System')
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [UserName], [PasswordHash], [PasswordSalt], [FullName], [Email], [Phone], [ProfilePicture], [RefreshToken], [RefreshTokenExpiry], [PasswordResetToken], [PasswordResetTokenExpiry], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (1, N'admin', N'c4vD3op8WBxFFjk_XPZoHA', N'qtixrauL4wM-8gdAhr6rAA', N'System Administrator', N'admin@example.com', N'0000000000', NULL, N'xwIXFsu5ozpp6eNA9eQZQ/1B08H2Jhf4HtmVmVLbKcM=', CAST(N'2026-01-05T07:28:40.5110373' AS DateTime2), NULL, NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3488539' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Users] ([Id], [UserName], [PasswordHash], [PasswordSalt], [FullName], [Email], [Phone], [ProfilePicture], [RefreshToken], [RefreshTokenExpiry], [PasswordResetToken], [PasswordResetTokenExpiry], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (2, N'manager', N'd4QTV4pwUJ-pwL2B2Y4V_w', N'cZ7UtVxlTYIEb97pOqfoBQ', N'Manager User', N'manager@example.com', N'0987654321', NULL, NULL, NULL, NULL, NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3488542' AS DateTime2), NULL, NULL, 0)
INSERT [dbo].[Users] ([Id], [UserName], [PasswordHash], [PasswordSalt], [FullName], [Email], [Phone], [ProfilePicture], [RefreshToken], [RefreshTokenExpiry], [PasswordResetToken], [PasswordResetTokenExpiry], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted]) VALUES (3, N'user', N'6Fn94S0iWXBrFbYv5v4Yxg', N'6Fn94S0iWXBrFbYv5v4Yxg', N'Standard User', N'user@example.com', N'0987654321', NULL, NULL, NULL, NULL, NULL, 1, N'system', CAST(N'2025-12-15T01:51:21.3488544' AS DateTime2), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Ingredients_SupplierID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Ingredients_SupplierID] ON [dbo].[Ingredients]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ingredients_UnitID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Ingredients_UnitID] ON [dbo].[Ingredients]
(
	[UnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Inventory_ProductID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Inventory_ProductID] ON [dbo].[Inventory]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Menus_ParentID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Menus_ParentID] ON [dbo].[Menus]
(
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_OrderID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_OrderID] ON [dbo].[OrderDetails]
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_ProductID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_ProductID] ON [dbo].[OrderDetails]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_CustomerID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerID] ON [dbo].[Orders]
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductIngredients_IngredientID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_ProductIngredients_IngredientID] ON [dbo].[ProductIngredients]
(
	[IngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductIngredients_ProductID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_ProductIngredients_ProductID] ON [dbo].[ProductIngredients]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductIngredients_UnitID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_ProductIngredients_UnitID] ON [dbo].[ProductIngredients]
(
	[UnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_CategoryID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Products_CategoryID] ON [dbo].[Products]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_ManufacturerID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Products_ManufacturerID] ON [dbo].[Products]
(
	[ManufacturerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_SupplierID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Products_SupplierID] ON [dbo].[Products]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseDetails_ProductID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseDetails_ProductID] ON [dbo].[PurchaseDetails]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseDetails_PurchaseID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseDetails_PurchaseID] ON [dbo].[PurchaseDetails]
(
	[PurchaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Purchases_CategoryId]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Purchases_CategoryId] ON [dbo].[Purchases]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Purchases_SupplierID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Purchases_SupplierID] ON [dbo].[Purchases]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RoleMenus_MenuID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_RoleMenus_MenuID] ON [dbo].[RoleMenus]
(
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RoleMenus_RoleID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_RoleMenus_RoleID] ON [dbo].[RoleMenus]
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RolePermissions_PermissionID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_RolePermissions_PermissionID] ON [dbo].[RolePermissions]
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RolePermissions_RoleID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_RolePermissions_RoleID] ON [dbo].[RolePermissions]
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SaleDetails_ProductID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_SaleDetails_ProductID] ON [dbo].[SaleDetails]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SaleDetails_SaleID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_SaleDetails_SaleID] ON [dbo].[SaleDetails]
(
	[SaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Sales_CategoryId]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Sales_CategoryId] ON [dbo].[Sales]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Sales_CustomerID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_Sales_CustomerID] ON [dbo].[Sales]
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SplitPayments_SaleID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_SplitPayments_SaleID] ON [dbo].[SplitPayments]
(
	[SaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockTransactions_ProductID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_StockTransactions_ProductID] ON [dbo].[StockTransactions]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockTransactions_SupplierID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_StockTransactions_SupplierID] ON [dbo].[StockTransactions]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRoles_RoleID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_UserRoles_RoleID] ON [dbo].[UserRoles]
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRoles_UserID]    Script Date: 08/01/2026 01:58:44 ******/
CREATE NONCLUSTERED INDEX [IX_UserRoles_UserID] ON [dbo].[UserRoles]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (getutcdate()) FOR [LastUpdated]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ProductIngredients] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[ProductIngredients] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[ProductIngredients] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ProductIngredients] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT ((0.0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT ((0.0)) FOR [DiscountPercentage]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsActive]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Promotions] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[StockTransactions] ADD  DEFAULT (getutcdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[StockTransactions] ADD  DEFAULT (getutcdate()) FOR [ExpireDate]
GO
ALTER TABLE [dbo].[StockTransactions] ADD  DEFAULT (CONVERT([bit],(1))) FOR [Status]
GO
ALTER TABLE [dbo].[StockTransactions] ADD  DEFAULT (N'system') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[StockTransactions] ADD  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[StockTransactions] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Ingredients]  WITH CHECK ADD  CONSTRAINT [FK_Ingredients_Suppliers_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([Id])
GO
ALTER TABLE [dbo].[Ingredients] CHECK CONSTRAINT [FK_Ingredients_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[Ingredients]  WITH CHECK ADD  CONSTRAINT [FK_Ingredients_Units_UnitID] FOREIGN KEY([UnitID])
REFERENCES [dbo].[Units] ([Id])
GO
ALTER TABLE [dbo].[Ingredients] CHECK CONSTRAINT [FK_Ingredients_Units_UnitID]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Products_ProductID]
GO
ALTER TABLE [dbo].[Menus]  WITH CHECK ADD  CONSTRAINT [FK_Menus_Menus_ParentID] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Menus] ([Id])
GO
ALTER TABLE [dbo].[Menus] CHECK CONSTRAINT [FK_Menus_Menus_ParentID]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders_OrderID] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders_OrderID]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Products_ProductID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers_CustomerID]
GO
ALTER TABLE [dbo].[ProductIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ProductIngredients_Ingredients_IngredientID] FOREIGN KEY([IngredientID])
REFERENCES [dbo].[Ingredients] ([IngredientID])
GO
ALTER TABLE [dbo].[ProductIngredients] CHECK CONSTRAINT [FK_ProductIngredients_Ingredients_IngredientID]
GO
ALTER TABLE [dbo].[ProductIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ProductIngredients_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[ProductIngredients] CHECK CONSTRAINT [FK_ProductIngredients_Products_ProductID]
GO
ALTER TABLE [dbo].[ProductIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ProductIngredients_Units_UnitID] FOREIGN KEY([UnitID])
REFERENCES [dbo].[Units] ([Id])
GO
ALTER TABLE [dbo].[ProductIngredients] CHECK CONSTRAINT [FK_ProductIngredients_Units_UnitID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories_CategoryID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Manufacturers_ManufacturerID] FOREIGN KEY([ManufacturerID])
REFERENCES [dbo].[Manufacturers] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Manufacturers_ManufacturerID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Suppliers_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetails_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_PurchaseDetails_Products_ProductID]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetails_Purchases_PurchaseID] FOREIGN KEY([PurchaseID])
REFERENCES [dbo].[Purchases] ([PurchaseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_PurchaseDetails_Purchases_PurchaseID]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_Purchases_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_Purchases_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_Purchases_Suppliers_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_Purchases_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[RoleMenus]  WITH CHECK ADD  CONSTRAINT [FK_RoleMenus_Menus_MenuID] FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMenus] CHECK CONSTRAINT [FK_RoleMenus_Menus_MenuID]
GO
ALTER TABLE [dbo].[RoleMenus]  WITH CHECK ADD  CONSTRAINT [FK_RoleMenus_Roles_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMenus] CHECK CONSTRAINT [FK_RoleMenus_Roles_RoleID]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permissions_PermissionID] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permissions_PermissionID]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Roles_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Roles_RoleID]
GO
ALTER TABLE [dbo].[SaleDetails]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetails_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SaleDetails] CHECK CONSTRAINT [FK_SaleDetails_Products_ProductID]
GO
ALTER TABLE [dbo].[SaleDetails]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetails_Sales_SaleID] FOREIGN KEY([SaleID])
REFERENCES [dbo].[Sales] ([SaleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SaleDetails] CHECK CONSTRAINT [FK_SaleDetails_Sales_SaleID]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Customers_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Customers_CustomerID]
GO
ALTER TABLE [dbo].[SplitPayments]  WITH CHECK ADD  CONSTRAINT [FK_SplitPayments_Sales_SaleID] FOREIGN KEY([SaleID])
REFERENCES [dbo].[Sales] ([SaleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SplitPayments] CHECK CONSTRAINT [FK_SplitPayments_Sales_SaleID]
GO
ALTER TABLE [dbo].[StockTransactions]  WITH CHECK ADD  CONSTRAINT [FK_StockTransactions_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[StockTransactions] CHECK CONSTRAINT [FK_StockTransactions_Products_ProductID]
GO
ALTER TABLE [dbo].[StockTransactions]  WITH CHECK ADD  CONSTRAINT [FK_StockTransactions_Suppliers_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([Id])
GO
ALTER TABLE [dbo].[StockTransactions] CHECK CONSTRAINT [FK_StockTransactions_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleID]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserID]
GO
USE [master]
GO
ALTER DATABASE [RMSDB] SET  READ_WRITE 
GO
