using AutoMapper;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using System.Runtime.CompilerServices;
using SalesAcc = DPLRef.eCommerce.Accessors.Sales;
using WSSalesContract = DPLRef.eCommerce.Contracts.WebStore.Sales;

[assembly: InternalsVisibleTo("DPLRef.eCommerce.Tests.EngineTests")]

namespace DPLRef.eCommerce.Engines
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

                        #region Sales

                        cfg.CreateMap<Cart, WSSalesContract.WebStoreCart>()
                            .ForMember(a => a.TaxAmount, b => b.Ignore())
                            .ForMember(a => a.SubTotal, b => b.Ignore())
                            .ForMember(a => a.Total, b => b.Ignore());

                        cfg.CreateMap<CartItem, WSSalesContract.WebStoreCartItem>()
                            .ForMember(a => a.UnitPrice, b => b.Ignore())
                            .ForMember(a => a.ExtendedPrice, b => b.Ignore());

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
