using System;
using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Tests.Mocks;
using System.Linq;

namespace DPLRef.eCommerce.Tests.ManagerTests
{
    [TestClass]
    public class FulfillmentManagerTests : ManagerTestBase
    {
        #region Test Data

        private const int _goodOrderId = 123;
        private const int _badOrderId = -1;
        private const int _badStatusOrderId = -2;
        private const string _validToken = "my valid token";
        private const string _invalidToken = "Invalid";
        private const string _shippingProvider = "my shipping provider";
        private const string _trackingCode = "my tracking code";

        private MockData mockData = new MockData()
        {
            Context = new AmbientContext()
            {

            }
        };

        private static readonly Address _validAddress = new Address()
        {
            Addr1 = "Address 1",
            City = "City",
            First = "First",
            Last = "Last",
            EmailAddress = "email@address.com",
            Postal = "Postal",
            State = "State"
        };

        private Order _orderToFulfill = new Order()
        {
            Id = _goodOrderId,
            TaxAmount = 0.11m,
            SubTotal = 1.50m,
            Total = 1.61m,
            Status = OrderStatuses.Authorized,
            BillingAddress = _validAddress,
            ShippingAddress = _validAddress,
            AuthorizationCode = "auth code",
            OrderLines = new OrderLine[]
            {
                new OrderLine()
                {
                    ProductId = 1,
                    ProductName = "Mock Product Name",
                    Quantity = 1,
                    UnitPrice = 1.50m,
                    ExtendedPrice = 1.50m
                }
            }
        };

        #endregion

        #region Factories

        private IAdminFulfillmentManager GetFulfillmentManager(string authToken)
        {
            this.mockData.Context = new AmbientContext()
            {
                AuthToken = authToken,
                SessionId = Guid.NewGuid(),
                SellerId = 1
            };
            ManagerFactory mgrFactory = new ManagerFactory(this.mockData.Context);
            return mgrFactory.CreateManager<IAdminFulfillmentManager>(null, SetupMockAccessorFactory(), SetupMockUtilityFactory());
        }

        private AccessorFactory SetupMockAccessorFactory()
        {
            // order accessor mock

            MockOrderAccessor mockOrderAccessor = new MockOrderAccessor(this.mockData);

            this.mockData.Orders.Clear();
            this.mockData.Orders.Add(
                _orderToFulfill);
            this.mockData.Orders.Add(new Order()
            {
                Id = _badStatusOrderId,
                TaxAmount = 0.11m,
                SubTotal = 1.50m,
                Total = 1.61m,
                Status = OrderStatuses.Failed, // invalid status for fulfillment
                BillingAddress = _validAddress,
                ShippingAddress = _validAddress,
                AuthorizationCode = "auth code",
                OrderLines = new OrderLine[]
                    {
                        new OrderLine()
                        {
                            ProductId = 1,
                            ProductName = "Mock Product Name",
                            Quantity = 1,
                            UnitPrice = 1.50m,
                            ExtendedPrice = 1.50m
                        }
                    }
            });

            // payment accessor mock
            MockPaymentAccessor mockPaymentAccessor = new MockPaymentAccessor(this.mockData);

            // shipping accessor mock
            MockShippingAccessor mockShippingAccessor = new MockShippingAccessor(this.mockData);

            AccessorFactory accFactory = new AccessorFactory(this.mockData.Context, new UtilityFactory(this.mockData.Context));
            accFactory.AddOverride<IOrderAccessor>(mockOrderAccessor);
            accFactory.AddOverride<IPaymentAccessor>(mockPaymentAccessor);
            accFactory.AddOverride<IShippingAccessor>(mockShippingAccessor);

            return accFactory;
        }

        private UtilityFactory SetupMockUtilityFactory()
        {
            var mockAsyncUtility = new MockAsyncUtility(mockData);
            UtilityFactory utilFactory = new UtilityFactory(this.mockData.Context);
            utilFactory.AddOverride<IAsyncUtility>(mockAsyncUtility);
            return utilFactory;
        }

        #endregion

