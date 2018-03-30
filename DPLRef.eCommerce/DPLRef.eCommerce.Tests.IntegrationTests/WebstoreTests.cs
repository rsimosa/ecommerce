using System;
using DPLref.eCommerce.Tests.IntegrationTests.ExpectedResponses;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.WebStore.Catalog;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Tests.Mocks;

namespace DPLref.eCommerce.Tests.IntegrationTests
{
    [TestClass]
    public class WebstoreTests : IntegrationTestBase
    {
        #region Test Objects
        private static readonly Address _myAddress = new Address()
        {
            Addr1 = "1511 Snowbird Lane",
            Addr2 = "",
            City = "Lincoln",
            First = "Joan",
            Last = "Belmont",
            Postal = "68508",
            State = "Nebraska",
            EmailAddress = "joan@example.com"
        };
        private static readonly Address _mySameAddress = new Address()
        {
            Addr1 = "4928 Commerce Boulevard",
            Addr2 = "",
            City = "Lincoln",
            First = "Marvin",
            Last = "Waller",
            Postal = "68508",
            State = "Nebraska",
            EmailAddress = "joan@example.com"
        };
        #endregion

        #region Helpers

        MockData mockData = new MockData()
        {
            Context = new AmbientContext()
        };

        private T GetManager<T>(AmbientContext context) where T : class
        {
            // replace the AsyncUtility with a mock to avoid usage of queue IFX in tests
            var mockAsyncUtility = new MockAsyncUtility(this.mockData);

            UtilityFactory utilFactory = new UtilityFactory(new AmbientContext());
            utilFactory.AddOverride<IAsyncUtility>(mockAsyncUtility);

            var managerFactory = new ManagerFactory(context);
            return managerFactory.CreateManager<T>(null, null, utilFactory);
        }

        #endregion

