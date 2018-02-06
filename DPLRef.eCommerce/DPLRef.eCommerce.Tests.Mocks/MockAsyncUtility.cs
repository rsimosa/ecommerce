using DPLRef.eCommerce.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockAsyncUtility : MockBase, IAsyncUtility
    {
        public MockAsyncUtility(MockData data) : base(data)
        {

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
