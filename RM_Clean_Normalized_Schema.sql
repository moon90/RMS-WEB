-- =============================================
-- RM Database - Normalized and Clean Schema
-- =============================================
CREATE DATABASE RM_Clean;
GO
USE RM_Clean;
GO

-- ================
-- Common Lookup Tables
-- ================

CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL
);

CREATE TABLE Units (
    UnitID INT IDENTITY(1,1) PRIMARY KEY,
    UnitName VARCHAR(50) NOT NULL,
    ShortCode VARCHAR(5) NOT NULL,
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100),
    UpdatedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName VARCHAR(100) NOT NULL,
    ContactPerson VARCHAR(100),
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Address NVARCHAR(250),
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100),
    UpdatedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Manufacturers (
    ManufacturerID INT IDENTITY(1,1) PRIMARY KEY,
    ManufacturerName VARCHAR(100),
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100),
    UpdatedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

-- ================
-- Products & Inventory
-- ================

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    ProductPrice DECIMAL(18,2) NOT NULL,
    CostPrice DECIMAL(18,2),
    ProductBarcode VARCHAR(100),
    ProductImage VARBINARY(MAX),
    ThumbnailImage VARBINARY(MAX),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    ManufacturerID INT FOREIGN KEY REFERENCES Manufacturers(ManufacturerID),
    ExpireDate DATE,
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Inventory (
    InventoryID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    InitialStock INT NOT NULL,
    CurrentStock INT NOT NULL,
    MinStockLevel INT NOT NULL,
    LastUpdated DATETIME DEFAULT GETUTCDATE(),
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);


-- ================
-- User & Role Management
-- ================

CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250),
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100),
    UpdatedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE UserRoles (
    UserRoleID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID)
);

CREATE TABLE Permissions (
    PermissionID INT IDENTITY(1,1) PRIMARY KEY,
    PermissionName NVARCHAR(100) NOT NULL,
    FormName NVARCHAR(100),
    IsActive BIT DEFAULT 1
);

CREATE TABLE RolePermissions (
    RolePermissionID INT IDENTITY(1,1) PRIMARY KEY,
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID),
    PermissionID INT FOREIGN KEY REFERENCES Permissions(PermissionID)
);

-- ================
-- Audit
-- ================

CREATE TABLE AuditLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    ActionType VARCHAR(50) NOT NULL,
    EntityName VARCHAR(100) NOT NULL,
    EntityID INT NOT NULL,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    ChangedBy NVARCHAR(100) NOT NULL,
    ChangedOn DATETIME DEFAULT GETUTCDATE()
);

-- =============================================
-- Redesigned RM Schema for Best Practices
-- =============================================
-- This version includes:
-- - Consistent naming conventions (PascalCase)
-- - Proper foreign key relationships
-- - Data normalization across related entities
-- - Audit fields (CreatedBy, ModifiedBy, Timestamps, Status, IsDeleted)
-- - Indexed foreign keys and frequently queried fields
-- - Improved stored procedures for security, clarity, and performance


-- ================
-- Customer & Staff
-- ================

CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName VARCHAR(100) NOT NULL,
    CustomerPhone VARCHAR(20),
    CustomerEmail VARCHAR(100),
    Address NVARCHAR(250),
    DriverName VARCHAR(100),
    Status BIT DEFAULT 1,
    CreatedBy VARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedBy VARCHAR(100),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    StaffName VARCHAR(100),
    StaffPhone VARCHAR(20),
    StaffRole VARCHAR(100),
    Status BIT DEFAULT 1,
    CreatedBy VARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedBy VARCHAR(100),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

-- ================
-- Ingredients & Products
-- ================

