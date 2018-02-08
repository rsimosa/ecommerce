create procedure SalesTotalForSeller
(
	@sellerId as int
)
as
begin

	select o.SellerId, s.Name as SellerName, Count(1) as OrderCount, Sum(o.Total) as OrderTotal
	from Orders o
	join Sellers s on s.Id = o.SellerId
	where o.SellerId = @sellerId
	group by o.SellerId, s.Name

end