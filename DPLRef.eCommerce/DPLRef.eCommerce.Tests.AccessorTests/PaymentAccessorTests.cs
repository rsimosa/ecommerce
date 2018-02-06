using DPLRef.eCommerce.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class PaymentAccessorTests : DbTestAccessorBase
    {

        private IPaymentAccessor CreatePaymentAccessor(int sellerId = 1)
        {
            var accessor = new AccessorFactory(Context, new UtilityFactory(Context));
            var result = accessor.CreateAccessor<IPaymentAccessor>();
            return result;
        }

        
        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void PaymentAccessor_AuthSuccess()
        {
            var accessor = CreatePaymentAccessor();
            var result = accessor.Authorize(new PaymentInstrument() { AccountNumber = "4012888888881881", VerificationCode = 123, ExpirationDate = "2019-01-01", PaymentType = PaymentTypes.CreditCard}, 1, 100.00M);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void PaymentAccessor_AuthFail()
        {
            var accessor = CreatePaymentAccessor();
            var result = accessor.Authorize(new PaymentInstrument() { AccountNumber = "5012888888881881", VerificationCode = 123, ExpirationDate = "2019-01-01", PaymentType = PaymentTypes.CreditCard }, 1, 100.00M);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void PaymentAccessor_CaptureSuccess()
        {
            var accessor = CreatePaymentAccessor();
            var result = accessor.Capture("valid_auth_code");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void PaymentAccessor_CaptureFail()
        {
            var accessor = CreatePaymentAccessor();
            var result = accessor.Capture("invalid_auth_code");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }
    }
}
