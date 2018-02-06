using DPLRef.eCommerce.Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Engines.Remmitance;

namespace DPLRef.eCommerce.Tests.EngineTests
{
    [TestClass]
    public class RemittanceCalculationEngineTests : EngineTestBase
    {
        [TestMethod]
        [TestCategory("Engine Tests")]
        public void RemittanceCalculationEngine_CalculateFee()
        {
            var engine = new EngineFactory(new AmbientContext(), null, null).CreateEngine<IRemittanceCalculationEngine>();
            var result = engine.CalculateFee(0, 100.0M);
            Assert.AreEqual(10.0M, result.FeeAmount);
            Assert.AreEqual(90.0M, result.RemittanceAmount);
        }
    }
}
