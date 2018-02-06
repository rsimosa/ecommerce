using System;
using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using catAcc = DPLRef.eCommerce.Accessors.Catalog;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Engines.Sales;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Utilities;
using DPLRef.eCommerce.Tests.Mocks;

namespace DPLRef.eCommerce.Tests.EngineTests
{
    [TestClass]
    public class PricingEngineTests : EngineTestBase
    {
        private Guid _mySessionId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

        #region Helpers

        MockData mockData = new MockData()
        {
            Context = new AmbientContext()
        };

        private ICartPricingEngine GetPricingEngine()
        {
            EngineFactory engFactory = new EngineFactory(mockData.Context, null, null);
            return engFactory.CreateEngine<ICartPricingEngine>(SetupMockAccessorFactory(), null);
        }

        private AccessorFactory SetupMockAccessorFactory()
        {

            MockCatalogAccessor mockAccessor = new MockCatalogAccessor(this.mockData);

            mockData.Products.Clear();
            mockData.Products.Add(new Product()
            {
                Id = 1,
                Price = 1.50m
            });
            mockData.Products.Add(new Product()
            {
                Id = 2,
                Price = 2.00m
            });

            AccessorFactory accFactory = new AccessorFactory(mockData.Context, new UtilityFactory(new AmbientContext()));
            accFactory.AddOverride<catAcc.ICatalogAccessor>(mockAccessor);
            return accFactory;
        }

        #endregion

        [TestMethod]
        [TestCategory("Engine Tests")]
        [ExpectedException(typeof(ArgumentException), "Invalid Product Id")]
        public void PricingEngine_GenerateCartPricingInvalidProducId()
        {
            // test for invalid product id
            var pricingEngine = GetPricingEngine();

            Cart storedCart = new Cart()
            {
                Id = _mySessionId,
                CartItems = new CartItem[]
                {
                    new CartItem()
                    {
                        ProductId = -1,
                        ProductName = "Test Product 1",
                        Quantity = 3
                    }
                }
            };

            var wsCart = pricingEngine.GenerateCartPricing(storedCart);
        }

        [TestMethod]
        [TestCategory("Engine Tests")]
        [ExpectedException(typeof(ArgumentException), "Invalid item quantity")]
        public void PricingEngine_GenerateCartPricingInvalidQuantity()
        {
            // test for invalid quantity
            var pricingEngine = GetPricingEngine();

            Cart storedCart = new Cart()
            {
                Id = _mySessionId,
                CartItems = new CartItem[]
                {
                    new CartItem()
                    {
                        ProductId = 1,
                        ProductName = "Test Product 1",
                        Quantity = 0
                    }
                }
            };

            var wsCart = pricingEngine.GenerateCartPricing(storedCart);
        }

        [TestMethod]
        [TestCategory("Engine Tests")]
        public void PricingEngine_GenerateCartPricing()
        {
            var pricingEngine = GetPricingEngine();

            Cart storedCart = new Cart()
            {
                Id = _mySessionId,
                CartItems = new CartItem[]
                {
                    new CartItem()
                    {
                        ProductId = 1,
                        ProductName = "Test Product 1",
                        Quantity = 3
                    },
                    new CartItem()
                    {
                        ProductId = 2,
                        ProductName = "Test Product 2",
                        Quantity = 2
                    }
                }
            };

            var wsCart = pricingEngine.GenerateCartPricing(storedCart);

            // Cart
            Assert.AreEqual(_mySessionId, wsCart.Id);
            Assert.AreEqual(8.50m, wsCart.SubTotal);
            Assert.AreEqual(8.50m, wsCart.Total);
            Assert.AreEqual(2, wsCart.CartItems.Length);
            //Cart Item 1
            Assert.AreEqual(1, wsCart.CartItems[0].ProductId);
            Assert.AreEqual("Test Product 1", wsCart.CartItems[0].ProductName);
            Assert.AreEqual(1.50m, wsCart.CartItems[0].UnitPrice);
            Assert.AreEqual(3, wsCart.CartItems[0].Quantity);
            Assert.AreEqual(4.50m, wsCart.CartItems[0].ExtendedPrice);
            //Cart Item 2
            Assert.AreEqual(2, wsCart.CartItems[1].ProductId);
            Assert.AreEqual("Test Product 2", wsCart.CartItems[1].ProductName);
            Assert.AreEqual(2.00m, wsCart.CartItems[1].UnitPrice);
            Assert.AreEqual(2, wsCart.CartItems[1].Quantity);
            Assert.AreEqual(4.00m, wsCart.CartItems[1].ExtendedPrice);
        }
    }
}
