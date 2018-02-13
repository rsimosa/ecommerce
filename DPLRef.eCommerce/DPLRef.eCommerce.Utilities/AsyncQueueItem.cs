using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Utilities
{
    public class AsyncQueueItem
    {
        public AsyncEventTypes EventType { get; set; }
        public int EventId { get; set; }
    }
}
