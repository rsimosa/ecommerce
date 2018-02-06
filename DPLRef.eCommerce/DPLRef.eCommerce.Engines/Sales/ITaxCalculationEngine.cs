using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.WebStore.Sales;

namespace DPLRef.eCommerce.Engines.Sales
{
    public interface ITaxCalculationEngine : IServiceContractBase
    {
        WebStoreCart CalculateCartTax(WebStoreCart cart);
    }
}
