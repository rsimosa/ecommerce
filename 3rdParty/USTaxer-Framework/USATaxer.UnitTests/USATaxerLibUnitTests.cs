using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace USATaxer.UnitTests
{
    [TestClass]
    public class USATaxerLibUnitTests
    {
        [TestMethod]
        public void USATaxer_Initialized()
        {
            var taxer = new USATaxerLib();
            taxer.Init();

            Assert.AreEqual(0.0725m, taxer.Rate("68512"));

            Assert.AreEqual(0.065m, taxer.Rate("68031"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void USATaxer_NotInitialized()
        {
            var taxer = new USATaxerLib();
            taxer.Rate("68512");
        }
    }
}
