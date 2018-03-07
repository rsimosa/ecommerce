
create table OrderLines (
    Id int identity(1,1) not null primary key,
    OrderId int not null,
    FromCartItemId int not null,

    ProductId int not null,
    Quantity int not null,
    ProductName nvarchar(50) null,
    CreatedAt datetimeoffset not null default(getdate()),
    UpdatedAt datetimeoffset not null default(getdate()),

    UnitPrice decimal(18,2) not null,
    ExtendedPrice decimal(18,2) not null
)

ALTER TABLE [dbo].OrderLines
ADD CONSTRAINT FK_OrderLines_OrderId FOREIGN KEY (OrderId)     
    REFERENCES Orders (Id);

ALTER TABLE [dbo].OrderLines
ADD CONSTRAINT FK_OrderLines_ProductId FOREIGN KEY (ProductId)     
    REFERENCES Products (Id);

