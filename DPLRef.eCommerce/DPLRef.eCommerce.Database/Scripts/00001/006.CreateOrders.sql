create table Orders (
    Id int identity(1,1) not null primary key,
    FromCartId uniqueidentifier null,
    BillingFirst nvarchar(50) null,
    BillingLast nvarchar(50) null,
	BillingEmailAddress nvarchar(50) not null,
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
    --CouponCode  nvarchar(50) null,
    CreatedAt datetimeoffset not null default(getdate()),
    UpdatedAt datetimeoffset not null default(getdate()),
    SubTotal decimal(18,2) not null,
    TaxAmount decimal(18,2) not null,
    Total decimal(18,2) not null,
	AuthorizationCode nvarchar(100) null,
	ShippingProvider nvarchar(100) null,
	TrackingCode nvarchar(100) null,
	Notes nvarchar(200) null,
	SellerId int not null,
	CatalogId int not null,
	[Status] bigint
	not null
)
 
ALTER TABLE [dbo].Orders
ADD CONSTRAINT FK_Orders_SellerId FOREIGN KEY (SellerId)     
    REFERENCES Sellers (Id);

ALTER TABLE [dbo].Orders
ADD CONSTRAINT FK_Orders_CatalogId FOREIGN KEY (CatalogId)     
    REFERENCES Catalogs (Id);



