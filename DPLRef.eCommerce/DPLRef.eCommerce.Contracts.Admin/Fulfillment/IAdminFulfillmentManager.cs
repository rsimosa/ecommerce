using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Fulfillment
{
    public interface IAdminFulfillmentManager: IServiceContractBase
    {
        /// <summary>
        /// Returns the unfulfilled orders for the seller
        /// </summary>
        /// <returns></returns>
        AdminOpenOrdersResponse GetOrdersToFulfill();

        /// <summary>
        /// Udpates status and captures payment for fulfilled orders
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        AdminFulfillmentResponse FulfillOrder(int orderId);
    }
}
