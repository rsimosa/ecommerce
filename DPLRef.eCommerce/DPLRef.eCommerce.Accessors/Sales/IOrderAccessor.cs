using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Accessors.Sales
{
    public interface IOrderAccessor : IServiceContractBase
    {
        Order SaveOrder(int catalogId, Order order);

        Order FindOrder(int id);

        SellerSalesTotal SalesTotal();

        Order[] UnfulfilledOrders();

        Order UpdateOrderStatus(int orderId, OrderStatuses status, string notes);

        Order FulfillOrder(int orderId, string shippingProvider, string trackingCode, string notes);
    }
}
