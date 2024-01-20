DROP TABLE IF EXISTS ProductImageEntities;
DROP TABLE IF EXISTS ProductPriceEntities;
DROP TABLE IF EXISTS ProductVariantEntities;
DROP TABLE IF EXISTS ProductEntities;
DROP TABLE IF EXISTS CurrencyEntities;
DROP TABLE IF EXISTS ImageEntities;
DROP TABLE IF EXISTS SizeEntities;
DROP TABLE IF EXISTS ColorEntities;
DROP TABLE IF EXISTS CategoryEntities;
DROP TABLE IF EXISTS BrandEntities;

CREATE TABLE BrandEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    BrandName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE CategoryEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    ParentCategoryId INT NULL REFERENCES CategoryEntities(Id),
    CategoryName NVARCHAR(50) NOT NULL
);

CREATE TABLE ColorEntities
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    ColorName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE SizeEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    SizeType VARCHAR(20), 
    SizeValue VARCHAR(4), 
    AgeGroup VARCHAR(4), 
    CONSTRAINT UC_Size UNIQUE (SizeType, SizeValue, AgeGroup)
);

CREATE TABLE ImageEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    ImageUrl NVARCHAR(250) NOT NULL UNIQUE
);

CREATE TABLE CurrencyEntities 
(
    Code CHAR(3) NOT NULL PRIMARY KEY,
    CurrencyName NVARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE ProductEntities 
(
    ArticleNumber VARCHAR(30) NOT NULL PRIMARY KEY,
    ProductName NVARCHAR(200) NOT NULL,
    Material NVARCHAR(MAX) NULL,
    ProductInfo NVARCHAR(MAX) NULL,
    CategoryId INT NOT NULL REFERENCES CategoryEntities(Id),
    BrandId INT NOT NULL REFERENCES BrandEntities(Id)
);

CREATE TABLE ProductVariantEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    ArticleNumber VARCHAR(30) NOT NULL REFERENCES ProductEntities(ArticleNumber),
    Quantity INT NOT NULL,
    SizeId INT NOT NULL REFERENCES SizeEntities(Id),
    ColorId INT NOT NULL REFERENCES ColorEntities(Id),
    CONSTRAINT UC_ProductVariant UNIQUE (ArticleNumber, SizeId, ColorId)
);

CREATE TABLE ProductPriceEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    ProductVariantId INT NOT NULL REFERENCES ProductVariantEntities(Id),
    ArticleNumber VARCHAR(30) NOT NULL,
    Price MONEY NOT NULL,
    DiscountPrice MONEY NULL,
    DicountPercentage DECIMAL(5, 2) NULL,
    CurrencyCode CHAR(3) NOT NULL REFERENCES CurrencyEntities(Code),
    CONSTRAINT UC_ProductPrice UNIQUE (ProductVariantId, Price)
);

CREATE TABLE ProductImageEntities 
(
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    ProductVariantId INT NOT NULL REFERENCES ProductVariantEntities(Id),
    ArticleNumber VARCHAR(30) NOT NULL,
    ImageId INT NOT NULL REFERENCES ImageEntities(Id),
    CONSTRAINT UC_ProductImage UNIQUE (ProductVariantId, ImageId)
);
