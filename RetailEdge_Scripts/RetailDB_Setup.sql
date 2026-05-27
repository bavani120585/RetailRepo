-- ============================================================================
-- RetailEdge Retail App Database Setup
-- SQL Server T-SQL Script
-- Database: RetailDB
-- Server: (localdb)\MSSQLLocalDB
-- ============================================================================

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'RetailDB')
BEGIN
    CREATE DATABASE RetailDB;
END
GO

USE RetailDB;
GO

-- ============================================================================
-- Drop existing tables if they exist (for clean setup)
-- ============================================================================
IF OBJECT_ID('dbo.CartItems', 'U') IS NOT NULL
    DROP TABLE dbo.CartItems;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL
    DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO

-- ============================================================================
-- Create Users Table
-- ============================================================================
CREATE TABLE dbo.Users
(
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,  -- Plain text for dev only - DO NOT USE IN PRODUCTION
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(500),
    City NVARCHAR(100),
    State NVARCHAR(50),
    PostalCode NVARCHAR(20),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    LastLogin DATETIME2
);

-- Create index on Email for faster lookups
CREATE INDEX IX_Users_Email ON dbo.Users(Email);

-- Create index on IsActive for filtering active users
CREATE INDEX IX_Users_IsActive ON dbo.Users(IsActive);

GO

-- ============================================================================
-- Create Products Table
-- ============================================================================
CREATE TABLE dbo.Products
(
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Category NVARCHAR(100),
    SKU NVARCHAR(50) UNIQUE NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,

    -- Check constraints
    CONSTRAINT CK_Products_Price CHECK (Price > 0),
    CONSTRAINT CK_Products_Stock CHECK (Stock >= 0)
);

-- Create index on Category for filtering
CREATE INDEX IX_Products_Category ON dbo.Products(Category);

-- Create index on IsActive for filtering active products
CREATE INDEX IX_Products_IsActive ON dbo.Products(IsActive);

-- Create index on SKU for unique lookups
CREATE INDEX IX_Products_SKU ON dbo.Products(SKU);

GO

-- ============================================================================
-- Create CartItems Table
-- ============================================================================
CREATE TABLE dbo.CartItems
(
    CartItemID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    AddedDate DATETIME2 DEFAULT GETDATE(),

    -- Foreign Keys with CASCADE DELETE
    CONSTRAINT FK_CartItems_Users FOREIGN KEY (UserID)
        REFERENCES dbo.Users(UserID) ON DELETE CASCADE,

    CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductID)
        REFERENCES dbo.Products(ProductID) ON DELETE CASCADE,

    -- Check constraints
    CONSTRAINT CK_CartItems_Quantity CHECK (Quantity > 0)
);

-- Create index on UserID for faster cart lookups by user
CREATE INDEX IX_CartItems_UserID ON dbo.CartItems(UserID);

-- Create index on ProductID for product usage analysis
CREATE INDEX IX_CartItems_ProductID ON dbo.CartItems(ProductID);

-- Create composite index for user-product lookups
CREATE UNIQUE INDEX IX_CartItems_UserProduct ON dbo.CartItems(UserID, ProductID);

GO

-- ============================================================================
-- Insert Sample Data - Users (8 users)
-- ============================================================================
INSERT INTO dbo.Users (Email, FirstName, LastName, Password, PhoneNumber, Address, City, State, PostalCode, IsActive)
VALUES
    ('raj.sharma@email.com', 'Raj', 'Sharma', 'password123', '+91-9876543210', '123 MG Road', 'Bangalore', 'KA', '560001', 1),
    ('priya.patel@email.com', 'Priya', 'Patel', 'secure@pass', '+91-9123456789', '456 Bandra Street', 'Mumbai', 'MH', '400050', 1),
    ('amit.gupta@email.com', 'Amit', 'Gupta', 'password456', '+91-8765432109', '789 CP Extension', 'Delhi', 'DL', '110016', 1),
    ('sneha.kulkarni@email.com', 'Sneha', 'Kulkarni', 'pass@word789', '+91-9999888877', '321 Indiranagar', 'Bangalore', 'KA', '560038', 1),
    ('vikram.singh@email.com', 'Vikram', 'Singh', 'vikram2024', '+91-8888777666', '654 Jubilee Road', 'Hyderabad', 'TG', '500001', 1),
    ('deepa.nair@email.com', 'Deepa', 'Nair', 'deepa@123', '+91-7777666555', '987 Fort Kochi Lane', 'Kochi', 'KL', '682001', 1),
    ('anuj.malhotra@email.com', 'Anuj', 'Malhotra', 'anuj456pass', '+91-6666555444', '111 Golf Course Road', 'Gurgaon', 'HR', '122001', 1),
    ('neha.desai@email.com', 'Neha', 'Desai', 'neha@secure', '+91-5555444333', '222 Koregaon Park', 'Pune', 'MH', '411001', 1);

GO

