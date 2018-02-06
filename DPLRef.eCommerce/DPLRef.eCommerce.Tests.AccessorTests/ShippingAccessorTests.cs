using System;
using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class ShippingAccessorTests : DbTestAccessorBase
    {
        private IShippingAccessor CreateShippingAccessor()
        {
            var accessor = new AccessorFactory(Context, new UtilityFactory(Context));
            var result = accessor.CreateAccessor<IShippingAccessor>();
            return result;
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void ShippingAccessor_RequestShipping()
        {
            var accessor = CreateShippingAccessor();
            var result = accessor.RequestShipping(1); // sending order id > 0 will succeed

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(string.IsNullOrEmpty(result.ShippingProvider));
            Assert.IsFalse(string.IsNullOrEmpty(result.TrackingCode));
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void ShippingAccessor_RequestShippingFail()
        {
            var accessor = CreateShippingAccessor();
            var result = accessor.RequestShipping(0); // sending 0 for order id will cause fail

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsTrue(string.IsNullOrEmpty(result.ShippingProvider));
            Assert.IsTrue(string.IsNullOrEmpty(result.TrackingCode));
        }
    }
}
