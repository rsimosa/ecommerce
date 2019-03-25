using AutoMapper;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DPLRef.eCommerce.Tests.AccessorTests")]
[assembly: InternalsVisibleTo("DPLRef.eCommerce.Tests.IntegrationTests")]

namespace DPLRef.eCommerce.Accessors
{
    //static class DTOMapper
    //{
    //    public static T Map<T>(object obj)
    //    {
    //        var dest = Activator.CreateInstance<T>();
    //        Map(obj, dest);
    //        return dest;
    //    }

    //    public static void Map(object obj, object dest)
    //    {
    //        DTOPropCopy.CopyProps(obj, dest);
    //    }

    //    public static WebStoreCatalog Map(EntityFramework.CatalogExtended c)
    //    {
    //        var result = Map<WebStoreCatalog>(c.Catalog);
    //        result.SellerName = c.SellerName;
    //        return result;
    //    }
    //}

    internal static class DTOMapper
    {
        static IMapper _mapper;
        private static IConfigurationProvider _config;

        static IMapper Mapper => _mapper ?? (_mapper = Configuration.CreateMapper());

        public static IConfigurationProvider Configuration
        {
            get
            {
                if (_config == null)
                {
                    var config = new AutoMapper.MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<EntityFramework.Catalog, WebStoreCatalog>()
                           .ForMember(a => a.SellerName, b => b.Ignore());

                        cfg.CreateMap<WebStoreCatalog, EntityFramework.Catalog>()
                           .ForMember(a => a.CreatedAt, b => b.Ignore())
                           .ForMember(a => a.UpdatedAt, b => b.Ignore());

                        cfg.CreateMap<EntityFramework.Product, Product>();

                        cfg.CreateMap<Product, EntityFramework.Product>()
                           .ForMember(a => a.CreatedAt, b => b.Ignore())
                           .ForMember(a => a.UpdatedAt, b => b.Ignore());

                        cfg.CreateMap<EntityFramework.Seller, Seller>();
                        cfg.CreateMap<Seller, EntityFramework.Seller>()
                           .ForMember(a => a.CreatedAt, b => b.Ignore())
                           .ForMember(a => a.UpdatedAt, b => b.Ignore());


                        //cfg.CreateMap<EntityFramework.Cart, Cart>()
                        //   .ForMember(a => a.BillingAddress, b => b.Ignore())
                        //   .ForMember(a => a.ShippingAddress, b => b.Ignore())
                        //   .ForMember(a => a.CartItems, b => b.Ignore());

                        cfg.CreateMap<EntityFramework.OrderLine, OrderLine>();

                        #region Cart

                        cfg.CreateMap<EntityFramework.CartItem, CartItem>()
                           .ForMember(a => a.ProductName, b => b.Ignore());

                        cfg.CreateMap<EntityFramework.Cart, Cart>()
                        .ForMember(a => a.BillingAddress, b => b.MapFrom(c => new Address
                        {
                            EmailAddress = c.BillingEmailAddress,
                            First = c.BillingFirst,
                            Last = c.BillingLast,
                            Addr1 = c.BillingAddr1,
                            Addr2 = c.BillingAddr2,
                            City = c.BillingCity,
                            Postal = c.BillingPostal,
                            State = c.BillingState,
                        }))
                        .ForMember(a => a.ShippingAddress, b => b.MapFrom(c => new Address
                        {
                            EmailAddress = c.ShippingEmailAddress,
                            First = c.ShippingFirst,
                            Last = c.ShippingLast,
                            Addr1 = c.ShippingAddr1,
                            Addr2 = c.ShippingAddr2,
                            City = c.ShippingCity,
                            Postal = c.ShippingPostal,
                            State = c.ShippingState,
                        }))

                        .ForMember(a => a.CartItems, b => b.Ignore());

                        cfg.CreateMap<Cart, EntityFramework.Cart>()

                        .ForMember(a => a.CatalogId, b => b.Ignore())
                        .ForMember(a => a.CreatedAt, b => b.Ignore())
                        .ForMember(a => a.UpdatedAt, b => b.Ignore())

                        .ForMember(a => a.BillingFirst, b => b.MapFrom(c => c.BillingAddress.First))
                        .ForMember(a => a.BillingLast, b => b.MapFrom(c => c.BillingAddress.Last))
                        .ForMember(a => a.BillingEmailAddress, b => b.MapFrom(c => c.BillingAddress.EmailAddress))

                        .ForMember(a => a.BillingCity, b => b.MapFrom(c => c.BillingAddress.City))
                        .ForMember(a => a.BillingPostal, b => b.MapFrom(c => c.BillingAddress.Postal))
                        .ForMember(a => a.BillingAddr1, b => b.MapFrom(c => c.BillingAddress.Addr1))
                        .ForMember(a => a.BillingAddr2, b => b.MapFrom(c => c.BillingAddress.Addr2))
                        .ForMember(a => a.BillingState, b => b.MapFrom(c => c.BillingAddress.State))

                        .ForMember(a => a.ShippingFirst, b => b.MapFrom(c => c.ShippingAddress.First))
                        .ForMember(a => a.ShippingLast, b => b.MapFrom(c => c.ShippingAddress.Last))
                        .ForMember(a => a.ShippingEmailAddress, b => b.MapFrom(c => c.ShippingAddress.EmailAddress))

                        .ForMember(a => a.ShippingCity, b => b.MapFrom(c => c.ShippingAddress.City))
                        .ForMember(a => a.ShippingPostal, b => b.MapFrom(c => c.ShippingAddress.Postal))
                        .ForMember(a => a.ShippingAddr1, b => b.MapFrom(c => c.ShippingAddress.Addr1))
                        .ForMember(a => a.ShippingAddr2, b => b.MapFrom(c => c.ShippingAddress.Addr2))
                        .ForMember(a => a.ShippingState, b => b.MapFrom(c => c.ShippingAddress.State));

                        #endregion

                    });
                    _config = config;
                }
                return _config;
            }
        }



        public static void Map(object source, object dest)
        {
            Mapper.Map(source, dest, source.GetType(), dest.GetType());
        }

        public static T Map<T>(object source)
        {
            return Mapper.Map<T>(source);
        }


        public static Order MapOrder(EntityFramework.Order model)
        {
            var result = new Order();

            result.Id = model.Id;
            result.AuthorizationCode = model.AuthorizationCode;
            result.ShippingProvider = model.ShippingProvider;
            result.TrackingCode = model.TrackingCode;
            result.Notes = model.Notes;
            result.SubTotal = model.SubTotal;
            result.TaxAmount = model.TaxAmount;
            result.Total = model.Total;
            result.Status = model.Status;
            result.Total = model.Total;
            result.SellerId = model.SellerId;

            result.BillingAddress = new Common.Contracts.Address();
            result.BillingAddress.First = model.BillingFirst;
            result.BillingAddress.Last = model.BillingLast;
            result.BillingAddress.Addr1 = model.BillingAddr1;
            result.BillingAddress.EmailAddress = model.BillingEmailAddress;
            result.BillingAddress.Addr2 = model.BillingAddr2;
            result.BillingAddress.City = model.BillingCity;
            result.BillingAddress.State = model.BillingState;
            result.BillingAddress.Postal = model.BillingPostal;

            result.ShippingAddress = new Common.Contracts.Address();
            result.ShippingAddress.First = model.ShippingFirst;
            result.ShippingAddress.Last = model.ShippingLast;
            result.ShippingAddress.EmailAddress = model.ShippingEmailAddress;
            result.ShippingAddress.Addr1 = model.ShippingAddr1;
            result.ShippingAddress.Addr2 = model.ShippingAddr2;
            result.ShippingAddress.City = model.ShippingCity;
            result.ShippingAddress.State = model.ShippingState;
            result.ShippingAddress.Postal = model.ShippingPostal;

            return result;
        }
        public static void MapOrder(Order order, EntityFramework.Order model)
        {
            // TODO: We need to change the mapping to be more robust and create test errors if we are missing fields to be mapped         
            model.Id = order.Id;
            model.AuthorizationCode = order.AuthorizationCode;
            model.SubTotal = order.SubTotal;
            model.TaxAmount = order.TaxAmount;
            model.Total = order.Total;
            model.Status = order.Status;
        }


        public static void MapBilling(Common.Contracts.Address address, EntityFramework.Cart cart)
        {
            cart.BillingFirst = address.First;
            cart.BillingLast = address.Last;
            cart.BillingEmailAddress = address.EmailAddress;
            cart.BillingAddr1 = address.Addr1;
            cart.BillingAddr2 = address.Addr2;
            cart.BillingCity = address.City;
            cart.BillingState = address.State;
            cart.BillingPostal = address.Postal;
        }

        public static void MapShipping(Common.Contracts.Address address, EntityFramework.Cart cart)
        {
            cart.ShippingFirst = address.First;
            cart.ShippingLast = address.Last;
            cart.ShippingEmailAddress = address.EmailAddress;
            cart.ShippingAddr1 = address.Addr1;
            cart.ShippingAddr2 = address.Addr2;
            cart.ShippingCity = address.City;
            cart.ShippingState = address.State;
            cart.ShippingPostal = address.Postal;
        }

        public static void MapBilling(Address address, EntityFramework.Order order)
        {
            order.BillingFirst = address.First;
            order.BillingLast = address.Last;
            order.BillingEmailAddress = address.EmailAddress;
            order.BillingAddr1 = address.Addr1;
            order.BillingAddr2 = address.Addr2;
            order.BillingCity = address.City;
            order.BillingState = address.State;
            order.BillingPostal = address.Postal;
        }

        public static void MapShipping(Address address, EntityFramework.Order order)
        {
            order.ShippingFirst = address.First;
            order.ShippingLast = address.Last;
            order.ShippingEmailAddress = address.EmailAddress;
            order.ShippingAddr1 = address.Addr1;
            order.ShippingAddr2 = address.Addr2;
            order.ShippingCity = address.City;
            order.ShippingState = address.State;
            order.ShippingPostal = address.Postal;
        }

        public static WebStoreCatalog Map(EntityFramework.CatalogExtended c)
        {
            var result = Map<WebStoreCatalog>(c.Catalog);
            result.SellerName = c.SellerName;
            return result;
        }
    }
}
