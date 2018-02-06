using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Remittance;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.ServiceHost.Notifications;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Tests.Mocks;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DPLRef.eCommerce.Tests.ManagerTests
{
    [TestClass]
    public class NotificationManagerTests : ManagerTestBase
    {        
        #region Test Data

        private const int _testOrderId = 123;
        private const int _testSellerId = 1;
        private const string _testOrderEmailAddress = "bob.smith@dontpaniclabs.com";

        private MockData mockData = new MockData()
        {
            Context = new AmbientContext()
        };

        #endregion

        #region Mocks

        private AccessorFactory SetupMockAccessorFactory()
        {
            AccessorFactory accFactory = new AccessorFactory(mockData.Context, new UtilityFactory(new AmbientContext()));

            // Order Accessor Mock
            var mockOrderAccessor = new MockOrderAccessor(mockData);
            accFactory.AddOverride<IOrderAccessor>(mockOrderAccessor);

            // Seller Accessor Mock
            var mockSellerAccessor = new MockSellerAccessor(mockData);
            
            accFactory.AddOverride<IOrderAccessor>(mockOrderAccessor);
            accFactory.AddOverride<ISellerAccessor>(mockSellerAccessor);

            return accFactory;
        }

        private EngineFactory SetupMockEngineFactory()
        {
            EngineFactory engFactory = new EngineFactory(mockData.Context, SetupMockAccessorFactory(), null);
            return engFactory;
        }

        #endregion

        #region Helpers

        private INotificationManager GetNotificationManager()
        {
            mockData.Orders.Add(
            new Order()
            {
                Id = _testOrderId,
                SellerId = _testSellerId,
                BillingAddress = new Address()
                {
                    EmailAddress = _testOrderEmailAddress
                },
                OrderLines = new OrderLine[] { },
            });

            ManagerFactory mgrFactory = new ManagerFactory(mockData.Context);
            return mgrFactory.CreateManager<INotificationManager>(SetupMockEngineFactory(), SetupMockAccessorFactory(),
                null);
        }

        #endregion

        [TestMethod]
        [TestCategory("Managers-ServiceHost")]
        public void NotificationManager_SendNewOrderNotices()
        {
            var mgr = GetNotificationManager();
            var response = mgr.SendNewOrderNotices(_testOrderId);

            Assert.IsTrue(response.Success);
            Assert.AreEqual($"New Order Notice email sent to {_testOrderEmailAddress} for order {_testOrderId}", response.Message);
        }

        [TestMethod]
        [TestCategory("Managers-ServiceHost")]
        public void NotificationManager_SendShippingNotices()
        {
            var mgr = GetNotificationManager();
            var response = mgr.SendOrderFulfillmentNotices(_testOrderId);

            Assert.IsTrue(response.Success);
            Assert.AreEqual($"Shipping Notice email sent to {_testOrderEmailAddress} for order {_testOrderId}", response.Message);
        }
    }
}