        #region Catalog

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowCatalog()
        {
            var context = new AmbientContext() { SellerId = 1 };
            var webStoreCatalogManager = GetManager<IWebStoreCatalogManager>(context);
            var response = webStoreCatalogManager.ShowCatalog(1);

            Assert.AreEqual(1000, response.Catalog.Products.Length);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowCatalogNotFound()
        {
            var context = new AmbientContext() { SellerId = 1 };
            var webStoreCatalogManager = GetManager<IWebStoreCatalogManager>(context);
            var response = webStoreCatalogManager.ShowCatalog(-1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(WebstoreResponses.CatalogNotFoundResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowProduct()
        {
            var context = new AmbientContext() { SellerId = 1 };
            var webStoreCatalogManager = GetManager<IWebStoreCatalogManager>(context);
            var response = webStoreCatalogManager.ShowProduct(2, 1003);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(WebstoreResponses.ProductResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowProductNotFound()
        {
            var context = new AmbientContext() { SellerId = 1 };
            var webStoreCatalogManager = GetManager<IWebStoreCatalogManager>(context);
            var response = webStoreCatalogManager.ShowProduct(2, -1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(WebstoreResponses.ProductNotFoundResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowProductNotFound_Security()
        {
            var context = new AmbientContext() { SellerId = 2 };
            var webStoreCatalogManager = GetManager<IWebStoreCatalogManager>(context);
            var response = webStoreCatalogManager.ShowProduct(3, 1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(WebstoreResponses.ProductNotFoundResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        #endregion

        #region Cart

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SaveCartItem()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            webStoreCartManager.UpdateBillingInfo(1, _myAddress, false);
            webStoreCartManager.UpdateShippingInfo(1, _myAddress, false);
            var response = webStoreCartManager.SaveCartItem(1, 1, 2);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(1, response.Cart.CartItems.Length);
            Assert.AreEqual(1, response.Cart.CartItems[0].ProductId);
            Assert.AreEqual(3238.00m, response.Cart.CartItems[0].UnitPrice);
            Assert.AreEqual(2, response.Cart.CartItems[0].Quantity);
            Assert.AreEqual(6476.00m, response.Cart.CartItems[0].ExtendedPrice);
            Assert.AreEqual(6476.00m, response.Cart.SubTotal);
            Assert.AreEqual(453.32m, response.Cart.TaxAmount);
            Assert.AreEqual(6929.32m, response.Cart.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SaveCartItemZeroQty()
        {
            // setup the cart with an item in the cart
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.SaveCartItem(1, 1, 2);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);

            // remove the item from the cart by setting the qty to 0
            response = webStoreCartManager.SaveCartItem(1, 1, 0);

            Assert.AreEqual(0, response.Cart.CartItems.Length);
            Assert.AreEqual(00.00m, response.Cart.SubTotal);
            Assert.AreEqual(0.00m, response.Cart.TaxAmount);
            Assert.AreEqual(0.00m, response.Cart.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SaveCartItemInvalidCatalog()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() }; // Catalog Id 0 does not own Product Id 1
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.SaveCartItem(0, 1, 1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem editting the cart", response.Message);
            Assert.AreEqual(null, response.Cart);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SaveCartItemInvalidQuantity()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.SaveCartItem(1, 1, -1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem editting the cart", response.Message);
            Assert.AreEqual(null, response.Cart);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SaveCartItemInvalidProductId()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.SaveCartItem(1, -1, 1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem editting the cart", response.Message);
            Assert.AreEqual(null, response.Cart);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_RemoveCartItem()
        {
            // NOTE: keeping this test simple since calls SaveCartItem() with 0 qty which is covered by test above 
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.SaveCartItem(1, 1, 2);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(1, response.Cart.CartItems.Length);

            response = webStoreCartManager.RemoveCartItem(1, 1);
            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(0, response.Cart.CartItems.Length);

        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowCart()
        {
            // create a new cart
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var createdCart = webStoreCartManager.SaveCartItem(1, 1, 2);

            // retrieve the cart
            var response = webStoreCartManager.ShowCart(1);

            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.Cart);
            Assert.AreEqual(StringUtilities.DataContractToJson(createdCart), StringUtilities.DataContractToJson(response));
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowCartNotFound()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.ShowCart(1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Cart not found", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_ShowCartInvalidCatalog()
        {
            // create a new cart
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            webStoreCartManager.SaveCartItem(1, 1, 2);

            // change the catalog id and retrieve the cart
            webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var response = webStoreCartManager.ShowCart(2);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Cart not found", response.Message);
            Assert.IsNull(response.Cart);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]

        public void Webstore_UpdateBillingInfo()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            // update billing info only
            var response = webStoreCartManager.UpdateBillingInfo(1, _myAddress, false);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(context.SessionId, response.Cart.Id);
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_myAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.BillingAddress));
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(new Address()),
                StringUtilities.DataContractToJson<Address>(response.Cart.ShippingAddress));
            Assert.AreEqual(0, response.Cart.CartItems.Length);
            Assert.AreEqual(0.00m, response.Cart.SubTotal);
            Assert.AreEqual(0.00m, response.Cart.TaxAmount);
            Assert.AreEqual(0.00m, response.Cart.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]

        public void Webstore_UpdateBillingInfoBoth()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            // set shipping  info same as billing info
            var response = webStoreCartManager.UpdateBillingInfo(1, _mySameAddress, true);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(context.SessionId, response.Cart.Id);
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_mySameAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.BillingAddress));
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_mySameAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.ShippingAddress));
            Assert.AreEqual(0, response.Cart.CartItems.Length);
            Assert.AreEqual(0.00m, response.Cart.SubTotal);
            Assert.AreEqual(0.00m, response.Cart.TaxAmount);
            Assert.AreEqual(0.00m, response.Cart.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]

        public void Webstore_UpdateShippingInfo()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            // set shipping info only
            var response = webStoreCartManager.UpdateShippingInfo(1, _myAddress, false);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(context.SessionId, response.Cart.Id);
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(new Address()),
                StringUtilities.DataContractToJson<Address>(response.Cart.BillingAddress));
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_myAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.ShippingAddress));
            Assert.AreEqual(0, response.Cart.CartItems.Length);
            Assert.AreEqual(0.00m, response.Cart.SubTotal);
            Assert.AreEqual(0.00m, response.Cart.TaxAmount);
            Assert.AreEqual(0.00m, response.Cart.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]

        public void Webstore_UpdateShippingInfoBoth()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            // set billing info same as shipping info
            var response = webStoreCartManager.UpdateShippingInfo(1, _mySameAddress, true);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(context.SessionId, response.Cart.Id);
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_mySameAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.BillingAddress));
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_mySameAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.ShippingAddress));
            Assert.AreEqual(0, response.Cart.CartItems.Length);
            Assert.AreEqual(0.00m, response.Cart.SubTotal);
            Assert.AreEqual(0.00m, response.Cart.TaxAmount);
            Assert.AreEqual(0.00m, response.Cart.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]

        public void Webstore_UpdateBothInfo()
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            // update the billing and shipping info separately
            webStoreCartManager.UpdateBillingInfo(1, _myAddress, false);
            var response = webStoreCartManager.UpdateShippingInfo(1, _mySameAddress, false);

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Message);
            Assert.AreEqual(context.SessionId, response.Cart.Id);
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_myAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.BillingAddress));
            Assert.AreEqual(StringUtilities.DataContractToJson<Address>(_mySameAddress),
                StringUtilities.DataContractToJson<Address>(response.Cart.ShippingAddress));
            Assert.AreEqual(0, response.Cart.CartItems.Length);
            Assert.AreEqual(0.00m, response.Cart.SubTotal);
            Assert.AreEqual(0.00m, response.Cart.TaxAmount);
            Assert.AreEqual(0.00m, response.Cart.Total);
        }

