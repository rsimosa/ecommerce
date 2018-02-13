using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockAsyncUtility : MockBase, IAsyncUtility
    {
        public MockAsyncUtility(MockData data) : base(data)
        {

        }

        public AsyncQueueItem CheckForNewItem()
        {
            return null;
        }

        public void SendEvent(AsyncEventTypes eventType, int eventId)
        {
            MockData.AsyncCalled = true;
        }

        public string TestMe(string input)
        {
            return input;
        }
    }
}