        #region Fulfillment Manager

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_GetOrdersToFulfill()
        {
            var mgr = GetFulfillmentManager(_validToken);

            var response = mgr.GetOrdersToFulfill();

            Assert.AreEqual(1, response.Orders.Length);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_GetOrdersToFulfill_NoAuth()
        {
            var mgr = GetFulfillmentManager(_invalidToken);

            var response = mgr.GetOrdersToFulfill();

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Seller not authenticated", response.Message);
            Assert.IsNull(response.Orders);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrder()
        {
            var mgr = GetFulfillmentManager(_validToken);
            
            this.mockData.ShippingResult = new ShippingResult()
            {
                Success = true,
                ShippingProvider = _shippingProvider,
                TrackingCode = _trackingCode
            };

            var response = mgr.FulfillOrder(_goodOrderId);

            Assert.IsTrue(response.Success);
            // verify progress flags set
            Assert.IsTrue(this.mockData.OrderCaptureAttempted);
            Assert.IsTrue(this.mockData.OrderCapturedStatus);
            Assert.IsTrue(this.mockData.OrderShippingRequested);
            Assert.IsTrue(this.mockData.OrderFulfilled);
            // verify the final status of the order
            var order = this.mockData.Orders.First(o => o.Id == _goodOrderId);
            Assert.AreEqual(OrderStatuses.Shipped, order.Status);
            Assert.AreEqual(_shippingProvider, order.ShippingProvider);
            Assert.AreEqual(_trackingCode, order.TrackingCode);
            //make sure send event called
            Assert.IsTrue(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrderNoAuth()
        {
            var mgr = GetFulfillmentManager(_invalidToken);
            var response = mgr.FulfillOrder(_goodOrderId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Seller not authenticated", response.Message);
            // verify progress flags not set
            Assert.IsFalse(this.mockData.OrderCaptureAttempted);
            Assert.IsFalse(this.mockData.OrderCapturedStatus);
            Assert.IsFalse(this.mockData.OrderShippingRequested);
            Assert.IsFalse(this.mockData.OrderFulfilled);
            //make sure send event called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrderBadOrderId()
        {
            var mgr = GetFulfillmentManager(_validToken);
            var response = mgr.FulfillOrder(_badOrderId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid order for fulfillment", response.Message);
            // verify progress flags not set
            Assert.IsFalse(this.mockData.OrderCaptureAttempted);
            Assert.IsFalse(this.mockData.OrderCapturedStatus);
            Assert.IsFalse(this.mockData.OrderShippingRequested);
            Assert.IsFalse(this.mockData.OrderFulfilled);
            //make sure send event called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrderBadOrderStatus()
        {
            var mgr = GetFulfillmentManager(_validToken);
            var response = mgr.FulfillOrder(_badStatusOrderId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid order for fulfillment", response.Message);
            // verify progress flags not set
            Assert.IsFalse(this.mockData.OrderCaptureAttempted);
            Assert.IsFalse(this.mockData.OrderCapturedStatus);
            Assert.IsFalse(this.mockData.OrderShippingRequested);
            Assert.IsFalse(this.mockData.OrderFulfilled);
            //make sure send event called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrderCaptureFail()
        {
            var mgr = GetFulfillmentManager(_validToken);
            mockData.ForceCaptureFail = true;
            var response = mgr.FulfillOrder(_goodOrderId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Capture failed for authorization", response.Message);
            // verify progress flags set
            Assert.IsTrue(this.mockData.OrderCaptureAttempted);
            // verify progress flags not set
            Assert.IsFalse(this.mockData.OrderCapturedStatus);
            Assert.IsFalse(this.mockData.OrderShippingRequested);
            Assert.IsFalse(this.mockData.OrderFulfilled);
            // verify the final status of the order
            Assert.AreEqual(OrderStatuses.Authorized, _orderToFulfill.Status);
            Assert.AreEqual("Capture failed for authorization", _orderToFulfill.Notes);
            Assert.IsTrue(string.IsNullOrEmpty(_orderToFulfill.ShippingProvider));
            Assert.IsTrue(string.IsNullOrEmpty(_orderToFulfill.TrackingCode));
            //make sure send event called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrderShippingFail()
        {
            var mgr = GetFulfillmentManager(_validToken);
            
            mockData.ForceShippingFail = true;
            var response = mgr.FulfillOrder(_goodOrderId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Unable to request shipping for order", response.Message);
            // verify progress flags set
            Assert.IsTrue(this.mockData.OrderCaptureAttempted);
            Assert.IsTrue(this.mockData.OrderCapturedStatus);
            Assert.IsTrue(this.mockData.OrderShippingRequested);
            // verify progress flags not set
            Assert.IsFalse(this.mockData.OrderFulfilled);
            // verify the final status of the order
            Assert.AreEqual(OrderStatuses.Captured, _orderToFulfill.Status);
            Assert.AreEqual("Unable to request shipping for order", _orderToFulfill.Notes);
            Assert.IsTrue(string.IsNullOrEmpty(_orderToFulfill.ShippingProvider));
            Assert.IsTrue(string.IsNullOrEmpty(_orderToFulfill.TrackingCode));
            //make sure send event called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void FulfillmentManager_FulfillOrderException()
        {
            var mgr = GetFulfillmentManager(_validToken);
            
            mockData.ForceException = true;
            var response = mgr.FulfillOrder(_goodOrderId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem fulfilling the order", response.Message);
            // verify progress flags not set
            Assert.IsFalse(this.mockData.OrderCaptureAttempted);
            Assert.IsFalse(this.mockData.OrderCapturedStatus);
            Assert.IsFalse(this.mockData.OrderShippingRequested);
            Assert.IsFalse(this.mockData.OrderFulfilled);
            //make sure send event called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        #endregion
    }
}
