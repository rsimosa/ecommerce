create table Products (
    Id int identity(1,1) not null primary key,
    CatalogId int not null,
    SellerProductId nvarchar(50) null,
    Name nvarchar(50) null,
    Summary nvarchar(max) null,	
    Detail nvarchar(max) null,		
    Price decimal(18,2) not null,
    CreatedAt datetimeoffset not null default(getdate()),
    UpdatedAt datetimeoffset not null default(getdate()),
    SupplierName nvarchar(50) null,
    ShippingWeight decimal(18,2) not null,
    IsAvailable bit not null default(1),
    IsDownloadable bit not null default(0)
)
 
ALTER TABLE [dbo].Products
ADD CONSTRAINT FK_Products_CatalogId FOREIGN KEY (CatalogId)     
    REFERENCES Catalogs (Id);
