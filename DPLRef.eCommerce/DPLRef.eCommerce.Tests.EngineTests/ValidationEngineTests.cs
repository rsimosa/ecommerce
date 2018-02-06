using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Engines.Sales;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DPLRef.eCommerce.Tests.EngineTests
{

    [TestClass]
    public class ValidationEngineTests : EngineTestBase
    {
        #region Test Objects

        private Address _validAddress = new Address()
        {
            Addr1 = "Address 1",
            City = "City",
            First = "First",
            Last = "Last",
            Postal = "Postal",
            State = "State"
        };

        #endregion

        [TestMethod]
        [TestCategory("Engine Tests")]
        public void ValidationEngine_ValidateOrder()
        {
            var factory = new EngineFactory(new AmbientContext(), null, null);

            var eng = factory.CreateEngine<IOrderValidationEngine>();

            // null order
            var response = eng.ValidateOrder(null);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Order is invalid", response.Message);

            // order lines is null
            var order = new Order();
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Order has no order lines", response.Message);

            // order lines length = 0
            order.OrderLines = new OrderLine[0];
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Order has no order lines", response.Message);

            // invlaid extended price
            order.OrderLines = new OrderLine[]
            {
                new OrderLine()
                {
                    ProductId = 1,
                    ProductName = "Product Name",
                    Quantity = 1,
                    UnitPrice = 1.00m,
                    ExtendedPrice = 2.00m
                },
                new OrderLine()
                {
                    ProductId = 2,
                    ProductName = "Product Name 2",
                    Quantity = 1,
                    UnitPrice = 1.00m,
                    ExtendedPrice = 1.00m
                }
            };
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid order line pricing", response.Message);

            // invalid order subtotal
            order.OrderLines[0].ExtendedPrice = 1.00m;
            order.SubTotal = 3.00m;
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid order subtotal", response.Message);

            // invalid order total
            order.SubTotal = 2.00m;
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid order total", response.Message);

            // invalid shipping address
            order.TaxAmount = 0.14m;
            order.Total = 2.14m;
            order.ShippingAddress = new Address();
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("ShippingAddress address is not valid", response.Message);

            // invalid billing address
            order.SubTotal = 2.00m;
            order.ShippingAddress = _validAddress;
            order.BillingAddress = new Address();
            response = eng.ValidateOrder(order);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("BillingAddress address is not valid", response.Message);

            // successful order
            order.BillingAddress = _validAddress;
            response = eng.ValidateOrder(order);
            Assert.IsTrue(response.Success);

            // skip billing address check when order subtotal = 0
            order.SubTotal = 0.00m;
            order.TaxAmount = 0.00m;
            order.Total = 0.00m;
            order.OrderLines = new OrderLine[]
            {
                new OrderLine()
                {
                    ProductId = 1,
                    ProductName = "Product Name",
                    Quantity = 1,
                    UnitPrice = 0.00m,
                    ExtendedPrice = 0.00m
                },
                new OrderLine()
                {
                    ProductId = 2,
                    ProductName = "Product Name 2",
                    Quantity = 1,
                    UnitPrice = 0.00m,
                    ExtendedPrice = 0.00m
                }
            };
            order.BillingAddress = new Address();
            response = eng.ValidateOrder(order);
            Assert.IsTrue(response.Success);
        }
    }
}
