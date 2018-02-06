using DPLRef.eCommerce.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockSecurityUtility : MockBase, ISecurityUtility
    {
        public MockSecurityUtility(MockData data) : base(data)
        {

        }

        public bool BackOfficeAdminAuthenticated()
        {
            return string.IsNullOrWhiteSpace(MockData.Context.AuthToken) ? false : true;
        }

        public bool SellerAuthenticated()
        {
            return string.IsNullOrWhiteSpace(MockData.Context.AuthToken) ? false : true;
        }

        public string TestMe(string input)
        {
            return input;

        }
    }
}