        #endregion

        #region Orders

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SubmitOrder()
        {
            // create the cart
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var cartResponse = webStoreCartManager.SaveCartItem(1, 1, 2);
            Assert.IsTrue(cartResponse.Success);

            // update the cart shipping/billing info
            cartResponse = webStoreCartManager.UpdateShippingInfo(1, _myAddress, true);
            Assert.IsTrue(cartResponse.Success);

            // submit the order with a valid credit card
            var webStoreOrderManager = GetManager<IWebStoreOrderManager>(context);
            var payment = new PaymentInstrument()
            {
                AccountNumber = "4111111111111111",
                PaymentType = PaymentTypes.CreditCard,
                ExpirationDate = "01/2019"
            };

            var response = webStoreOrderManager.SubmitOrder(1, payment);

            // verify the order success
            Assert.IsTrue(response.Success);
            Assert.IsTrue(response.Order.Id > 0);
            Assert.AreEqual(OrderStatuses.Authorized, response.Order.Status);
            Assert.IsFalse(string.IsNullOrEmpty(response.Order.AuthorizationCode));
            Assert.AreEqual(1, response.Order.OrderLines.Length);
            Assert.AreEqual(6476.00m, response.Order.SubTotal);
            Assert.AreEqual(453.32m, response.Order.TaxAmount);
            Assert.AreEqual(6929.32m, response.Order.Total);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SubmitOrderBadCart()
        {
            // create the cart but don't put address information in
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var cartResponse = webStoreCartManager.SaveCartItem(1, 1, 2);
            Assert.IsTrue(cartResponse.Success);
            Assert.IsTrue(cartResponse.Success);

            // submit the order with a valid credit card
            var webStoreOrderManager = GetManager<IWebStoreOrderManager>(context);
            var payment = new PaymentInstrument()
            {
                AccountNumber = "4111111111111111",
                PaymentType = PaymentTypes.CreditCard,
                ExpirationDate = "01/2019"
            };

            var response = webStoreOrderManager.SubmitOrder(1, payment);

            // verify the order failed
            Assert.IsFalse(response.Success);
            Assert.AreEqual("ShippingAddress address is not valid", response.Message);
            Assert.IsNull(response.Order);
        }

        [TestMethod]
        [TestCategory("Integration-Webstore")]
        public void Webstore_SubmitOrderBadPaymentMethod()
        {
            // create the cart
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid() };
            var webStoreCartManager = GetManager<IWebStoreCartManager>(context);
            var cartResponse = webStoreCartManager.SaveCartItem(1, 1, 2);
            Assert.IsTrue(cartResponse.Success);

            // update the cart shipping/billing info
            cartResponse = webStoreCartManager.UpdateShippingInfo(1, _myAddress, true);
            Assert.IsTrue(cartResponse.Success);

            // submit the order with a invalid credit card
            var webStoreOrderManager = GetManager<IWebStoreOrderManager>(context);
            var payment = new PaymentInstrument()
            {
                AccountNumber = "5111111111111111",
                PaymentType = PaymentTypes.CreditCard,
                ExpirationDate = "01/2019"
            };

            var response = webStoreOrderManager.SubmitOrder(1, payment);

            // verify the order failed
            Assert.IsFalse(response.Success);
            Assert.AreEqual("There was a problem processing the payment", response.Message);
            Assert.AreEqual(OrderStatuses.Failed,response.Order.Status);
            Assert.IsTrue(string.IsNullOrEmpty(response.Order.AuthorizationCode));
        }

        #endregion
    }
}
