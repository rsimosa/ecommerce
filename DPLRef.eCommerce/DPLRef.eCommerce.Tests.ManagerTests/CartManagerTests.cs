using System;
using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Utilities;
using DPLRef.eCommerce.Tests.Mocks;

namespace DPLRef.eCommerce.Tests.ManagerTests
{
    [TestClass]
    public class CartManagerTests : ManagerTestBase
    {
        #region Test Data
  
        private static readonly Guid _myBadSessionId = new Guid("11111111-bbbb-cccc-dddd-eeeeeeeeeeee");
        private static readonly Guid _myBothInfoSessionId = new Guid("ffffffff-bbbb-cccc-dddd-eeeeeeeeeeee");
        private static readonly Address _myAddress = new Address()
        {
            Addr1 = "My Address 1",
            Addr2 = "My Address 2",
            City = "My City",
            First = "My First",
            Last = "My Last",
            Postal = "My Postal",
            State = "My State"
        };
        private static readonly Address _mySameAddress = new Address()
        {
            Addr1 = "My Same Address 1",
            Addr2 = "My Same Address 2",
            City = "My Same City",
            First = "My Same First",
            Last = "My Same Last",
            Postal = "My Same Postal",
            State = "My Same State"
        };
        private static readonly Address _myBadAddress = new Address();

        private const int CatalogId = 1;

        private const int SellerId = 1;

        MockData mockData = new MockData()
        {
            Context = new AmbientContext()
            {
                AuthToken = "",
                SessionId = MockData.MySessionId
            }
        };       


        private readonly WebStoreCart _billingInfoCart = new WebStoreCart()
        {
            Id = MockData.MySessionId,
            BillingAddress = _myAddress,
            ShippingAddress = null,
            SubTotal = 0.0m,
            TaxAmount = 0.0m,
            Total = 0.0m,
            CartItems = new WebStoreCartItem[0]
        };

        private readonly WebStoreCart _shippingInfoCart = new WebStoreCart()
        {
            Id = MockData.MySessionId,
            BillingAddress = null,
            ShippingAddress = _myAddress,
            SubTotal = 0.0m,
            TaxAmount = 0.0m,
            Total = 0.0m,
            CartItems = new WebStoreCartItem[0]
        };

        private readonly WebStoreCart _bothInfoCart = new WebStoreCart()
        {
            Id = MockData.MySessionId,
            BillingAddress = _mySameAddress,
            ShippingAddress = _mySameAddress,
            SubTotal = 0.0m,
            TaxAmount = 0.0m,
            Total = 0.0m,
            CartItems = new WebStoreCartItem[0]
        };

        #endregion

        #region Factories

        private AccessorFactory SetupMockAccessorFactory()
        {
            AccessorFactory accFactory = new AccessorFactory(this.mockData.Context, new UtilityFactory(new AmbientContext()));

            var mockCartAccessor = new MockCartAccessor(mockData);
            accFactory.AddOverride<ICartAccessor>(mockCartAccessor);

            var mockCatalogAccessor = new MockCatalogAccessor(mockData);
            accFactory.AddOverride<Accessors.Catalog.ICatalogAccessor>(mockCatalogAccessor);

            return accFactory;
        }

        private EngineFactory SetupMockEngineFactory()
        {          
            EngineFactory engFactory = new EngineFactory(this.mockData.Context, SetupMockAccessorFactory(), null);
            return engFactory;
        }

        private IWebStoreCartManager GetWebStoreManager(Guid sessionId)
        {
            mockData.Context.SessionId = sessionId;
            ManagerFactory mgrFactory = new ManagerFactory(mockData.Context);
            return mgrFactory.CreateManager<IWebStoreCartManager>(SetupMockEngineFactory(), SetupMockAccessorFactory(), null);
        }

        #endregion

