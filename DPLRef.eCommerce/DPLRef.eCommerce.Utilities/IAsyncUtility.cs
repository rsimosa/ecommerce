using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Utilities
{
    public interface IAsyncUtility : IServiceContractBase
    {
        void SendEvent(AsyncEventTypes eventType, int eventId);

        AsyncQueueItem CheckForNewItem();
    }
}
