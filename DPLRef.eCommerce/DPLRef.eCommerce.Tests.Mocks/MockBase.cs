using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public abstract class MockBase
    {
        public MockData MockData { get; set; }

        public MockBase(MockData data) 
        {
            MockData = data;
        }
    }
}
