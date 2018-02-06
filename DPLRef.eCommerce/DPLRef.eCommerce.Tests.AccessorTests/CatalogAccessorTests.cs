using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.Catalog;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class CatalogAccessorTests : DbTestAccessorBase
    {
        #region Helpers

        private const int catalogId = 2;
        private const int sellerId = 1;

        private Product CreateProduct(WebStoreCatalog catalog)
        {
            var accessor = CreateCatalogAccessor();
            var product = new Product()
            {
                Name = "UNIT TEST PRODUCT",
                CatalogId = catalog.Id,
                IsDownloadable = true,
                IsAvailable = true,
                Price = 1.5m
            };
            var saved = accessor.SaveProduct(catalog.Id, product);
            return saved;
        }

        #endregion

        #region Catalog CRUD

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_Find_FindNone()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = accessor.Find(99999);
            Assert.IsNull(catalog);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_FindAllProductsForCatalog_None()
        {
            var accessor = CreateCatalogAccessor();
            var products = accessor.FindAllProductsForCatalog(99999);
            Assert.IsNotNull(products);
            Assert.AreEqual(0, products.Length);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_FindAllProduct_None()
        {
            var accessor = CreateCatalogAccessor();
            var product = accessor.FindProduct(99999);
            Assert.IsNull(product);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_Catalog_CRUD()
        {
            var accessor = CreateCatalogAccessor();
            var insertSaved = CreateCatalog();
            Assert.IsNotNull(insertSaved);
            Assert.IsTrue(insertSaved.Id > 0);
            Assert.AreNotEqual("", insertSaved.SellerName);
            Assert.IsTrue(insertSaved.IsApproved);
            Assert.AreNotEqual("", insertSaved.Description);
            Assert.AreNotEqual("", insertSaved.Name);

            var loaded = accessor.Find(insertSaved.Id);
            Assert.IsNotNull(loaded);
            Assert.IsTrue(loaded.Id > 0);
            Assert.AreEqual(insertSaved.Id, loaded.Id);

            loaded.Name = "LOADED";
            var updateSaved = accessor.SaveCatalog(loaded);
            Assert.IsNotNull(updateSaved);
            Assert.IsTrue(updateSaved.Id > 0);
            Assert.IsNotNull(updateSaved.SellerName);
            Assert.AreNotEqual("", updateSaved.SellerName);

            var loaded2 = accessor.Find(loaded.Id);
            Assert.AreEqual("LOADED", loaded2.Name);
            Assert.IsNotNull(loaded2.SellerName);

            accessor.DeleteCatalog(loaded.Id);

            var loaded3 = accessor.Find(loaded.Id);
            Assert.IsNull(loaded3);
        }


        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_Catalog_NoSellerId()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = new WebStoreCatalog()
            {
                Name = "UNIT TEST CATALOG",
                SellerId = 0,
                SellerName = "UNIT TEST SELLER"
            };
            var saved = accessor.SaveCatalog(catalog);

            Assert.IsNotNull(saved);
            Assert.AreEqual(1, saved.SellerId);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Seller Id mismatch")]
        public void CatalogAccessor_Catalog_Delete()
        {
            var accessor = CreateCatalogAccessor();
            var insertSaved = CreateCatalog();
            accessor.DeleteCatalog(-1);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_FindAllSellerCatalogs()
        {
            var accessor = CreateCatalogAccessor();
            var before = accessor.FindAllSellerCatalogs();
            var insertSaved = CreateCatalog();
            var after = accessor.FindAllSellerCatalogs();
            Assert.AreEqual(before.Length + 1, after.Length);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Seller Id mismatch")]
        public void CatalogAccessor_SaveCatalogSellerIdMismatch()
        {
            var saved = CreateCatalog(); // create catalog for Seller Id 1
            var accessor = CreateCatalogAccessor(2); // create new accessor with seller id == 2
            accessor.SaveCatalog(saved);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_ShowCatalog()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = accessor.Find(1);
            Assert.IsNotNull(catalog);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_ShowCatalog_BadSeller()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = accessor.Find(1);
            Assert.IsNotNull(catalog);
        }

        #endregion

        #region Product

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_Product_CRUD()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = CreateCatalog();
            var saved = CreateProduct(catalog);
            Assert.IsNotNull(saved);
            Assert.IsTrue(saved.Id > 0);

            var loaded = accessor.FindProduct(saved.Id);
            Assert.IsNotNull(loaded);
            Assert.IsTrue(loaded.Id > 0);
            Assert.AreEqual(saved.Id, loaded.Id);
            Assert.AreEqual(saved.Price, loaded.Price);

            loaded.Name = "LOADED";
            accessor.SaveProduct(catalog.Id, loaded);
            var loaded2 = accessor.FindProduct(loaded.Id);
            Assert.AreEqual("LOADED", loaded.Name);

            // rebuild the accessor to update the context with the new catalog id
            AmbientContext context = new AmbientContext()
            {
            };
            accessor = new AccessorFactory(context, new UtilityFactory(context)).CreateAccessor<ICatalogAccessor>();
            var products = accessor.FindAllProductsForCatalog(catalog.Id);
            Assert.IsNotNull(products);
            Assert.AreEqual(1, products.Length);

            accessor.DeleteProduct(catalog.Id, loaded.Id);

            var loaded3 = accessor.FindProduct(loaded.Id);
            Assert.IsNull(loaded3);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CatalogAccessor_Product_NoCatalogId()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = CreateCatalog();

            var product = new Product()
            {
                Name = "UNIT TEST PRODUCT",
                CatalogId = 0,
                IsDownloadable = true,
                IsAvailable = true
            };
            var saved = accessor.SaveProduct(catalog.Id, product);
            Assert.IsNotNull(saved);
            Assert.IsTrue(saved.Id > 0);

        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Catalog Id mismatch")]
        public void CatalogAccessor_Product_Save_Null_Exception()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = CreateCatalog();
            var saved = CreateProduct(catalog);
            saved.Id = 999999;
            accessor.SaveProduct(catalog.Id, saved);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Catalog Id mismatch")]
        public void CatalogAccessor_Product_Save_Mismatch_Exception()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = CreateCatalog();
            var saved = CreateProduct(catalog);
            saved.CatalogId = -1;
            var loaded = accessor.SaveProduct(catalog.Id, saved);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Catalog Id mismatch")]
        public void CatalogAccessor_Product_Delete_ContextId_Exception()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = CreateCatalog();
            var saved = CreateProduct(catalog);

            accessor.DeleteProduct(-1, saved.Id);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Catalog Id mismatch")]
        public void CatalogAccessor_Product_Delete_Null_Exception()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = CreateCatalog();
            var saved = CreateProduct(catalog);
            accessor.DeleteProduct(catalog.Id, -1);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException), "Catalog Id mismatch")]
        public void CatalogAccessor_SaveProductCatalogIdMismatch()
        {
            var catalog = CreateCatalog();
            var saved = CreateProduct(catalog);
            var accessor = CreateCatalogAccessor();
            accessor.SaveProduct(-1, saved);
        }

        #endregion
    }
}
