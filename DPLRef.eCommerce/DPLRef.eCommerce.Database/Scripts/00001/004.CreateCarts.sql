
create table Carts (
    Id uniqueidentifier not null primary key,
    CatalogId int not null,
    BillingFirst nvarchar(50) null,
    BillingLast nvarchar(50) null,
    BillingEmailAddress nvarchar(50) null,
    BillingAddr1 nvarchar(50) null,
    BillingAddr2 nvarchar(50) null,
    BillingCity nvarchar(50) null,
    BillingState nvarchar(50) null,
    BillingPostal  nvarchar(50) null,
    ShippingFirst nvarchar(50) null,
    ShippingLast nvarchar(50) null,
    ShippingEmailAddress nvarchar(50) null,
    ShippingAddr1 nvarchar(50) null,
    ShippingAddr2 nvarchar(50) null,
    ShippingCity nvarchar(50) null,
    ShippingState nvarchar(50) null,
    ShippingPostal  nvarchar(50) null,
    CreatedAt datetimeoffset not null default(getdate()),
    UpdatedAt datetimeoffset not null default(getdate())
)
 
ALTER TABLE [dbo].Carts
ADD CONSTRAINT FK_Carts_CatalogId FOREIGN KEY (CatalogId)     
    REFERENCES Catalogs (Id);
