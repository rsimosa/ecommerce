using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.WebStore.Sales
{
    public interface IWebStoreCartManager : IServiceContractBase
    {
        /// <summary>
        /// Add a product to the shopping cart or change its quantity
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        WebStoreCartResponse SaveCartItem(int catalogId, int productId, int quantity);

        /// <summary>
        /// Remove a product from the shopping cart
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        WebStoreCartResponse RemoveCartItem(int catalogId, int productId);

        /// <summary>
        /// Returns the webstore shopping cart details
        /// </summary>
        /// <returns></returns>
        WebStoreCartResponse ShowCart(int catalogId);

        /// <summary>
        /// Adds or updates billing information (and optionally shipping information) in the shopping cart
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="billingInfo"></param>
        /// <param name="shippingSameAsBilling"></param>
        /// <returns></returns>
        WebStoreCartResponse UpdateBillingInfo(int catalogId, Address billingInfo, bool shippingSameAsBilling);

        /// <summary>
        /// Add or updates shipping information in the shopping cart
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="shippingInfo"></param>
        /// <param name="billingSameAsShipping"></param>
        /// <returns></returns>
        WebStoreCartResponse UpdateShippingInfo(int catalogId, Address shippingInfo, bool billingSameAsShipping);
    }
}