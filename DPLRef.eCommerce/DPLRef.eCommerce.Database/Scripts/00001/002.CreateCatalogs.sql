
create table Catalogs (
    Id int identity(1,1) not null primary key,
    SellerId int not null,
    Name nvarchar(50) null,
    Description nvarchar(max) null,
    IsApproved bit not null default(1),
    CreatedAt datetimeoffset not null default(getdate()),
    UpdatedAt datetimeoffset not null default(getdate())
)

ALTER TABLE [dbo].Catalogs
ADD CONSTRAINT FK_Catalogs_SellerId FOREIGN KEY (SellerId)     
    REFERENCES Sellers (Id);

