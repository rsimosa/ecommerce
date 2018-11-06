using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Utilities
{
    public class AsyncQueueItem
    {
        public AmbientContext AmbientContext { get; set;} //<== allows context to traverse asynchronous workflow
        public AsyncEventTypes EventType { get; set; }
        public int EventId { get; set; }
    }
}
