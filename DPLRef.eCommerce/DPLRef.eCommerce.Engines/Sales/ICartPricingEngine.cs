
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.WebStore.Sales;

namespace DPLRef.eCommerce.Engines.Sales
{
    public interface ICartPricingEngine : IServiceContractBase
    {
        WebStoreCart GenerateCartPricing(Cart cart);
    }
}