using System;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class OrderAccessorTests : DbTestAccessorBase
    {
        #region Test Objects

        Address BillingAddress
        {
            get
            {
                return new Address()
                {
                    First = "Bob",
                    Last = "Smith",
                    EmailAddress = "bob.smith@dontpaniclabsl.com",
                    Addr1 = "4273 Commerce Boulevard",
                    City = "Lincoln",
                    State = "Nebraska",
                    Postal = "68508",
                };
            }
        }

        Address ShippingAddress
        {
            get
            {
                return BillingAddress;
            }
        }

        Order CreateUnfulfilledOrderObject()
        {
            return new Order()
            {
                BillingAddress = BillingAddress,
                ShippingAddress = ShippingAddress,
                OrderLines = new OrderLine[]
                    {
                    new OrderLine()
                    {
                        ProductId = 1,
                        ProductName = "Test Product Name",
                        UnitPrice = 10.0M,
                        ExtendedPrice = 10.0M,
                        Quantity = 1,
                    }
                    },
                AuthorizationCode = "12345",
                Status = OrderStatuses.Authorized,
                SubTotal = 10.0M,
                TaxAmount = 0.70M,
                Total = 10.70M
            };
        }

        #endregion

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_Submit()
        {
            var order = new Order()
            {
                BillingAddress = BillingAddress,
                ShippingAddress = ShippingAddress,
                OrderLines = new OrderLine[] 
                {
                    new OrderLine()
                    {
                        ProductId = 1,
                        ProductName = "Test Product Name",
                        UnitPrice = 10.0M,
                        ExtendedPrice = 10.0M,
                        Quantity = 1,
                    }
                },
                AuthorizationCode = "12345",
                Status = OrderStatuses.Authorized,
                SubTotal = 10.0M,
                TaxAmount = 0.70M,
                Total = 10.70M                    
            };

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);

            Assert.IsNotNull(saved);
            Assert.IsTrue(saved.Id > 0);
            Assert.AreEqual(order.Total, saved.Total);

            Assert.AreEqual(order.ShippingAddress.First, saved.ShippingAddress.First);
            Assert.AreEqual(order.ShippingAddress.Last, saved.ShippingAddress.Last);
            Assert.AreEqual(order.ShippingAddress.Addr1, saved.ShippingAddress.Addr1);
            Assert.AreEqual(order.ShippingAddress.Addr2, saved.ShippingAddress.Addr2);
            Assert.AreEqual(order.ShippingAddress.City, saved.ShippingAddress.City);
            Assert.AreEqual(order.ShippingAddress.State, saved.ShippingAddress.State);
            Assert.AreEqual(order.ShippingAddress.Postal, saved.ShippingAddress.Postal);

            Assert.AreEqual(order.BillingAddress.First, saved.BillingAddress.First);
            Assert.AreEqual(order.BillingAddress.Last, saved.BillingAddress.Last);
            Assert.AreEqual(order.BillingAddress.Addr1, saved.BillingAddress.Addr1);
            Assert.AreEqual(order.BillingAddress.Addr2, saved.BillingAddress.Addr2);
            Assert.AreEqual(order.BillingAddress.City, saved.BillingAddress.City);
            Assert.AreEqual(order.BillingAddress.State, saved.BillingAddress.State);
            Assert.AreEqual(order.BillingAddress.Postal, saved.BillingAddress.Postal);

            Assert.AreEqual(order.AuthorizationCode, saved.AuthorizationCode);
            Assert.AreEqual(order.Status, saved.Status);
            Assert.AreEqual(order.SubTotal, saved.SubTotal);
            Assert.AreEqual(order.TaxAmount, saved.TaxAmount);
            Assert.AreEqual(order.Total, saved.Total);

            Assert.AreEqual(order.OrderLines.Length, saved.OrderLines.Length);
            Assert.AreEqual(order.OrderLines[0].ProductId, saved.OrderLines[0].ProductId);
            Assert.AreEqual(order.OrderLines[0].ProductName, saved.OrderLines[0].ProductName);
            Assert.AreEqual(order.OrderLines[0].UnitPrice, saved.OrderLines[0].UnitPrice);
            Assert.AreEqual(order.OrderLines[0].ExtendedPrice, saved.OrderLines[0].ExtendedPrice);
            Assert.AreEqual(order.OrderLines[0].Quantity, saved.OrderLines[0].Quantity);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException))]
        public void OrderAccessor_Submit_Error_NoOrderLines()
        {
            var order = new Order()
            {
                BillingAddress = BillingAddress,
                ShippingAddress = ShippingAddress,
                OrderLines = null,
                Total = 10.0M
            };

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderAccessor_Submit_Error_NoOrder()
        {
            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, null);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException))]
        public void OrderAccessor_Submit_Error_NoBilling()
        {
            var order = new Order()
            {
                ShippingAddress = ShippingAddress,
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

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_Submit_Error_NoShipping()
        {
            var order = new Order()
            {
                BillingAddress = BillingAddress,
                ShippingAddress = null,
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

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);

            Assert.IsNotNull(saved);
            Assert.IsTrue(saved.Id > 0);
            Assert.AreEqual(10.0M, saved.Total);

            Assert.IsNotNull(saved.ShippingAddress);

            Assert.AreEqual(null, saved.ShippingAddress.First);
            Assert.AreEqual(null, saved.ShippingAddress.Last);
            Assert.AreEqual(null, saved.ShippingAddress.Addr1);
            Assert.AreEqual(null, saved.ShippingAddress.Addr2);
            Assert.AreEqual(null, saved.ShippingAddress.City);
            Assert.AreEqual(null, saved.ShippingAddress.State);
            Assert.AreEqual(null, saved.ShippingAddress.Postal);

            Assert.AreEqual(order.BillingAddress.First, saved.BillingAddress.First);
            Assert.AreEqual(order.BillingAddress.Last, saved.BillingAddress.Last);
            Assert.AreEqual(order.BillingAddress.Addr1, saved.BillingAddress.Addr1);
            Assert.AreEqual(order.BillingAddress.Addr2, saved.BillingAddress.Addr2);
            Assert.AreEqual(order.BillingAddress.City, saved.BillingAddress.City);
            Assert.AreEqual(order.BillingAddress.State, saved.BillingAddress.State);
            Assert.AreEqual(order.BillingAddress.Postal, saved.BillingAddress.Postal);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_Find()
        {
            var order = new Order()
            {
                BillingAddress = BillingAddress,
                ShippingAddress = ShippingAddress,
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
                Total = 10.0M,
            };

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);

            Assert.IsNotNull(saved);
            Assert.IsTrue(saved.Id > 0);

            var loaded = accessor.FindOrder(saved.Id);

            Assert.AreEqual(order.ShippingAddress.First, loaded.ShippingAddress.First);
            Assert.AreEqual(order.ShippingAddress.Last, loaded.ShippingAddress.Last);
            Assert.AreEqual(order.ShippingAddress.Addr1, loaded.ShippingAddress.Addr1);
            Assert.AreEqual(order.ShippingAddress.Addr2, loaded.ShippingAddress.Addr2);
            Assert.AreEqual(order.ShippingAddress.City, loaded.ShippingAddress.City);
            Assert.AreEqual(order.ShippingAddress.State, loaded.ShippingAddress.State);
            Assert.AreEqual(order.ShippingAddress.Postal, loaded.ShippingAddress.Postal);

            Assert.AreEqual(order.BillingAddress.First, loaded.BillingAddress.First);
            Assert.AreEqual(order.BillingAddress.Last, loaded.BillingAddress.Last);
            Assert.AreEqual(order.BillingAddress.Addr1, loaded.BillingAddress.Addr1);
            Assert.AreEqual(order.BillingAddress.Addr2, loaded.BillingAddress.Addr2);
            Assert.AreEqual(order.BillingAddress.City, loaded.BillingAddress.City);
            Assert.AreEqual(order.BillingAddress.State, loaded.BillingAddress.State);
            Assert.AreEqual(order.BillingAddress.Postal, loaded.BillingAddress.Postal);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_Find_Fails()
        {
            var accessor = CreateOrderAccessor();
            Assert.IsNull(accessor.FindOrder(0));
            Assert.IsNull(accessor.FindOrder(-1));
            Assert.IsNull(accessor.FindOrder(999999));
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_SellerSalesTotal()
        {
            var order = new Order()
            {
                BillingAddress = BillingAddress,
                ShippingAddress = ShippingAddress,
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

            var accessor = CreateOrderAccessor();

            var before = accessor.SalesTotal();
            if (before == null)
            {
                before = new SellerSalesTotal();
            }

            var saved = accessor.SaveOrder(2, order);

            Assert.IsNotNull(saved);
            
            var after = accessor.SalesTotal();
            Assert.IsNotNull(after);

            Assert.AreEqual(before.OrderCount + 1, after.OrderCount);
            Assert.AreEqual(before.OrderTotal + 10.0M, after.OrderTotal);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_SellerSalesTotal_NoResults()
        {
            var accessor = CreateOrderAccessor(3); // use seller id 3 to ensure no orders

            var results = accessor.SalesTotal();
            
            Assert.IsNull(results);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_UnfulfilledOrders()
        {
            var order = CreateUnfulfilledOrderObject();

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);
            int order1Id = saved.Id;

            saved = accessor.SaveOrder(2, order);
            int order2Id = saved.Id;

            //Save one order with a different status
            order.Status = OrderStatuses.Failed;
            saved = accessor.SaveOrder(2, order);
            int order3Id = saved.Id;

            var unfulfilledOrders = accessor.UnfulfilledOrders();
            int goodOrders = 0;
            int badOrders = 0;

            Assert.IsTrue(unfulfilledOrders.Length >= 2);

            // make sure that only the good orders we added are in the list
            foreach (var unfulfilledOrder in unfulfilledOrders)
            {
                Assert.AreEqual(OrderStatuses.Authorized, unfulfilledOrder.Status);
                if (unfulfilledOrder.Id == order1Id || unfulfilledOrder.Id == order2Id)
                {
                    goodOrders += 1;
                }
                else if (unfulfilledOrder.Id == order3Id)
                {
                    badOrders += 1;
                }
            }
            Assert.AreEqual(2, goodOrders);
            Assert.AreEqual(0, badOrders);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_UpdateOrderStatus()
        {
            var order = CreateUnfulfilledOrderObject();

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);

            var saved2 = accessor.UpdateOrderStatus(saved.Id, OrderStatuses.Captured, "Order capture notes");

            Assert.AreEqual(saved.Id, saved2.Id);
            Assert.AreEqual(OrderStatuses.Captured, saved2.Status);
            Assert.AreEqual("Order capture notes", saved2.Notes);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Cannot pass new order")]
        public void OrderAccessor_UpdateOrderStatusInvalidOrder()
        {
            var order = CreateUnfulfilledOrderObject();

            var accessor = CreateOrderAccessor();

            // set the order id to 0 to cause exception
            accessor.UpdateOrderStatus(0, OrderStatuses.Captured, "");
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Order not found")]
        public void OrderAccessor_UpdateOrderStatusNullOrder()
        {
            var accessor = CreateOrderAccessor();

            accessor.UpdateOrderStatus(999999999, OrderStatuses.Captured, "");
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void OrderAccessor_FulfillOrder()
        {
            var order = CreateUnfulfilledOrderObject();

            var accessor = CreateOrderAccessor();
            var saved = accessor.SaveOrder(2, order);

            var saved2 = accessor.FulfillOrder(saved.Id, "UPS", "my tracking code", "");

            Assert.AreEqual(saved.Id, saved2.Id);
            Assert.AreEqual(OrderStatuses.Shipped, saved2.Status);
            Assert.AreEqual("UPS", saved2.ShippingProvider);
            Assert.AreEqual("my tracking code", saved2.TrackingCode);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Cannot pass new order")]
        public void OrderAccessor_FulfillOrderInvalidOrder()
        {
            var order = CreateUnfulfilledOrderObject();

            var accessor = CreateOrderAccessor();

            // set the order id to 0 to cause exception
            accessor.FulfillOrder(0, "", "", "");
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Order not found")]
        public void OrderAccessor_FulfillOrderNullOrder()
        {
            var accessor = CreateOrderAccessor();

            accessor.FulfillOrder(999999999, "", "", "");
        }

    }
}
