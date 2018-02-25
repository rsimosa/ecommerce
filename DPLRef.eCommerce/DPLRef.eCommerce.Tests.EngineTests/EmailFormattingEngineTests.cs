using System.IO;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Engines.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DPLRef.eCommerce.Tests.EngineTests
{
    [TestClass]
    public class EmailFormattingEngineTests : EngineTestBase
    {
        // Deployment Items reference:
        // http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.testtools.unittesting.deploymentitemattribute.aspx

        //[TestMethod]
        [TestCategory("Engine Tests")]
        [DeploymentItem(@"ExpectedResults\EmailBody.txt")]
        public void EmailFormattingEngine_FormatOrderEmailBody()
        {
            var engFactory = new EngineFactory(new AmbientContext(), null, null);
            var eng = engFactory.CreateEngine<IEmailFormattingEngine>();

            var order = new Order()
            {
                Id = 1,
                Status = OrderStatuses.Authorized,
                ShippingProvider = "UPS",
                TrackingCode = "1234abcd",
                OrderLines = new OrderLine[]
                {
                    new OrderLine()
                    {
                        ProductName = "My Product",
                        Quantity = 2,
                        UnitPrice = 5.99m,
                        ExtendedPrice = 10.00m
                    }
                },
                SubTotal = 10.00m,
                TaxAmount = 0.70m,
                Total = 10.70m
            };

            var seller = new Seller()
            {
                Name = "My Seller Name"
            };

            var emailBody = eng.FormatOrderEmailBody(order, seller);
            Assert.IsTrue(File.Exists(@"ExpectedResults\EmailBody.txt"));
            Assert.AreEqual(File.ReadAllText(@"ExpectedResults\EmailBody.txt"), emailBody);
        }
    }
}
