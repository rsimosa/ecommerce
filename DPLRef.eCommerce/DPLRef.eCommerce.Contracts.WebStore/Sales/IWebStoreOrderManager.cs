using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.WebStore.Sales
{
    public interface IWebStoreOrderManager : IServiceContractBase
    {
        WebStoreOrderResponse SubmitOrder(int catalogId, PaymentInstrument paymentInstrument);
    }
}
