using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;
using System.Linq;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class RemittanceAccessorTests : DbTestAccessorBase
    {
        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void RemittanceAccessor_SalesTotal()
        {
            var order = new Order()
            {
                BillingAddress = new Address()
                {
                    First = "Bob",
                    Last = "Smith",
                    EmailAddress = "bob.smith@dontpaniclabsl.com",
                    Addr1 = "address1",
                    City = "Lincoln",
                    State = "Nebraska",
                    Postal = "68508",
                },
                ShippingAddress = new Address()
                {
                    First = "Bob",
                    Last = "Smith",
                    EmailAddress = "bob.smith@dontpaniclabsl.com",
                    Addr1 = "address1",
                    City = "Lincoln",
                    State = "Nebraska",
                    Postal = "68508",
                },
                OrderLines = new OrderLine[]
                {
                    new OrderLine()
                    {
                        ProductId = 1,
                        UnitPrice = 10.0M,
                        ExtendedPrice = 10.0M,
                        Quantity = 1,
                    }
                },
                Total = 10.0M
            };

            const int sellerId = 1;
            const int catalogId = 2;

            var orderAccessor = CreateOrderAccessor();
            var remittanceAccessor = CreateRemittanceAccessor(); 

            var before = remittanceAccessor.SalesTotal();
            Assert.IsNotNull(before);
            var beforeOrderData = before.FirstOrDefault(o => o.SellerId == sellerId);
            var beforeOrderDataCount = 0;
            if (beforeOrderData != null)
            {
                beforeOrderDataCount = beforeOrderData.OrderCount;
            }

            var saved = orderAccessor.SaveOrder(catalogId, order);

            Assert.IsNotNull(saved);
            
            var after = remittanceAccessor.SalesTotal();
            Assert.IsNotNull(after);

            var afterOrderData = after.FirstOrDefault(o => o.SellerId == sellerId);
            var afterOrderDataCount = afterOrderData.OrderCount;

            Assert.AreEqual(beforeOrderDataCount + 1, afterOrderDataCount);
        }
    }
}
