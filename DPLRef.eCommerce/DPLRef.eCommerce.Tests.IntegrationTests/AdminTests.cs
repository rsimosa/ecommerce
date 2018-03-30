using System;
using DPLref.eCommerce.Tests.IntegrationTests.ExpectedResponses;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Tests.Mocks;

namespace DPLref.eCommerce.Tests.IntegrationTests
{
    [TestClass]
    public class AdminTests : IntegrationTestBase
    {
        #region Test Objects
        private static readonly Address _myAddress = new Address()
        {
            Addr1 = "My Address 1",
            Addr2 = "My Address 2",
            City = "My City",
            First = "My First",
            Last = "My Last",
            EmailAddress = "My Email Address",
            Postal = "My Postal",
            State = "My State"
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

            UtilityFactory utilFactory = new UtilityFactory(context);
            utilFactory.AddOverride<IAsyncUtility>(mockAsyncUtility);

            var managerFactory = new ManagerFactory(context);
            return managerFactory.CreateManager<T>(null, null, utilFactory);
        }

        #endregion

        #region Catalog Operations

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_FindCatalogs()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.FindCatalogs();
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.FindCatalogsResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_FindCatalogsNoAuth()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.FindCatalogs();
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.FindCatalogsNoAuthResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowCatalog()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken"};
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowCatalog(1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowCatalogNotFound()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowCatalog(-1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogNotFoundResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowCatalogNoAuth()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowCatalog(1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogNoAuthResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveCatalogUpdate()
        {
            // Retrieve the catalog
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowCatalog(1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

            // change the description and update the catalog
            response.Catalog.Description = "UPDATED";
            var updateResponse = adminCatalogManager.SaveCatalog(response.Catalog);
            responseJson = StringUtilities.DataContractToJson(updateResponse);
            expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogUpdateResponse);

            // Compare the update response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveCatalogNew()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            // create a new catalog object
            var catalog = new WebStoreCatalog()
            {
                Name = "NEW_TEST_CATALOG",
                Description = "NEW_TEST_CATALOG description"
            };
            var newResponse = adminCatalogManager.SaveCatalog(catalog);
            Assert.IsTrue(newResponse.Success);
            Assert.IsNull(newResponse.Message);
            Assert.AreNotEqual(0, newResponse.Catalog.Id);
            Assert.AreEqual("NEW_TEST_CATALOG", newResponse.Catalog.Name);
            Assert.AreEqual("NEW_TEST_CATALOG description", newResponse.Catalog.Description);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveCatalogSellerIdMismatch()
        {
            // Retrieve the catalog
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowCatalog(1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

            // change the description and update the catalog
            response.Catalog.Description = "TEST_CATALOG updated description";
            // change the seller id in the context and recreate the manager
            managerFactory.Context.SellerId = 2;
            adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var updateResponse = adminCatalogManager.SaveCatalog(response.Catalog);
            responseJson = StringUtilities.DataContractToJson(updateResponse);
            expectedJson = StringUtilities.DataContractToJson(AdminResponses.CatalogSellerIdMismatchResponse);

            // Compare the update response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        #endregion

        #region Product Operations

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowProduct()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken"};
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = webStoreCatalogManager.ShowProduct(2, 1003);

            response.Product.Price = response.Product.Price + 0.00M;
            AdminResponses.ProductResponse.Product.Price = AdminResponses.ProductResponse.Product.Price + 0.00M;
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowProductNotFound()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken"};
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = webStoreCatalogManager.ShowProduct(1, -1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductNotFoundResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowProductNotFound_SecurityCatalog()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = webStoreCatalogManager.ShowProduct(2, 1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductNotFoundResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowProductNotFound_SecuritySeller()
        {
            var context = new AmbientContext() { SellerId = 2,AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = webStoreCatalogManager.ShowProduct(2, 1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductNotFoundResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_ShowProductNoAuth()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "" };
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = webStoreCatalogManager.ShowProduct(1, 1);
            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductNoAuthResponse);

            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveProductUpdate()
        {
            // Retrieve the product
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowProduct(2, 1003);

            // normalize decimal percision - SQL Server and Sqlite behave differently
            response.Product.Price = response.Product.Price + 0.00M;

            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

            // change the summary and update the product
            response.Product.Summary = "TEST_PRODUCT updated summary";
            var updateResponse = adminCatalogManager.SaveProduct(2, response.Product);
            responseJson = StringUtilities.DataContractToJson(updateResponse);
            expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductUpdatedResponse);

            // Compare the update response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveProductNew()
        {
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            // create a new product to save
            var product = new Product()
            {
                Id = 0,
                CatalogId = 2,
                SellerProductId = null,
                Name = "NEW_TEST_PRODUCT",
                Summary = "NEW_TEST_PRODUCT summary",
                Detail = "NEW_TEST_PRODUCT detail",
                Price = 9.9m,
                IsDownloadable = false, 
                IsAvailable = false, 
                SupplierName = "NEW_TEST_PRODUCT supplier name",
                ShippingWeight = 99.9m

            };
                
            var newResponse = adminCatalogManager.SaveProduct(2, product);
            Assert.IsTrue(newResponse.Success);
            Assert.IsNull(newResponse.Message);
            Assert.AreNotEqual(0, newResponse.Product.Id);
            Assert.AreEqual(2, newResponse.Product.CatalogId);
            Assert.AreEqual("NEW_TEST_PRODUCT", newResponse.Product.Name);
            Assert.AreEqual("NEW_TEST_PRODUCT summary", newResponse.Product.Summary);
            Assert.AreEqual("NEW_TEST_PRODUCT detail", newResponse.Product.Detail);
            Assert.AreEqual(9.9m, newResponse.Product.Price);
            Assert.AreEqual(99.9m, newResponse.Product.ShippingWeight);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveProductNoAuth()
        {
            // Retrieve the product
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowProduct(2, 1003);

            // normalize decimal percision - SQL Server and Sqlite behave differently
            response.Product.Price = response.Product.Price + 0.00M;

            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

            // change the summary and update the product
            response.Product.Summary = "TEST_PRODUCT updated summary";
            // remove the auth token and recreate the manager
            managerFactory.Context.AuthToken = "";
            adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var updateResponse = adminCatalogManager.SaveProduct(2, response.Product);

            // normalize decimal percision - SQL Server and Sqlite behave differently
            response.Product.Price = response.Product.Price + 0.00M;

            responseJson = StringUtilities.DataContractToJson(updateResponse);
            expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductNoAuthResponse);

            // Compare the update response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_SaveProductCatalogIdMismatch()
        {
            // Retrieve the product
            var context = new AmbientContext() { SellerId = 1, AuthToken = "MyToken" };
            var managerFactory = new ManagerFactory(context);
            var adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var response = adminCatalogManager.ShowProduct(2, 1003);

            // normalize decimal percision - SQL Server and Sqlite behave differently
            response.Product.Price = response.Product.Price + 0.00M;

            string responseJson = StringUtilities.DataContractToJson(response);
            string expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductResponse);

            // Compare the response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);

            // change the summary and update the product
            response.Product.Summary = "TEST_PRODUCT updated summary";
            // update the catalog id and recreate the manager           
            adminCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();
            var updateResponse = adminCatalogManager.SaveProduct(3, response.Product);
            responseJson = StringUtilities.DataContractToJson(updateResponse);
            expectedJson = StringUtilities.DataContractToJson(AdminResponses.ProductCatalogIdMismtachResponse);

            // Compare the update response to a static representation of the response
            Assert.AreEqual(expectedJson, responseJson);
        }

        #endregion

        #region Order Operations

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_OrderFulfillment()
        {
            // TODO: This is common code between Webstore and Admin integration tests -- consolidate
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

            var orderResponse = webStoreOrderManager.SubmitOrder(1, payment);

            // verify the order success
            Assert.IsTrue(orderResponse.Success);
            Assert.IsTrue(orderResponse.Order.Id > 0);
            Assert.AreEqual(OrderStatuses.Authorized, orderResponse.Order.Status);
            Assert.IsFalse(string.IsNullOrEmpty(orderResponse.Order.AuthorizationCode));
            
            // get orders to fulfill
            context.AuthToken = "my valid token";
            var fulfillmentManager = GetManager<IAdminFulfillmentManager>(context);

            var orderToFulfilleResponse = fulfillmentManager.GetOrdersToFulfill();

            Assert.IsTrue(orderToFulfilleResponse.Success);
            Assert.IsTrue(orderToFulfilleResponse.Orders.Length > 0);
            bool orderFound = false;
            foreach (var order in orderToFulfilleResponse.Orders)
            {
                if (order.Id == orderResponse.Order.Id)
                {
                    orderFound = true;
                }   
            }
            Assert.IsTrue(orderFound);

            // fulfill the order
            var fulfillmentResponse = fulfillmentManager.FulfillOrder(orderResponse.Order.Id);

            Assert.IsTrue(fulfillmentResponse.Success);
        }

        [TestMethod]
        [TestCategory("Integration-Admin")]
        public void Admin_OrderFulfillmentFailure()
        {
            // TODO: This is common code between Webstore and Admin integration tests -- consolidate
            // create the cart
            var context = new AmbientContext() { SellerId = 1, SessionId = Guid.NewGuid(), AuthToken = "my valid token" };
            var fulfillmentManager = GetManager<IAdminFulfillmentManager>(context);

            // fulfill the order
            var fulfillmentResponse = fulfillmentManager.FulfillOrder(0);

            Assert.IsFalse(fulfillmentResponse.Success);
            Assert.AreEqual("Invalid order for fulfillment", fulfillmentResponse.Message);
        }

        #endregion
    }
}
