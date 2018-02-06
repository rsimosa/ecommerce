using System;
using DPLRef.eCommerce.Accessors;
using CatAcc = DPLRef.eCommerce.Accessors.Catalog;
using SalesAcc = DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Tests.Mocks;
using System.Linq;


namespace DPLRef.eCommerce.Tests.ManagerTests
{
    [TestClass]
    public class OrderManagerTests : ManagerTestBase
    {
        // NOTE: This is a hybrid unit test in that the manager under test is not completely tested in isolation.
        // While the most accessors and utilities are mocked here, the test does use the actual engine dependencies
        // and the actual payment accessor that are in the manager as opposed to mocking them out as well. This is a judgment
        // call based upon how the engines/accessors are constructed and tested on their own and whether it is
        // more work to mock them out than to just use them as-is.

        // TODO: combine all of the common helper classes, methods and objects for Order/Cart manager into a base class

        // TODO: Tests:
        // Re-submit order after payment auth failure               

        #region Test Data
        
        private readonly PaymentInstrument _goodPayment = new PaymentInstrument()
        {
            AccountNumber = "4000111122223333",
            VerificationCode = 123,
            ExpirationDate = "10/20",
            PaymentType = PaymentTypes.CreditCard
        };

        private readonly PaymentInstrument _badPayment = new PaymentInstrument()
        {
            AccountNumber = "5000111122223333",
            VerificationCode = 123,
            ExpirationDate = "10/20",
            PaymentType = PaymentTypes.CreditCard
        };

        MockData mockData;

        #endregion

        #region Factory

        private AccessorFactory SetupMockAccessorFactory()
        {
            var factory = new AccessorFactory(mockData.Context, SetupMockUtilityFactory());
            
            // cart accessor mock
            var mockCartAccessor = new MockCartAccessor(mockData);
            factory.AddOverride<SalesAcc.ICartAccessor>(mockCartAccessor);

            // catalog accessor 
            var mockCatalogAccessor = new MockCatalogAccessor(mockData);
            factory.AddOverride<CatAcc.ICatalogAccessor>(mockCatalogAccessor);

            // order accessor
            var mockOrderAccessor = new MockOrderAccessor(mockData);
            factory.AddOverride<SalesAcc.IOrderAccessor>(mockOrderAccessor);

            // payment accessor
            var mockPaymentAccessor = new MockPaymentAccessor(mockData);
            factory.AddOverride<SalesAcc.IPaymentAccessor>(mockPaymentAccessor);
            return factory;
        }

        private UtilityFactory SetupMockUtilityFactory()
        {
            var mockAsyncUtility = new MockAsyncUtility(this.mockData);

            UtilityFactory utilFactory = new UtilityFactory(new AmbientContext());
            utilFactory.AddOverride<IAsyncUtility>(mockAsyncUtility);

            return utilFactory;
        }

        private const int CatalogId = 1;

        private IWebStoreOrderManager GetWebStoreManager(Guid sessionId)
        {

            mockData = new MockData();
            mockData.Context = new AmbientContext()
            {
                AuthToken = "",
                SessionId = sessionId
            };

            ManagerFactory mgrFactory = new ManagerFactory(mockData.Context);
            return mgrFactory.CreateManager<IWebStoreOrderManager>(null, SetupMockAccessorFactory(), SetupMockUtilityFactory());
        }

        #endregion

        #region WebStoreManager

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void OrderManager_SubmitOrder()
        {
            var mgr = GetWebStoreManager(MockData.MySessionIdForOrder);    

            var result = mgr.SubmitOrder(CatalogId, _goodPayment);

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, result.Order.Id);
            Assert.AreEqual(1.60m, result.Order.Total);
            Assert.IsTrue(mockData.OrderCreated, "Order not created");
            Assert.IsTrue(mockData.OrderSucceeded, "Order unsuccessful");
            Assert.IsTrue(mockData.CartDeleted, "Cart not deleted");
            Assert.IsTrue(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void OrderManager_SubmitOrderNoCart()
        {
            var mgr = GetWebStoreManager(Guid.NewGuid());

            var result = mgr.SubmitOrder(CatalogId, _goodPayment);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Cart is not valid", result.Message);
            Assert.IsNull(result.Order);
            // verify that the progress flags were not set
            Assert.IsFalse(mockData.OrderCreated);
            Assert.IsFalse(mockData.OrderSucceeded);
            Assert.IsFalse(mockData.CartDeleted);
            // verify that the SendEvent method was NOT called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void OrderManager_SubmitOrderBadPayment()
        {
            var mgr = GetWebStoreManager(MockData.MySessionIdForOrder);
            
            var result = mgr.SubmitOrder(CatalogId, _badPayment);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("There was a problem processing the payment", result.Message);
            Assert.IsNotNull(result.Order);

            Assert.AreEqual(1, result.Order.Id);
            Assert.AreEqual(1.60m, result.Order.Total);
            Assert.IsTrue(mockData.OrderCreated, "Order not created");
            Assert.IsTrue(string.IsNullOrEmpty(result.Order.AuthorizationCode));
            // verify that the progress flags were not set
            Assert.IsFalse(mockData.OrderSucceeded);
            Assert.IsFalse(mockData.CartDeleted);
            // verify that the SendEvent method was NOT called
            Assert.IsFalse(mockData.AsyncCalled);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void OrderManager_SubmitOrderInvalidOrder()
        {
            var mgr = GetWebStoreManager(MockData.MySessionIdForOrder);

            // remove the billing address from the shopping cart    
            mockData.Carts.First(c => c.Id == MockData.MySessionIdForOrder).BillingAddress = null;

            var result = mgr.SubmitOrder(CatalogId, _goodPayment);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("BillingAddress address is not valid", result.Message);
            Assert.IsNull(result.Order);
            // verify that the progress flags were not set
            Assert.IsFalse(mockData.OrderCreated);
            Assert.IsFalse(mockData.OrderSucceeded);
            Assert.IsFalse(mockData.CartDeleted);
            // verify that the SendEvent method was NOT called
            Assert.IsFalse(mockData.AsyncCalled);

        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void OrderManager_SubmitOrderException()
        {
            var mgr = GetWebStoreManager(MockData.MyBadSessionId);
                        
            var result = mgr.SubmitOrder(CatalogId, _goodPayment);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("There was a problem processing the order", result.Message);
            Assert.IsNull(result.Order);
            // verify that the progress flags were not set
            Assert.IsFalse(mockData.OrderCreated);
            Assert.IsFalse(mockData.OrderSucceeded);
            Assert.IsFalse(mockData.CartDeleted);
            // verify that the SendEvent method was NOT called
            Assert.IsFalse(mockData.AsyncCalled);

        }

        #endregion
    }
}
