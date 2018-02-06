using AutoMapper;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using CatAcc = DPLRef.eCommerce.Accessors.Catalog;
using SalesAcc = DPLRef.eCommerce.Accessors.Sales;
using RemittanceAcc = DPLRef.eCommerce.Accessors.Remittance;
using WSCatContract = DPLRef.eCommerce.Contracts.WebStore.Catalog;
using WSSalesContract = DPLRef.eCommerce.Contracts.WebStore.Sales;
using AdminCatContract = DPLRef.eCommerce.Contracts.Admin.Catalog;
using BackOfficeremittanceContract = DPLRef.eCommerce.Contracts.BackOfficeAdmin.Remittance;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DPLRef.eCommerce.Tests.ManagerTests")]

namespace DPLRef.eCommerce.Managers
{
    internal static class DTOMapper
    {
        static IMapper _mapper;
        private static IConfigurationProvider _config;

        private static IMapper Mapper => _mapper ?? (_mapper = Configuration.CreateMapper());

        public static IConfigurationProvider Configuration
        {
            get
            {
                if (_config == null)
                {
                    var config = new AutoMapper.MapperConfiguration(cfg =>
                    {
                        // NOTE: CreateMap<Source, Destination>()
                        // ForMember(destination member, source mapping/member option)
                        #region WebStore Catalog

                        cfg.CreateMap<WebStoreCatalog, WSCatContract.WebStoreCatalog>()
                            .ForMember(a => a.Products, b => b.Ignore());

                        cfg.CreateMap<Product, WSCatContract.ProductSummary>();

                        cfg.CreateMap<Product, WSCatContract.ProductDetail>();

                        #endregion

                        #region WebStore Order

                        cfg.CreateMap<WSSalesContract.WebStoreCartItem, OrderLine>();

                        cfg.CreateMap<WSSalesContract.WebStoreCart, Order>()
                            .ForMember(a => a.FromCartId, opt => opt.MapFrom(b => b.Id))
                            .ForMember(a => a.OrderLines, opt => opt.MapFrom(b => b.CartItems))
                            .ForMember(a => a.Id, b => b.Ignore())
                            .ForMember(a => a.Status, b => b.Ignore())
                            .ForMember(a => a.AuthorizationCode, b => b.Ignore())
                            .ForMember(a => a.SellerId, b => b.Ignore())
                            .ForMember(a => a.ShippingProvider, b => b.Ignore())
                            .ForMember(a => a.TrackingCode, b => b.Ignore())
                            .ForMember(a => a.Notes, b => b.Ignore());

                        cfg.CreateMap<Order, WSSalesContract.WebStoreOrder>()
                            .ForMember(a => a.OrderLines, opt => opt.MapFrom(b => b.OrderLines));

                        cfg.CreateMap<OrderLine, WSSalesContract.WebStoreOrderLine>();

                        #endregion

                        #region Admin Catalog
                        cfg.CreateMap<AdminCatContract.WebStoreCatalog, WebStoreCatalog>()
                            .ForMember(a => a.SellerId, b => b.Ignore())
                            .ForMember(a => a.SellerName, b => b.Ignore())
                            .ForMember(a => a.IsApproved, b => b.Ignore());

                        cfg.CreateMap<WebStoreCatalog, AdminCatContract.WebStoreCatalog>();

                        cfg.CreateMap<AdminCatContract.Product, Product>();

                        cfg.CreateMap<Product, AdminCatContract.Product>();
                        #endregion

                        #region Unfulfilled Orders

                        cfg.CreateMap<Order, AdminUnfulfilledOrder>()
                            .ForMember(a => a.OrderLines, opt => opt.MapFrom(b => b.OrderLines));

                        cfg.CreateMap<OrderLine, AdminUnfulfilledOrderLine>();

                        #endregion

                        #region BackOffice
                        cfg.CreateMap<SellerOrderData, BackOfficeremittanceContract.SellerOrderData>()
                            .ForMember(a => a.FeeAmount, b => b.Ignore())
                            .ForMember(a => a.RemittanceAmount, b => b.Ignore());
                        cfg.CreateMap<BackOfficeremittanceContract.SellerOrderData, SellerOrderData>();
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
    }
}
