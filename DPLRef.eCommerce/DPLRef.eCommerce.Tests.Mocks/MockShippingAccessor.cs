using DPLRef.eCommerce.Accessors.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockShippingAccessor : MockBase, IShippingAccessor
    {
        public MockShippingAccessor(MockData data) : base(data)
        {

        }

        public ShippingResult RequestShipping(int orderId)
        {
            MockData.OrderShippingRequested = true;
            return MockData.ShippingResult;   
        }

        public string TestMe(string input)
        {
            return input;
        }
    }
}
