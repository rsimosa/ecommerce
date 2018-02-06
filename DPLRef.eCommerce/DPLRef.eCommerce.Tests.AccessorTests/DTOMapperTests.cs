using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class DTOMapperTests
    {
        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void DTOMapper_IsDTOMapperConfigValid()
        {
            DTOMapper.Configuration.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void DTOMapper_CartMap_1()
        {
            var input = new Accessors.EntityFramework.Cart
            {
                BillingCity = "Bill City",
                BillingPostal = "Bill Postal",
                BillingAddr1 = "Bill Addr1",
                BillingAddr2 = "Bill Addr2",

                ShippingCity = "Shipping City",
                ShippingPostal = "Shipping Postal",
                ShippingAddr1 = "Shipping Addr1",
                ShippingAddr2 = "Shipping Addr2",
            };
            var result = DTOMapper.Map<Cart>(input);

            Assert.AreEqual(input.BillingCity, result.BillingAddress.City);
            Assert.AreEqual(input.BillingPostal, result.BillingAddress.Postal);
            Assert.AreEqual(input.BillingAddr1, result.BillingAddress.Addr1);
            Assert.AreEqual(input.BillingAddr2, result.BillingAddress.Addr2);
            Assert.AreEqual(input.BillingState, result.BillingAddress.State);

            Assert.AreEqual(input.ShippingCity, result.ShippingAddress.City);
            Assert.AreEqual(input.ShippingPostal, result.ShippingAddress.Postal);
            Assert.AreEqual(input.ShippingAddr1, result.ShippingAddress.Addr1);
            Assert.AreEqual(input.ShippingAddr2, result.ShippingAddress.Addr2);
            Assert.AreEqual(input.ShippingState, result.ShippingAddress.State);

        }

        [TestMethod]
        public void DTOMapper_CartMap_2()
        {
            var input = new Cart
            {
                BillingAddress = new Address
                {
                    City = "Bill City",
                    Addr1 = "addr1",
                    Addr2 = "addr2",
                    Postal = "zip",
                    State = "state"
                },
                ShippingAddress = new Address
                {
                    City = "Ship City",
                    Addr1 = "addr1",
                    Addr2 = "addr2",
                    Postal = "zip",
                    State = "state"
                }
            };

            var result = DTOMapper.Map<Accessors.EntityFramework.Cart>(input);

            Assert.AreEqual(input.BillingAddress.City, result.BillingCity);
            Assert.AreEqual(input.BillingAddress.Addr1, result.BillingAddr1);
            Assert.AreEqual(input.BillingAddress.Addr2, result.BillingAddr2);
            Assert.AreEqual(input.BillingAddress.Postal, result.BillingPostal);
            Assert.AreEqual(input.BillingAddress.State, result.BillingState);

            Assert.AreEqual(input.ShippingAddress.City, result.ShippingCity);
            Assert.AreEqual(input.ShippingAddress.Addr1, result.ShippingAddr1);
            Assert.AreEqual(input.ShippingAddress.Addr2, result.ShippingAddr2);
            Assert.AreEqual(input.ShippingAddress.Postal, result.ShippingPostal);
            Assert.AreEqual(input.ShippingAddress.State, result.ShippingState);

        }

    }
}
