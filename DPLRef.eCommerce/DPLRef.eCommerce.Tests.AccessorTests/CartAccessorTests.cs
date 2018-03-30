using System;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class CartAccessorTests : DbTestAccessorBase
    {
        private const int catalogId = 2;
        private Cart CreateCartWithBillingAddress()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());
            var cart = new Cart()
            {               
                BillingAddress = billing
            };
            var catalog = CreateCatalog();            
            var saved = accessor.SaveBillingInfo(catalogId, cart.BillingAddress);
            return saved;
        }

        Address billing = new Address()
        {
            First = "Melody",
            Last = "Ensign",
            Addr1 = "3427 Berkley Street",
            City = "Lincoln",
            State = "Nebraska",
            Postal = "68508",
        };

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_Find_FindNone()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());
            var catalog = accessor.ShowCart(catalogId);
            Assert.IsNull(catalog);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_Find_FindOne()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var catalog = accessor.ShowCart(catalogId);
            Assert.IsNotNull(catalog);
            Assert.IsNotNull(catalog.BillingAddress);
            Assert.AreEqual(billing.Addr1, catalog.BillingAddress.Addr1);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_SaveShipping()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var catalog = accessor.ShowCart(catalogId);
            catalog.ShippingAddress = new Address()
            {
                First = "first",
                Last = "last", 
            };

            var saved = accessor.SaveShippingInfo(catalogId, catalog.ShippingAddress);

            Assert.IsNotNull(saved);
            Assert.IsNotNull(saved.ShippingAddress);
            Assert.AreEqual("first", saved.ShippingAddress.First);
            Assert.AreEqual("last", saved.ShippingAddress.Last);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_SaveBilling()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var cart = accessor.ShowCart(catalogId);
            cart.BillingAddress = new Address()
            {
                First = "first",
                Last = "last",
            };

            var saved = accessor.SaveBillingInfo(catalogId, cart.BillingAddress);

            Assert.IsNotNull(saved);
            Assert.IsNotNull(saved.BillingAddress);
            Assert.AreEqual("first", saved.BillingAddress.First);
            Assert.AreEqual("last", saved.BillingAddress.Last);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_SaveShippingAndBilling()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var catalog = accessor.ShowCart(catalogId);
            catalog.ShippingAddress = new Address()
            {
                First = "first1",
                Last = "last1",
            };

            var saved1 = accessor.SaveShippingInfo(catalogId, catalog.ShippingAddress);

            Assert.IsNotNull(saved1);
            Assert.IsNotNull(saved1.ShippingAddress);
            Assert.AreEqual("first1", saved1.ShippingAddress.First);
            Assert.AreEqual("last1", saved1.ShippingAddress.Last);

            saved1.BillingAddress = new Address()
            {
                First = "first2",
                Last = "last2",
            };

            var saved2 = accessor.SaveBillingInfo(catalogId, saved1.BillingAddress);

            Assert.IsNotNull(saved2);
            Assert.IsNotNull(saved2.BillingAddress);
            Assert.AreEqual("first2", saved2.BillingAddress.First);
            Assert.AreEqual("last2", saved2.BillingAddress.Last);

            Assert.AreEqual("first1", saved2.ShippingAddress.First);
            Assert.AreEqual("last1", saved2.ShippingAddress.Last);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_SaveCartItem_Single()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var catalog = accessor.ShowCart(catalogId);

            var saved = accessor.SaveCartItem(catalogId, 2, 1001);

            Assert.IsNotNull(saved);
            Assert.IsNotNull(saved.CartItems);
            Assert.AreEqual(1, saved.CartItems.Length);
            Assert.AreEqual(2, saved.CartItems[0].ProductId);
            Assert.AreEqual(1001, saved.CartItems[0].Quantity);
            Assert.IsNotNull(saved.CartItems[0].ProductName);
            Assert.AreEqual("1997 Volvo 960", saved.CartItems[0].ProductName);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_SaveCartItem_Double()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var catalog = accessor.ShowCart(catalogId);

            var saved1 = accessor.SaveCartItem(catalogId, 2, 1);
            var saved2 = accessor.SaveCartItem(catalogId, 1, 1);

            Assert.IsNotNull(saved2);
            Assert.IsNotNull(saved2.CartItems);
            Assert.AreEqual(2, saved2.CartItems.Length);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccesor_SaveCartItem_NoAddress()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());
            var cart = new Cart()
            {
                BillingAddress = billing
            };
            var catalog = CreateCatalog();

            // create cart item with no address
            var saved = accessor.SaveCartItem(catalogId, 2, 1);

            Assert.IsNotNull(saved);
            Assert.IsNotNull(saved.CartItems);
            Assert.AreEqual(1, saved.CartItems.Length);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccesor_RemoveItem_WithCartItems()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());
            
            var catalog = CreateCatalog();

            // create cart item with no address
            var saved = accessor.SaveCartItem(catalogId, 2, 1);

            Assert.IsNotNull(saved);
            Assert.IsNotNull(saved.CartItems);
            Assert.AreEqual(1, saved.CartItems.Length);

            // now delete the cart
            Assert.IsTrue(accessor.DeleteCart(catalogId));

            var loaded = accessor.ShowCart(catalogId);
            Assert.IsNull(loaded);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void CartAccessor_RemoveItem_NoCartItems()
        {
            var accessor = CreateCartAccessor(Guid.NewGuid());

            CreateCartWithBillingAddress();

            var cart = accessor.ShowCart(catalogId);
            cart.BillingAddress = new Address()
            {
                First = "first",
                Last = "last",
            };

            var saved = accessor.SaveBillingInfo(catalogId, cart.BillingAddress);

            Assert.IsNotNull(saved);
                     
            // now delete the cart
            Assert.IsTrue(accessor.DeleteCart(catalogId));

            var loaded = accessor.ShowCart(catalogId);
            Assert.IsNull(loaded);

        }
    }
}
