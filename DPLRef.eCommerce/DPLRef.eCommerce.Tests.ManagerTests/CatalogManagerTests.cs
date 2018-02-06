using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Accessors;
using acc = DPLRef.eCommerce.Accessors.Catalog;
using Admin = DPLRef.eCommerce.Contracts.Admin.Catalog;
using WebStore = DPLRef.eCommerce.Contracts.WebStore.Catalog;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Utilities;
using DPLRef.eCommerce.Tests.Mocks;

namespace DPLRef.eCommerce.Tests.ManagerTests
{
    [TestClass]
    public class CatalogManagerTests : ManagerTestBase
    {
        #region Test Data

        MockData mockData = new MockData()
        {
            Context = new AmbientContext()
            {
                AuthToken = "something"
            }
        };

        #endregion

        #region Factories

        MockCatalogAccessor MockCatalogAccessor { get; set; }

        public eCommerce.Contracts.WebStore.Catalog.IWebStoreCatalogManager CreateWebStoreCatalogManager()
        {
            var factory = new ManagerFactory(mockData.Context);                       
            var manager = factory
                .CreateManager<eCommerce.Contracts.WebStore.Catalog.IWebStoreCatalogManager>(
                    null, SetupMockAccessorFactory(), SetupMockUtilityFactory());
            return manager;
        }
        public eCommerce.Contracts.Admin.Catalog.IAdminCatalogManager CreateAdminCatalogManager()
        {
            var factory = new ManagerFactory(mockData.Context);
            var manager = factory
                .CreateManager<eCommerce.Contracts.Admin.Catalog.IAdminCatalogManager>(
                    null, SetupMockAccessorFactory(), SetupMockUtilityFactory());
            return manager;
        }

        public AccessorFactory SetupMockAccessorFactory()
        {
            var accFactory = new AccessorFactory(mockData.Context, SetupMockUtilityFactory());
            MockCatalogAccessor = new MockCatalogAccessor(mockData);
            accFactory.AddOverride<acc.ICatalogAccessor>(MockCatalogAccessor);
            return accFactory;
        }

        public UtilityFactory SetupMockUtilityFactory()
        {
            UtilityFactory utilFactory = new UtilityFactory(mockData.Context);
            MockSecurityUtility mockSecurity = new MockSecurityUtility(mockData);
            utilFactory.AddOverride<ISecurityUtility>(mockSecurity);
            return utilFactory;
        }
        

        private Admin.WebStoreCatalog CreateWebStoreCatalog(int catalogId = 0)
        {
            return new Admin.WebStoreCatalog()
            {
                Id = catalogId,
                Description = "UNIT_TEST_CATALOG description",
                Name = "UNIT_TEST_CATALOG"
            };
        }
        #endregion

        #region IWebStoreCatalogManager

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_WebStoreShowCatalogEmpty()
        {
            var mgr = CreateWebStoreCatalogManager();
            var response = mgr.ShowCatalog(3);

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Catalog);
            Assert.AreEqual("Catalog not found", response.Message);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_WebStoreShowCatalog()
        {
            var mgr = CreateWebStoreCatalogManager();
            var response = mgr.ShowCatalog(1);
            WebStore.WebStoreCatalog catalog = response.Catalog;

            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, catalog.Id);
            Assert.AreEqual(2, catalog.Products.Length);
            Assert.AreEqual("My Product", catalog.Products[0].Name);
            Assert.AreEqual("My Second Product", catalog.Products[1].Name);
            Assert.AreEqual("My Webstore", catalog.Name);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_WebStoreShowCatalog_InternalException()
        {          
            var mgr = CreateWebStoreCatalogManager();

            var response = mgr.ShowCatalog(99);
            WebStore.WebStoreCatalog catalog = response.Catalog;

            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_WebStoreShowProduct()
        {
            var mgr = CreateWebStoreCatalogManager();
            var response = mgr.ShowProduct(1, 1);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Product.Id);
            Assert.AreEqual(response.Product.Name, "My Product");

            response = mgr.ShowProduct(1, -1);

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Product);
            Assert.AreEqual(response.Message, "Product not found");
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_WebStoreShowProduct_InternalException()
        {
            var mgr = CreateWebStoreCatalogManager();

            var response = mgr.ShowProduct(1, 99);
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Product);
        }
        #endregion  