CREATE TABLE Ingredients (
    IngredientID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    QuantityAvailable DECIMAL(10,2) NOT NULL,
    UnitID INT NOT NULL FOREIGN KEY REFERENCES Units(UnitID),
    ReorderLevel DECIMAL(10,2) NOT NULL,
    ReorderQuantity DECIMAL(10,2) NOT NULL,
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    ExpireDate DATE DEFAULT GETUTCDATE(),
    Remarks NVARCHAR(250),
    CreatedBy NVARCHAR(50),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(50),
    UpdatedDate DATETIME DEFAULT GETUTCDATE(),
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE ProductIngredients (
    ProductIngredientID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    IngredientID INT NOT NULL FOREIGN KEY REFERENCES Ingredients(IngredientID),
    Quantity DECIMAL(10,2) NOT NULL,
    UnitID INT NOT NULL FOREIGN KEY REFERENCES Units(UnitID),
    Remarks NVARCHAR(250),
    CreatedBy NVARCHAR(50),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(50),
    UpdatedDate DATETIME DEFAULT GETUTCDATE(),
    IsDeleted BIT DEFAULT 0
);


-- ================
-- Promotions
-- ================

CREATE TABLE Promotions (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,
    CouponCode VARCHAR(50) NOT NULL,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    DiscountPercentage DECIMAL(10,2) DEFAULT 0,
    Description VARCHAR(255),
    ValidFrom DATE NOT NULL,
    ValidTo DATETIME NOT NULL,
    IsActive BIT DEFAULT 1
);

-- ================
-- Purchases
-- ================

CREATE TABLE Purchases (
    PurchaseID INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseDate DATETIME DEFAULT GETUTCDATE(),
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(50),
    CreatedOn DATETIME DEFAULT GETUTCDATE()
);

CREATE TABLE PurchaseDetails (
    PurchaseDetailID INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseID INT NOT NULL FOREIGN KEY REFERENCES Purchases(PurchaseID),
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL
);

-- ================
-- Sales
-- ================

CREATE TABLE Sales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    SaleDate DATETIME DEFAULT GETUTCDATE(),
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    TotalAmount DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) DEFAULT 0,
    FinalAmount DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(50),
    CreatedOn DATETIME DEFAULT GETUTCDATE()
);

CREATE TABLE SaleDetails (
    SaleDetailID INT IDENTITY(1,1) PRIMARY KEY,
    SaleID INT NOT NULL FOREIGN KEY REFERENCES Sales(SaleID),
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0,
    TotalAmount DECIMAL(18,2) NOT NULL
);

CREATE TABLE SplitPayments (
    SplitPaymentID INT IDENTITY(1,1) PRIMARY KEY,
    SaleID INT NOT NULL FOREIGN KEY REFERENCES Sales(SaleID),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    CreatedOn DATETIME DEFAULT GETUTCDATE()
);


-- ================
-- Tables (Seating/Restaurant Context)
-- ================

CREATE TABLE DiningTables (
    TableID INT IDENTITY(1,1) PRIMARY KEY,
    TableName VARCHAR(100),
    Status BIT DEFAULT 1,
    CreatedBy VARCHAR(100) NOT NULL DEFAULT 'system',
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedBy VARCHAR(100),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

-- ================
-- Orders and Details (tblMaster/tblDetails normalized)
-- ================

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATE,
    OrderTime VARCHAR(15),
    TableName VARCHAR(50),
    WaiterName VARCHAR(50),
    OrderStatus VARCHAR(20),
    OrderType VARCHAR(20),
    Total DECIMAL(18,2) DEFAULT 0,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    DiscountPercentage DECIMAL(10,2) DEFAULT 0,
    PromotionID INT FOREIGN KEY REFERENCES Promotions(PromotionID),
    Received DECIMAL(18,2) DEFAULT 0,
    ChangeAmount DECIMAL(18,2) DEFAULT 0,
    DriverID INT,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID)
);

CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT,
    Price DECIMAL(18,2),
    DiscountPrice DECIMAL(18,2),
    Amount DECIMAL(18,2),
    PromotionDetailID INT
);

-- ================
-- Stock Transactions
-- ================

CREATE TABLE StockTransactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    TransactionType VARCHAR(10) NOT NULL,
    Quantity INT NOT NULL,
    TransactionDate DATETIME DEFAULT GETUTCDATE(),
    ExpireDate DATE DEFAULT GETUTCDATE(),
    Remarks NVARCHAR(250),
    SaleID INT,
    PurchaseID INT,
    TransactionSource VARCHAR(50)
);

-- =============================================
-- Audit/Status Fields Update
-- =============================================

ALTER TABLE Products ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Inventory ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Categories ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Suppliers ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Manufacturers ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Units ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Users ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Roles ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Permissions ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE UserRoles ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE RolePermissions ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Customers ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Staff ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Ingredients ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE ProductIngredients ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Promotions ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Purchases ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE PurchaseDetails ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Sales ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE SaleDetails ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE Orders ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE OrderDetails ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE StockTransactions ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;

ALTER TABLE DiningTables ADD
    Status BIT DEFAULT 1,
    CreatedBy NVARCHAR(100) NOT NULL DEFAULT 'system',
    ModifiedBy NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME,
    IsDeleted BIT DEFAULT 0;