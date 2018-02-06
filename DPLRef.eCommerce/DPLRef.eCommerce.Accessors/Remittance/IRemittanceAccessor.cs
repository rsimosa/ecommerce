using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Accessors.Remittance
{
    public interface IRemittanceAccessor : IServiceContractBase
    {
        SellerOrderData[] SalesTotal();
    }
}
