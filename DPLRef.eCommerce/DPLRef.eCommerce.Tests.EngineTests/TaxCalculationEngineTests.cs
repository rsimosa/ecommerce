using System;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Engines.Sales;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Tests.EngineTests
{
    [TestClass]
    public class TaxCalculationEngineTests : EngineTestBase
    {
        [TestMethod]
        [TestCategory("Engine Tests")]
        public void TaxCalculationEngine_CalculateCartTax()
        {
            WebStoreCart wsCart = new WebStoreCart()
            {
                Id = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                SubTotal = 15.00m,
                Total = 15.00m,
                CartItems = new WebStoreCartItem[]
                {
                    new WebStoreCartItem()
                    {
                        ProductId = 1,
                        ProductName = "Test Product 1",
                        UnitPrice = 5.00m,
                        Quantity = 2,
                        ExtendedPrice = 10.00m
                    },
                    new WebStoreCartItem()
                    {
                        ProductId = 2,
                        ProductName = "Test Product 2",
                        UnitPrice = 5.00m,
                        Quantity = 1,
                        ExtendedPrice = 5.00m
                    },
                }
            };

            var taxCalculationEngine = new EngineFactory(new AmbientContext(), null, null).CreateEngine<ITaxCalculationEngine>();

            var result = taxCalculationEngine.CalculateCartTax(wsCart);

            Assert.AreEqual(15.00m, result.SubTotal);
            Assert.AreEqual(1.05m, result.TaxAmount);
            Assert.AreEqual(16.05m, result.Total);
        }
    }
}
