using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.Sales
{
    public interface ICartAccessor : IServiceContractBase
    {
        Cart ShowCart(int catalogId);

        Cart SaveBillingInfo(int catalogId, Address billingAddress);

        Cart SaveShippingInfo(int catalogId, Address shippingAddress);

        Cart SaveCartItem(int catalogId, int productId, int quantity);

        bool DeleteCart(int catalogId);
    }
}
