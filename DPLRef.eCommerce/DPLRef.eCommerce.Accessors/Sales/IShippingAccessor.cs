using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Accessors.Sales
{
    public interface IShippingAccessor : IServiceContractBase
    {
        ShippingResult RequestShipping(int orderId);
    }
}
