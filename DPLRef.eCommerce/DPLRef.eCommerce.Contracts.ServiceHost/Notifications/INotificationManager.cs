using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.ServiceHost.Notifications
{
    public interface INotificationManager : IServiceContractBase
    {
        NotificationResponse SendNewOrderNotices(int orderId);

        NotificationResponse SendOrderFulfillmentNotices(int orderId);
    }
}
