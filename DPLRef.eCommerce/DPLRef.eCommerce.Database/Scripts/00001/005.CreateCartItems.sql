
create table CartItems (
    Id int identity(1,1) not null primary key,
    CartId uniqueidentifier not null,
    CatalogId int not null,
    ProductId int not null,
    Quantity int not null,

    CreatedAt datetimeoffset not null default(getdate()),
    UpdatedAt datetimeoffset not null default(getdate())
)
 
ALTER TABLE [dbo].CartItems
ADD CONSTRAINT FK_CartItems_CartId FOREIGN KEY (CartId)     
    REFERENCES Carts (Id);

ALTER TABLE [dbo].CartItems
ADD CONSTRAINT FK_CartItems_ProductId FOREIGN KEY (ProductId)     
    REFERENCES Products (Id);

ALTER TABLE [dbo].CartItems
ADD CONSTRAINT FK_CartItems_CatalogId FOREIGN KEY (CatalogId)     
    REFERENCES Catalogs (Id);