        #region IAdminCatalogManager
        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminFindCatalogListEmpty()
        {
            var mgr = CreateAdminCatalogManager();
            mockData.Catalogs.Clear();
            var response = mgr.FindCatalogs();
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.Catalogs.Length);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminFindCatalogList()
        {
            var mgr = CreateAdminCatalogManager();
            var response = mgr.FindCatalogs();

            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Catalogs.Length);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminFindCatalogListNoAuth()
        {
            mockData.Context.AuthToken = null;
            var mgr = CreateAdminCatalogManager();
            var response = mgr.FindCatalogs();

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Seller not authenticated", response.Message);
            Assert.IsNull(response.Catalogs);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminFindCatalog_InternalException()
        {
            var mgr = CreateAdminCatalogManager();
            mockData.Catalogs = null;
            var response = mgr.FindCatalogs();
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminShowCatalogEmpty()
        {
            var mgr = CreateAdminCatalogManager();
            var response = mgr.ShowCatalog(-1);

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Catalog);
            Assert.AreEqual("Catalog not found", response.Message);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminShowCatalog()
        {
            var mgr = CreateAdminCatalogManager();
            var response = mgr.ShowCatalog(1);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Catalog.Id);
            Assert.AreEqual("My Webstore", response.Catalog.Name);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminShowCatalogNoAuth()
        {
            mockData.Context.AuthToken = null;
            var mgr = CreateAdminCatalogManager();
            var response = mgr.ShowCatalog(1);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Seller not authenticated", response.Message);
            Assert.IsNull(response.Catalog);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminShowCatalog_InternalException()
        {
            var mgr = CreateAdminCatalogManager();
            mockData.Context.AuthToken = null;
            var response = mgr.ShowCatalog(1);
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Catalog);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminSaveCatalogNoAuth()
        {
            mockData.Context.AuthToken = null;
            var mgr = CreateAdminCatalogManager();
            var response = mgr.SaveCatalog(CreateWebStoreCatalog());
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Seller not authenticated", response.Message);
            Assert.IsNull(response.Catalog);
        }


        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminSaveCatalog()
        {
            var catalog = CreateWebStoreCatalog();
            var mgr = CreateAdminCatalogManager();
            var response = mgr.SaveCatalog(catalog);
            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.Catalog);
        }

        [TestMethod]
        [TestCategory("Managers-Admin")]
        public void CatalogManager_AdminSaveCatalog_Failed()
        {
            mockData.Context.AuthToken = null;
            var mgr = CreateAdminCatalogManager();
            mockData.Products = null;
            var response = mgr.SaveCatalog(CreateWebStoreCatalog());
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_AdminShowProduct()
        {
            var mgr = CreateAdminCatalogManager();
            var response = mgr.ShowProduct(1, 1);
            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.Product);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_AdminShowProduct_InternalException()
        {
            mockData.Products = null;
            var mgr = CreateAdminCatalogManager();
            var response = mgr.ShowProduct(1, 1);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_AdminSaveProduct()
        {
            var mgr = CreateAdminCatalogManager();

            var response = mgr.SaveProduct(
                1,
                new Contracts.Admin.Catalog.Product()
                {
                    Detail = "UNIT TEST PRODUCT",
                    IsAvailable = true,
                    Price = 10.0m,
                    ShippingWeight = 10,
                });

            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.Product);
        }

        [TestMethod]
        [TestCategory("Managers-WebStore")]
        public void CatalogManager_AdminSaveProduct_Fail()
        {
            mockData.Products = null;
            var mgr = CreateAdminCatalogManager();

            var response = mgr.SaveProduct(1, new Contracts.Admin.Catalog.Product()
            {
                Detail = "UNIT TEST PRODUCT",
                IsAvailable = true,
                Price = 10.0m,
                ShippingWeight = 10,
            });

            Assert.IsFalse(response.Success);
        }

        #endregion
    }
}