        #region IWebStoreCartManager


        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_SaveCartItem()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.SaveCartItem(CatalogId, 1, 1);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(1, response.Cart.CartItems.Length);
            Assert.AreEqual(1, response.Cart.CartItems[0].ProductId);
            Assert.AreEqual("My Product", response.Cart.CartItems[0].ProductName);
            Assert.AreEqual(1, response.Cart.CartItems[0].Quantity);
            Assert.AreEqual(1.50m, response.Cart.CartItems[0].UnitPrice);
            Assert.AreEqual(1.50m, response.Cart.CartItems[0].ExtendedPrice);
            Assert.AreEqual(0.10m, response.Cart.TaxAmount);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_SaveCartItemInvalidArgument()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.SaveCartItem(CatalogId, 1, -1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem editting the cart", response.Message);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_ShowCart()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            // add an item to the cart
            var saveResponse = mgr.SaveCartItem(CatalogId, 1, 1);

            // retrieve the cart
            mgr = GetWebStoreManager(saveResponse.Cart.Id);
            var showResponse = mgr.ShowCart(CatalogId);

            // compare JSON representations to ensure they are equal
            Assert.AreEqual(StringUtilities.DataContractToJson(saveResponse),
                StringUtilities.DataContractToJson(showResponse));
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_ShowCartNotFound()
        {
            // retrieve the non-existent cart
            var mgr = GetWebStoreManager(Guid.NewGuid());
            var response = mgr.ShowCart(CatalogId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Cart not found", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_ShowCartNoId()
        {
            // retrieve the non-existent cart
            var mgr = GetWebStoreManager(Guid.Empty);
            var response = mgr.ShowCart(CatalogId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Cart not found", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_ShowCartError()
        {
            // retrieve the cart that generates an exception
            var mgr = GetWebStoreManager(MockData.MyBadSessionId);
            var response = mgr.ShowCart(CatalogId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem accessing the cart", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_RemoveCartItem()
        {
            // NOTE: we only need to test the happy path removal of a cart item since the method simply calls SaveCartItem()
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.SaveCartItem(CatalogId, 1, 1);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(1, response.Cart.CartItems.Length);

            response = mgr.RemoveCartItem(CatalogId, 1);
            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(0, response.Cart.CartItems.Length);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_UpdateBillingInfo()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.UpdateBillingInfo(CatalogId, _myAddress, false);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);

            // compare JSON representations to ensure they are equal
            Assert.AreEqual(StringUtilities.DataContractToJson(_billingInfoCart),
                StringUtilities.DataContractToJson(response.Cart));
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_UpdateBillingInfoError()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.UpdateBillingInfo(CatalogId, MockData.MyBadAddress, false);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem saving the billing info", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_UpdateShippingInfoError()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.UpdateShippingInfo(CatalogId, MockData.MyBadAddress, false);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem saving the shipping info", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_UpdateBillingInfoBoth()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            mgr.UpdateShippingInfo(CatalogId, _mySameAddress, true);
            var response = mgr.UpdateBillingInfo(CatalogId, _mySameAddress, true);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);

            // compare JSON representations to ensure they are equal
            Assert.AreEqual(StringUtilities.DataContractToJson(_bothInfoCart),
                StringUtilities.DataContractToJson(response.Cart));
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_UpdateShippingInfo()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            var response = mgr.UpdateShippingInfo(CatalogId, _myAddress, false);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);

            // compare JSON representations to ensure they are equal
            Assert.AreEqual(StringUtilities.DataContractToJson(_shippingInfoCart),
                StringUtilities.DataContractToJson(response.Cart));
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CartManager_UpdateShippingInfoBoth()
        {
            var mgr = GetWebStoreManager(MockData.MySessionId);

            mgr.UpdateBillingInfo(CatalogId, _mySameAddress, false);
            var response = mgr.UpdateShippingInfo(CatalogId, _mySameAddress, true);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);

            // compare JSON representations to ensure they are equal
            Assert.AreEqual(StringUtilities.DataContractToJson(_bothInfoCart),
                StringUtilities.DataContractToJson(response.Cart));
        }

        #endregion
    }
}