-- ============================================================================
-- Insert Sample Data - Products (10 products with INR pricing)
-- ============================================================================
INSERT INTO dbo.Products (ProductName, Description, Price, Stock, Category, SKU, IsActive)
VALUES
    ('Wireless Bluetooth Headphones', 'High-quality sound with noise cancellation', 3499.00, 45, 'Electronics', 'WBH-001', 1),
    ('Stainless Steel Water Bottle', 'Keeps water cold for 24 hours', 899.00, 120, 'Accessories', 'SWB-002', 1),
    ('Cotton T-Shirt Pack', 'Pack of 3 comfortable cotton t-shirts', 1199.00, 85, 'Apparel', 'CTP-003', 1),
    ('USB-C Fast Charging Cable', 'High-speed data and charging', 349.00, 200, 'Electronics', 'UFC-004', 1),
    ('Running Sports Shoes', 'Professional grade athletic footwear', 4999.00, 30, 'Footwear', 'RSS-005', 1),
    ('Portable Power Bank 20000mAh', 'Fast charging capability with LED display', 1499.00, 60, 'Electronics', 'PPB-006', 1),
    ('Canvas Backpack', 'Durable and spacious travel backpack', 2199.00, 40, 'Accessories', 'CBP-007', 1),
    ('Smart Watch', 'Fitness tracking and notifications', 6999.00, 25, 'Electronics', 'SMW-008', 1),
    ('Yoga Mat with Carrying Strap', 'Non-slip exercise and meditation mat', 799.00, 75, 'Sports', 'YMC-009', 1),
    ('Bamboo Coffee Mug Set', 'Eco-friendly reusable mug set', 649.00, 110, 'Kitchen', 'BCM-010', 1);

GO

-- ============================================================================
-- Insert Sample Data - CartItems (15 cart items across users)
-- ============================================================================
INSERT INTO dbo.CartItems (UserID, ProductID, Quantity, AddedDate)
VALUES
    (1, 1, 1, DATEADD(HOUR, -5, GETDATE())),      -- Raj: 1x Wireless Headphones
    (1, 4, 2, DATEADD(HOUR, -4, GETDATE())),      -- Raj: 2x USB-C Cables
    (2, 5, 1, DATEADD(HOUR, -3, GETDATE())),      -- Priya: 1x Running Shoes
    (2, 2, 3, DATEADD(HOUR, -2, GETDATE())),      -- Priya: 3x Water Bottles
    (3, 6, 1, DATEADD(HOUR, -2, GETDATE())),      -- Amit: 1x Power Bank
    (3, 3, 2, DATEADD(HOUR, -1, GETDATE())),      -- Amit: 2x T-Shirt Packs
    (4, 8, 1, DATEADD(MINUTE, -45, GETDATE())),   -- Sneha: 1x Smart Watch
    (4, 9, 1, DATEADD(MINUTE, -40, GETDATE())),   -- Sneha: 1x Yoga Mat
    (5, 1, 1, DATEADD(MINUTE, -35, GETDATE())),   -- Vikram: 1x Wireless Headphones
    (5, 7, 2, DATEADD(MINUTE, -30, GETDATE())),   -- Vikram: 2x Canvas Backpacks
    (6, 10, 2, DATEADD(MINUTE, -25, GETDATE())),  -- Deepa: 2x Coffee Mug Sets
    (6, 2, 1, DATEADD(MINUTE, -20, GETDATE())),   -- Deepa: 1x Water Bottle
    (7, 4, 3, DATEADD(MINUTE, -15, GETDATE())),   -- Anuj: 3x USB-C Cables
    (7, 6, 1, DATEADD(MINUTE, -10, GETDATE())),   -- Anuj: 1x Power Bank
    (8, 3, 1, DATEADD(MINUTE, -5, GETDATE()));    -- Neha: 1x T-Shirt Pack

GO

-- ============================================================================
-- Verification Queries
-- ============================================================================
PRINT '========== USER DATA =========='
SELECT COUNT(*) AS TotalUsers FROM dbo.Users;
SELECT * FROM dbo.Users ORDER BY UserID;

PRINT ''
PRINT '========== PRODUCT DATA =========='
SELECT COUNT(*) AS TotalProducts FROM dbo.Products;
SELECT * FROM dbo.Products ORDER BY ProductID;

PRINT ''
PRINT '========== CART ITEMS DATA =========='
SELECT COUNT(*) AS TotalCartItems FROM dbo.CartItems;
SELECT
    ci.CartItemID,
    u.Email,
    p.ProductName,
    ci.Quantity,
    p.Price,
    (ci.Quantity * p.Price) AS CartItemTotal,
    ci.AddedDate
FROM dbo.CartItems ci
    JOIN dbo.Users u ON ci.UserID = u.UserID
    JOIN dbo.Products p ON ci.ProductID = p.ProductID
ORDER BY ci.AddedDate DESC;

PRINT ''
PRINT '========== CART SUMMARY BY USER =========='
SELECT
    u.Email,
    u.FirstName,
    u.LastName,
    COUNT(ci.CartItemID) AS ItemCount,
    SUM(ISNULL(ci.Quantity * p.Price, 0)) AS CartTotal
FROM dbo.Users u
    LEFT JOIN dbo.CartItems ci ON u.UserID = ci.UserID
    LEFT JOIN dbo.Products p ON ci.ProductID = p.ProductID
WHERE u.IsActive = 1
GROUP BY u.UserID, u.Email, u.FirstName, u.LastName
ORDER BY SUM(ISNULL(ci.Quantity * p.Price, 0)) DESC;

-- ============================================================================
-- End of Setup Script
-- ============================================================================
