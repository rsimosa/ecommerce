using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.Remittance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class SellerAccessorTests : DbTestAccessorBase
    {
        #region Helpers

        private ISellerAccessor CreateAccessor()
        {
            var accessor = new AccessorFactory(Context, new UtilityFactory(Context));
            var result = accessor.CreateAccessor<ISellerAccessor>();
            return result;
        }

        public Seller CreateSeller()
        {
            var accessor = CreateAccessor();
            var seller = new Seller()
            {
                Name = "UNIT TEST SELLER",
                IsApproved = true,
                UserName = "UNIT TEST USER",
            };
            var saved = accessor.Save(seller);
            return saved;
        }

        #endregion

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void SellerAccessor_Find_FindNone()
        {
            var accessor = CreateAccessor();

            var seller = accessor.Find(-1);
            Assert.IsNull(seller);

            seller = accessor.Find(99999);
            Assert.IsNull(seller);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void SellerAccessor_Seller_CRUD()
        {
            var accessor = CreateAccessor();
            var created = CreateSeller();
            Assert.IsTrue(created.Id > 0);

            var loaded = accessor.Find(created.Id);
            Assert.IsNotNull(loaded);
            Assert.AreEqual(created.Id, loaded.Id);

            loaded.Name = "UPDATED";
            var updated = accessor.Save(loaded);
            Assert.IsNotNull(updated);
            Assert.AreEqual(created.Id, updated.Id);
            Assert.AreEqual("UPDATED", updated.Name);

            accessor.Delete(updated.Id);

            var loadedFail = accessor.Find(updated.Id);
            Assert.IsNull(loadedFail);  
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        [ExpectedException(typeof(ArgumentException))]
        public void SellerAccessor_Seller_UpdateInvalid()
        {
            var accessor = CreateAccessor();
            var created = CreateSeller();
            Assert.IsTrue(created.Id > 0);

            var seller = new Seller()
            {
                Id = 99999
            };
            accessor.Save(seller);
        }
    }
}
