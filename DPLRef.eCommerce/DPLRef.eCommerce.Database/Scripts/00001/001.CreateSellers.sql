create table Sellers (
	Id int identity(1,1) not null primary key,
	Name nvarchar(50) null,
	CreatedAt datetimeoffset not null default(getdate()),
	UpdatedAt datetimeoffset not null default(getdate()),
	IsApproved bit not null default(0),
	BankRoutingNumber int not null default(0),
	BankAccountNumber int not null default(0),
	OrderNotificationEmail nvarchar(1000) null,
	UserName varchar(50) not null
)
