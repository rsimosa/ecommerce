using DPLRef.eCommerce.Accessors.Remittance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockRemittanceAccessor : MockBase, IRemittanceAccessor
    {
        public MockRemittanceAccessor(MockData data) : base(data)
        {

        }

        public SellerOrderData[] SalesTotal()
        {
            return MockData.SellerOrderData.ToArray();
        }

        public string TestMe(string input)
        {
            return input;
        }
    }
}
